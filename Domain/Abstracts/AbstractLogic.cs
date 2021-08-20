using Data.Interfaces;
using Data.Models;
using Domain.Interfaces;
using Domain.Models.Responses;
using Help.Constents;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstracts
{
    public abstract class AbstractLogic
    {
        protected ITranslator _translator;
        protected IUserWorkOfUnit _baseUserWorkOfUnit;
        protected IAppSetting _appSetting;
        public AbstractLogic(ITranslator translator, IUserWorkOfUnit userWorkOfUnit, IAppSetting appSetting)
        {
            _translator = translator;
            _baseUserWorkOfUnit = userWorkOfUnit;
            _appSetting = appSetting;
        }

        protected ValidationResponse Validat<T>(IValidatorWithTranslator<T> validator, T request, string language) where T : IRequest
        {
            var result = validator.Init(language).Validate(request);
            if (result.Errors.Count > 0)
            {
                ValidationResponse response = new ValidationResponse();
                foreach (var failure in result.Errors)
                {
                    response.Messages.Add(failure.ErrorMessage);
                }
                return response;
            }
            return null;
        }

        protected AppUser LoadUserWithSessionId(string sessionId)
        {
            return _baseUserWorkOfUnit.LoadUserWithSessionId(sessionId);
        }

        protected ValidationResponse ValidateSession(AppUser user, string language)
        {
            if (user == null || (DateTime.UtcNow - user.Session.UpdateTs).TotalSeconds > _appSetting.SessionDuration)
            {
                ValidationResponse response = new ValidationResponse { SessionInvalid = true };
                response.Messages.Add(_translator[language, ConstentMessages.SessionIdIsInvalid]);
                return response;
            }
            return null;
        }
    }
}
