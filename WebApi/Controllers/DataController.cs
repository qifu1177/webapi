using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public DataController(IConfiguration configuration, ILogger<DataController> logger):base(configuration,logger)
        {

        }
        
        [HttpGet("all/{classname}")]
        public IActionResult LoadAll(string classname)
        {
            string jsonStr = "";
            try
            {
                jsonStr = CallMethode("BLL", classname, "All", new object[0]).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, new object[0]);
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return Ok(jsonStr);
        }
        [HttpPost("{classname}")]
        public IActionResult Insert(string classname)
        {
            try
            {
                CallMethodeWithFormData("BLL", classname, "Insert", Request.Form);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            return Ok("OK");
        }
        [HttpPut("{classname}")]
        public IActionResult Update(string classname)
        {
            try
            {
                CallMethodeWithFormData("BLL", classname, "Update", Request.Form);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            return Ok("OK");
        }
        [HttpDelete("{classname}/{id}")]
        public IActionResult Delete(string classname, int id)
        {
            try
            {
                CallMethodeWithId("BLL", classname, "DeleteWithId", id);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            return Ok("OK");
        }
    }
}
