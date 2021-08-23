using Domain.Interfaces;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Help.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UseradminController : AbstractController<UseradminController, UserRequest, UserListResponse>
    {
        public UseradminController(IConfiguration configuration, ILogger<UseradminController> logger, ITranslator translator, ILogic<UserRequest, UserListResponse> logic) : base(configuration, logger, translator, logic)
        {
            
        }
    }
}
