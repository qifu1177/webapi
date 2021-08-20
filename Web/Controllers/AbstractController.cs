using Domain.Interfaces;
using Domain.Models.Responses;
using Help.Constents;
using Help.Exceptions;
using Help.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public abstract class AbstractController<C,Request,Response> : ControllerBase where C: ControllerBase where Request:IRequest where Response:IResponse
    {
        protected readonly ILogger<C> _logger;
        protected readonly IConfiguration _configuration;
        protected ILogic<Request, Response> _logic;
        ITranslator _translator;
        public AbstractController(IConfiguration configuration,ILogger<C> logger, ITranslator translator, ILogic<Request, Response> logic)
        {
            _configuration = configuration;
            _logger = logger;
            _translator = translator;
            _logic = logic;
        }
        protected IActionResult RequestHandler(string language,Func<IActionResult> func)
        {
            try
            {
                return func();

            } 
            catch (MessagesException ex)
            {
                ErrorMessagesResponse errorResponse = new ErrorMessagesResponse { ErrorCode = (int)HttpStatusCode.BadRequest };
                errorResponse.Messages.AddRange(ex.Messages);
                return BadRequest(errorResponse);
            }
            catch(TranslationException ex)
            {
                ErrorMessagesResponse errorResponse = new ErrorMessagesResponse { ErrorCode = (int)HttpStatusCode.BadRequest };
                errorResponse.Messages.Add(ex.Message);
                _logger.LogError(ex.InnerException?.Message);
                return BadRequest(errorResponse);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorMessagesResponse errorResponse = new ErrorMessagesResponse { ErrorCode = (int)HttpStatusCode.BadRequest };
                errorResponse.Messages.Add(_translator[language, ConstentMessages.ServerHandleError]);
                return BadRequest(errorResponse);
            }
            
        }

        [HttpGet("all/{language}")]
        public virtual IActionResult GetAll(string language)
        {
            return this.RequestHandler(language, () =>
            {
                IEnumerable<Response> response = _logic.LoadAll(language);
                return Ok(response);
            });
        }
        [HttpGet("list/{parentid}/{language}")]
        public virtual IActionResult GetListWithParentId(string language,string parentId)
        {
            return this.RequestHandler(language, () =>
            {
                IEnumerable<Response> response = _logic.Load(language,parentId);
                return Ok(response);
            });
        }
        [HttpGet("{Id}/{language}")]
        public virtual IActionResult Get(string language,string id)
        {
            return this.RequestHandler(language, () =>
            {
                Response response = _logic.LoadWithId(language,id);
                return Ok(response);
            });
        }
        [HttpPost("{language}")]
        public virtual IActionResult Post(string language,Request request)
        {
            return this.RequestHandler(language, () =>
            {
                IdResponse response = _logic.Insert(language, request); 
                return Ok(response);
            });
        }

        [HttpPut("{language}")]
        public virtual IActionResult Put(string language, Request request)
        {
            return this.RequestHandler(language, () =>
            {
                MessageResponse response = _logic.Update(language, request);
                return Ok(response);
            });
        }

        [HttpDelete("{language}")]
        public virtual IActionResult Delete(string language, string id)
        {
            return this.RequestHandler(language, () =>
            {
                MessageResponse response = _logic.DeleteWithId(language, id);
                return Ok(response);
            });
        }
    }
}
