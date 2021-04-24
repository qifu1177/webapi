using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IConfiguration _configuration;
        private string _connectString;
        public DataController(IConfiguration configuration, ILogger<DataController> logger)
        {
            _configuration = configuration;
            _connectString = _configuration.GetConnectionString("DB");
            _logger = logger;
        }

        private object CallMethode(string assemblyName, string className, string methodeName, object[] paramenter)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType(assemblyName + "." + className+"Logic");
            var instance = Activator.CreateInstance(type, new List<object> { _connectString }.ToArray());
            MethodInfo method = type.GetMethod(methodeName);
            return method.Invoke(instance, paramenter);
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("live");
        }
        [HttpGet("all/{classname}")]
        public IActionResult LoadBlogs(string classname)
        {
            string jsonStr = "";
            try
            {
                jsonStr = CallMethode("BLL", classname, "All", new object[0]).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, new object[0]);
            }

            return Ok(jsonStr);
        }
        //[HttpPost("{classname}")]
        //public IActionResult InsertBlog(string classname, Blog data)
        //{
        //    try
        //    {
        //        using (BloggingContext context = new BloggingContext(_connectString))
        //        {
        //            //Blog blog = new Blog { Url = url };
        //            context.Blogs.Add(data);
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok("false");
        //    }

        //    return Ok("true");
        //}
        //[HttpPut("{classname}")]
        //public IActionResult UpdateBlog(string classname, Blog data)
        //{
        //    try
        //    {
        //        using (BloggingContext context = new BloggingContext(_connectString))
        //        {
        //            //Blog blog = new Blog { Url = url };
        //            var blog = context.Blogs.First(a => a.BlogId == data.BlogId);
        //            blog.Url = data.Url;
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok("false");
        //    }

        //    return Ok("true");
        //}
        //[HttpDelete("{classname}/{id}")]
        //public IActionResult DeleteBlog(string classname, int blogId)
        //{
        //    try
        //    {
        //        using (BloggingContext context = new BloggingContext(_connectString))
        //        {
        //            context.Remove(context.Blogs.Single(a => a.BlogId == blogId));
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok("false");
        //    }

        //    return Ok("true");
        //}
    }
}
