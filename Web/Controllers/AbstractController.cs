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
    public abstract class AbstractController<C, Request, Response> : ControllerBase where C : ControllerBase where Request : IRequest where Response : IResponse
    {
        protected readonly ILogger<C> _logger;
        protected readonly IConfiguration _configuration;
        protected ILogic<Request, Response> _logic;
        ITranslator _translator;
        public AbstractController(IConfiguration configuration, ILogger<C> logger, ITranslator translator, ILogic<Request, Response> logic)
        {
            _configuration = configuration;
            _logger = logger;
            _translator = translator;
            _logic = logic;
        }
        protected IActionResult RequestHandler(string language, Func<IActionResult> func, bool userBaseLogic = false)
        {
            if (userBaseLogic && _logic == null)
            {
                ErrorMessagesResponse errorResponse = new ErrorMessagesResponse { ErrorCode = (int)HttpStatusCode.BadRequest };
                errorResponse.Messages.Add(_translator[language, ConstentMessages.ServerHandleError]);
                return new BadRequestResult();
            }
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
            catch (TranslationException ex)
            {
                ErrorMessagesResponse errorResponse = new ErrorMessagesResponse { ErrorCode = (int)HttpStatusCode.BadRequest };
                errorResponse.Messages.Add(ex.Message);
                _logger.LogError(ex.InnerException?.Message);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorMessagesResponse errorResponse = new ErrorMessagesResponse { ErrorCode = (int)HttpStatusCode.BadRequest };
                errorResponse.Messages.Add(_translator[language, ConstentMessages.ServerHandleError]);
                return BadRequest(errorResponse);
            }

        }

        [HttpGet("all/{sessionid}/{language}")]
        public virtual IActionResult GetAll(string language, string sessionid)
        {
            return this.RequestHandler(language, () =>
            {
                Response response = _logic.LoadAll(sessionid, language);
                return Ok(response);
            },true);
        }

        [HttpGet("list/{parentid}/{sessionid}/{language}")]
        public virtual IActionResult GetListWithParentId(string language, string parentId, string sessionid)
        {
            return this.RequestHandler(language, () =>
            {
                Response response = _logic.Load(language, sessionid, parentId);
                return Ok(response);
            },true);
        }
        [HttpGet("{id}/{sessionid}/{language}")]
        public virtual IActionResult Get(string language, string sessionid, string id)
        {
            return this.RequestHandler(language, () =>
            {
                Response response = _logic.LoadWithId(language, sessionid, id);
                return Ok(response);
            },true);
        }
        [HttpPost("{sessionid}/{language}")]
        public virtual IActionResult Post(string language, string sessionid, Request request)
        {
            return this.RequestHandler(language, () =>
            {
                IdResponse response = _logic.Insert(language, sessionid, request);
                return Ok(response);
            },true);
        }

        [HttpPut("{sessionid}/{language}")]
        public virtual IActionResult Put(string language, string sessionid, Request request)
        {
            return this.RequestHandler(language, () =>
            {
                MessageSessionResponse response = _logic.Update(language, sessionid, request);
                return Ok(response);
            },true);
        }

        [HttpDelete("{id}/{sessionid}/{language}")]
        public virtual IActionResult Delete(string language, string id, string sessionid)
        {
            return this.RequestHandler(language, () =>
            {
                MessageSessionResponse response = _logic.DeleteWithId(language, sessionid, id);
                return Ok(response);
            },true);
        }
    }
}
