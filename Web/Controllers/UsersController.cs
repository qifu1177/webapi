using Domain.Interfaces;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Help.Exceptions;
using Help.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : AbstractController<UsersController>
    {
        private IUserLogic _logic;
        public UsersController(IConfiguration configuration, ILogger<UsersController> logger, ITranslator translator, IUserLogic logic):base(configuration,logger, translator)
        {
            _logic = logic;
        }
        [HttpPost("login/{language}")]
        public IActionResult Login(string language,UserLoginRequest request)
        {
            return this.RequestHandler(language, () =>
            {
                UserLoginResponse response = _logic.Login(language, request);
                return Ok(response);
            });           

        }
        [HttpPost("register/{language}")]
        public IActionResult Register(string language, UserRegisterRequest request)
        {
            return this.RequestHandler(language, () =>
            {
                MessageResponse response=_logic.Register(language,request); ;
                return Ok(response);
            });

        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
