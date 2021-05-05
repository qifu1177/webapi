using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;

namespace WebApi.Controllers
{
    public abstract class AbstractController : ControllerBase
    {
        protected readonly ILogger<DataController> _logger;
        protected readonly IConfiguration _configuration;
        protected string _connectString;
        public AbstractController(IConfiguration configuration, ILogger<DataController> logger)
        {
            _configuration = configuration;
            _connectString = _configuration.GetConnectionString("DB");
            _logger = logger;
        }

        protected object CallMethode(string assemblyName, string className, string methodeName, object[] paramenter)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType(assemblyName + "." + className + "Logic");
            var instance = Activator.CreateInstance(type, new List<object> { _connectString }.ToArray());
            MethodInfo method = type.GetMethod(methodeName);
            return method.Invoke(instance, paramenter);
        }
        protected void CallMethodeWithId(string assemblyName, string className, string methodeName, int id)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType(assemblyName + "." + className + "Logic");
            var instance = Activator.CreateInstance(type, new List<object> { _connectString }.ToArray());
            MethodInfo method = type.GetMethod(methodeName);
            method.Invoke(instance, new object[] { id });
        }
        protected void CallMethodeWithFormData(string assemblyName, string className, string methodeName, IFormCollection formData)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType(assemblyName + "." + className + "Logic");
            var instance = Activator.CreateInstance(type, new List<object> { _connectString }.ToArray());
            MethodInfo method = type.GetMethod(methodeName);
            Dictionary<string, StringValues> dic = new Dictionary<string, StringValues>();
            foreach (string k in formData.Keys)
            {
                StringValues sv = new StringValues();
                formData.TryGetValue(k, out sv);
                dic.Add(k, sv);
            }
            method.Invoke(instance, new object[] { dic });
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("live");
        }
        [HttpGet("sessionid")]
        public IActionResult GetSessionId()
        {
            return Ok(Guid.NewGuid());
        }

        [HttpPost("file"), DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                string sessionId = Request.Form["sessionid"];
                var file = Request.Form.Files[0];
                string currentDir = Directory.GetCurrentDirectory();
                var fileDir = Path.Combine(currentDir, "files");
                if (file.Length > 0)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(fileDir);
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(fileDir, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok("OK");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                var fileDir = Path.Combine(currentDir, "files");
                string filePath = Path.Combine(fileDir, fileName);
                if(IsImage(filePath))
                {
                    var image = System.IO.File.OpenRead(filePath);
                    return File(image, "image/jpeg");
                }
                else
                {
                    FileStream stream = System.IO.File.OpenRead(filePath);
                    return File(stream, "application/octet-stream");
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"File is not found. Error: {ex}");
            }            
        }

        private bool IsImage(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension.EndsWith("jpg") || extension.EndsWith("jpeg") || extension.EndsWith("png") || extension.EndsWith("gif") || extension.EndsWith("bmp"))
                return true;
            return false;
        }
    }
}
