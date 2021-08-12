﻿using BLL;
using BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : AbstractController
    {
        public DataController(IConfiguration configuration, ILogger<DataController> logger) : base(configuration, logger)
        {

        }
        protected MessageData<dynamic> CallMethode(string assemblyName, string className, string methodeName, object[] paramenter)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType(assemblyName + "." + className + "Logic");
            var instance = Activator.CreateInstance(type, new List<object> { _connectString }.ToArray());
            MethodInfo method = type.GetMethod(methodeName);
            return method.Invoke(instance, paramenter) as MessageData<dynamic>;
        }
        protected MessageObject CallMethodeWithId(string assemblyName, string className, string methodeName, int id, DateTime updateTs)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType(assemblyName + "." + className + "Logic");
            var instance = Activator.CreateInstance(type, new List<object> { _connectString }.ToArray());
            MethodInfo method = type.GetMethod(methodeName);
            return method.Invoke(instance, new object[] { id, updateTs }) as MessageObject;
        }
        protected MessageObject CallMethodeWithFormData(string assemblyName, string className, string methodeName, IFormCollection formData, DateTime updateTs)
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
            return method.Invoke(instance, new object[] { dic, updateTs }) as MessageObject;
        }
        [HttpGet("all/{classname}/{sessionid}")]
        public IActionResult LoadAll(string classname, string sessionid)
        {
            DateTime updateTs = DateTime.UtcNow;
            OkObjectResult result = new OkObjectResult("OK");            
            try
            {
                string userId = UserLogic.Instance.GetUserIdAndUpdateUpdateTs(sessionid, updateTs);
                if (!string.IsNullOrEmpty(userId))
                {
                    object filePath = GetFileDir(userId);
                    object baseUrl = GetBaseUrl();
                    MessageData<dynamic> messageData = CallMethode("BLL", classname, "All", new object[] { sessionid, filePath, baseUrl, updateTs });
                    result.Value = messageData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, new object[0]);
                result.Value = new MessageObject(ex.Message, updateTs);

            }
            return result;
        }
        [HttpPost("{classname}")]
        public IActionResult Insert(string classname)
        {
            DateTime updateTs = DateTime.UtcNow;
            OkObjectResult result;
            try
            {
                MessageObject messageObj = CallMethodeWithFormData("BLL", classname, "Insert", Request.Form, updateTs);
                result = new OkObjectResult(messageObj);
            }
            catch (Exception ex)
            {
                MessageObject messageObject = new MessageObject(ex.Message, updateTs);
                result = new OkObjectResult(messageObject);
            }

            return result;
        }
        [HttpPut("{classname}")]
        public IActionResult Update(string classname)
        {
            DateTime updateTs = DateTime.UtcNow;
            OkObjectResult result = new OkObjectResult("OK");
            try
            {
                MessageObject messageObj = CallMethodeWithFormData("BLL", classname, "Update", Request.Form, updateTs);
                result.Value = messageObj;
            }
            catch (Exception ex)
            {
                result.Value = new MessageObject(ex.Message, updateTs);
            }

            return result;
        }
        [HttpDelete("{classname}/{id}")]
        public IActionResult Delete(string classname, int id)
        {
            DateTime updateTs = DateTime.UtcNow;
            OkObjectResult result = new OkObjectResult("OK");
            try
            {
                MessageObject messageObj = CallMethodeWithId("BLL", classname, "DeleteWithId", id, updateTs);
                result.Value = messageObj;
            }
            catch (Exception ex)
            {
                result.Value = new MessageObject(ex.Message, updateTs);
            }

            return result;
        }
    }
}
