using Data.Interfaces;
using Data.Models;
using Domain.Abstracts;
using Domain.Constents;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Help.Exceptions;
using Help.Extensions;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Logics
{
    public class UserLogic : AbstractLogic,IUserLogic
    {
        private IUserWorkOfUnit _work;
        private IValidatorWithTranslator<UserRegisterRequest> _validator;

        public UserLogic(IUserWorkOfUnit work,ITranslator translator, IValidatorWithTranslator<UserRegisterRequest> validator):base(translator)
        {
            _work = work;
            _validator = validator;
        }
        public UserLoginResponse Login(string language, UserLoginRequest request)
        {
            string errorMessageKey = "";
            try
            {
                AppUser user = _work.LoadUserWithUn(request.LoginName, request.Password);
                if(user==null)
                    user = _work.LoadUserWithEmail(request.LoginName, request.Password);
                if (user == null)
                {
                    errorMessageKey = ConstentMessages.UserNotExist;
                    throw new ArgumentException();
                }                   
                errorMessageKey = ConstentMessages.LoadRoleError;
                Dictionary<string, string> moduleRights = _work.LoadModuleRights(user.RoleId);
                DateTime utcNow = DateTime.UtcNow;
                user.Session = new AppSession { CreateTs = utcNow, UpdateTs = utcNow };
                errorMessageKey = ConstentMessages.CreateSessionForUserError;
                _work.UpdateAppUser(user);
                return new UserLoginResponse { UserName = user.Name, SessionId = user.Session.Id, UpdateTs = user.Session.UpdateTs.ToJsTime(), ModuleRights= moduleRights };
            }
            catch(Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public MessageResponse Register(string language, UserRegisterRequest request)
        {
            ValidationResponse validationResponse = Validat<UserRegisterRequest>(_validator, request, language);
            if (validationResponse == null)
            {
                string errorMessageKey = "";
                try
                {
                    errorMessageKey = ConstentMessages.LoadRoleError;
                    string roleId = _work.LoadRoleId(EnumData.Role.Manager.ToString());
                    errorMessageKey = ConstentMessages.SaveUserError;
                    _work.InserUser(request.UserName, request.Password, request.Email, roleId);
                }catch(Exception ex)
                {
                    throw new TranslationException(_translator, language, errorMessageKey, ex); ;
                }
            }
            else
                throw new MessagesException(validationResponse.Messages);
            return null;
        }
    }
}
