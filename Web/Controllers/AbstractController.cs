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
    public abstract class AbstractController<C> : ControllerBase where C : ControllerBase
    {
        protected readonly ILogger<C> _logger;
        protected readonly IConfiguration _configuration;
        protected ITranslator _translator;
        public AbstractController(IConfiguration configuration, ILogger<C> logger, ITranslator translator)
        {
            _configuration = configuration;
            _logger = logger;
            _translator = translator;            
        }
        protected virtual IActionResult RequestHandler(string language, Func<IActionResult> func)
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
    }
}
