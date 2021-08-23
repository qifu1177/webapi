using Data.Interfaces;
using Data.Models;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Responses;
using Help.Constents;
using Help.Exceptions;
using Help.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

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
        protected void UpdateAdminSession(AppUser admin)
        {
            DateTime utcNow = DateTime.UtcNow;
            admin.Session = new AppSession { Id = ObjectId.GenerateNewId().ToString(), CreateTs = utcNow, UpdateTs = utcNow };
            _baseUserWorkOfUnit.UpdateAppUser(admin);            
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
        protected ValidationResponse ValidateRight(AppUser user, string language, string appModuleName,List<string> rights)
        {
            if (user == null)
            {
                ValidationResponse response = new ValidationResponse();
                response.Messages.Add(_translator[language, ConstentMessages.UserNotExist]);
                return response;
            }
            else
            {
                Dictionary<string, string> moduleRights = _baseUserWorkOfUnit.LoadModuleRights(user.RoleId);
                if (moduleRights.ContainsKey(appModuleName) && rights.Contains(moduleRights[appModuleName]))
                    return null;
                ValidationResponse response = new ValidationResponse();
                response.Messages.Add(_translator[language, ConstentMessages.UserHasNotRight]);
                return response;
            }            
        }
        protected void ValidateSessionAndRight(string language, AppUser appUser, string appModuleName, List<string> rights)
        {            
            ValidationResponse validationResponse = this.ValidateSession(appUser, language);
            if (validationResponse != null)
                throw new MessagesException(validationResponse.Messages);
            validationResponse = this.ValidateRight(appUser, language, appModuleName,rights);
            if (validationResponse != null)
                throw new MessagesException(validationResponse.Messages);
          
        }
    }
}
