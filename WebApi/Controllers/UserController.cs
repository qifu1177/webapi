using BLL;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : AbstractController
    {
        public UserController(IConfiguration configuration, ILogger<DataController> logger) : base(configuration, logger)
        {

        }
        [HttpGet("config")]
        public IActionResult GetUserSetting()
        {
            try
            {
                dynamic setting = ConfigService.Instance.GetUserSettings(_configuration);
                string str=Newtonsoft.Json.JsonConvert.SerializeObject(setting, Formatting.None);
                return Ok(str);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("login/{email}")]
        public IActionResult Login(string email)
        {
            try
            {
                string psw = Request.Headers["psw"];
                string str=UserLogic.Instance.SetConnectString(_connectString).Login(email, psw);
                return Ok(str);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
            
        }
        [HttpPost("register")]
        public IActionResult Register()
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (string k in Request.Form.Keys)
                {
                    StringValues sv = new StringValues();
                    Request.Form.TryGetValue(k, out sv);
                    dic.Add(k, sv);
                }
                string str = UserLogic.Instance.SetConnectString(_connectString).Register(dic);
                return Ok(str);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }
    }
}
