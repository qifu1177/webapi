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
    public class UsersController : AbstractController<UsersController,UserRequest,UserResponse>
    {
        private IUserLogic<UserRequest,UserResponse> _userLogic;
        public UsersController(IConfiguration configuration, ILogger<UsersController> logger, ITranslator translator, IUserLogic<UserRequest,UserResponse> logic):base(configuration,logger, translator,logic)
        {
            _userLogic = logic;
        }
        [HttpPost("login/{language}")]
        public IActionResult Login(string language,UserLoginRequest request)
        {
            return this.RequestHandler(language, () =>
            {
                UserLoginResponse response = _userLogic.Login(language, request);
                return Ok(response);
            });           

        }
        [HttpPost("register/{language}")]
        public IActionResult Register(string language, UserRegisterRequest request)
        {
            return this.RequestHandler(language, () =>
            {
                MessageResponse response= _userLogic.Register(language,request); ;
                return Ok(response);
            });

        }
        
    }
}
