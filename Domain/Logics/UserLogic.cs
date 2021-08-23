using Data.Interfaces;
using Data.Models;
using Domain.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Help.Constents;
using Help.Exceptions;
using Help.Extensions;
using Help.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Logics
{
    public class UserLogic : AbstractLogic, IUserLogic
    {
        private IUserWorkOfUnit _work;
        private IValidatorWithTranslator<UserRegisterRequest> _validator;
        private IValidatorWithTranslator<UserRequest> _userValidator;

        public UserLogic(IUserWorkOfUnit work, ITranslator translator, IValidatorWithTranslator<UserRegisterRequest> validator, IValidatorWithTranslator<UserRequest> userValidator,IAppSetting appSetting) : base(translator,work, appSetting)
        {
            _work = work;
            _validator = validator;
            _userValidator = userValidator;
        }       

        public UserLoginResponse Login(string language, UserLoginRequest request)
        {
            string errorMessageKey = "";
            try
            {
                AppUser user = _work.LoadUserWithUn(request.LoginName, request.Password);
                if (user == null)
                    user = _work.LoadUserWithEmail(request.LoginName, request.Password);
                if (user == null)
                {
                    errorMessageKey = ConstentMessages.UserNotExist;
                    throw new ArgumentException();
                }
                errorMessageKey = ConstentMessages.LoadRoleError;
                Dictionary<string, string> moduleRights = _work.LoadModuleRights(user.RoleId);
                
                DateTime utcNow = DateTime.UtcNow;
                user.Session = new AppSession { Id = ObjectId.GenerateNewId().ToString(), CreateTs = utcNow, UpdateTs = utcNow };
                errorMessageKey = ConstentMessages.CreateSessionForUserError;
                _work.UpdateAppUser(user);
                return new UserLoginResponse { UserName = user.Name, SessionId = user.Session.Id, SessionUpdateTs = user.Session.UpdateTs.ToJsTime(), ModuleRights = moduleRights,AppSetting=_appSetting };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public MessageResponse Register(string language, UserRegisterRequest request)
        {
            ValidationResponse validationResponse = Validat<UserRegisterRequest>(_validator, request, language);
            if (validationResponse == null)
            {
                if (_work.EmailIsExist(request.Email))
                    throw new TranslationException(_translator, language, ConstentMessages.EmailIsExist, null);                
                if (_work.UserNameIsExist(request.UserName))
                    throw new TranslationException(_translator, language, ConstentMessages.UserNameIsExist, null);
                string errorMessageKey = "";
                try
                {
                    errorMessageKey = ConstentMessages.LoadRoleError;
                    string roleId = _work.LoadRoleId(EnumData.Role.Manager.ToString());
                    errorMessageKey = ConstentMessages.SaveUserError;
                    _work.InserUser(request.UserName, request.Password, request.Email, roleId);
                    return new MessageResponse { Message = "OK" };
                }
                catch (Exception ex)
                {
                    throw new TranslationException(_translator, language, errorMessageKey, ex); ;
                }
            }
            else
                throw new MessagesException(validationResponse.Messages);

        }

        public MessageSessionResponse UpdateUserInfo(string language, string sessionId, UserRequest request)
        {
            AppUser appUser = this.LoadUserWithSessionId(sessionId);
            ValidationResponse validationResponse = this.ValidateSession(appUser, language);
            if (validationResponse != null)
                throw new MessagesException(validationResponse.Messages);
            validationResponse = Validat<UserRequest>(_userValidator, request, language);
            if (validationResponse != null)
            {
                throw new MessagesException(validationResponse.Messages);
            }
            if (_work.EmailIsExist(request.Email))
                throw new TranslationException(_translator, language, ConstentMessages.EmailIsExist, null);
            if (_work.UserNameIsExist(request.UserName))
                throw new TranslationException(_translator, language, ConstentMessages.UserNameIsExist, null);

            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.ServerError;
                UpdateUserFromUserRequest(appUser, request);
                DateTime utcNow = DateTime.UtcNow;
                appUser.Session = new AppSession { Id = ObjectId.GenerateNewId().ToString(), CreateTs = utcNow, UpdateTs = utcNow };
                _work.UpdateAppUser(appUser);
                return new MessageSessionResponse { Message = "OK",SessionId=appUser.Session.Id,SessionUpdateTs=appUser.Session.UpdateTs.ToJsTime() };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }
        public MessageSessionResponse ChangePassword(string language, string sessionId,PasswordRequest request)
        {
            AppUser appUser = this.LoadUserWithSessionId(sessionId);
            ValidationResponse validationResponse = this.ValidateSession(appUser, language);
            if (validationResponse != null)
                throw new MessagesException(validationResponse.Messages);
            if(appUser.Password!=request.OldPassword)
                throw new TranslationException(_translator, language, ConstentMessages.PasswordNotRight, null);
            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.ServerError;                
                DateTime utcNow = DateTime.UtcNow;
                appUser.Password = request.NewPassword;
                appUser.Session = new AppSession { Id = ObjectId.GenerateNewId().ToString(), CreateTs = utcNow, UpdateTs = utcNow };
                _work.UpdateAppUser(appUser);
                return new MessageSessionResponse { Message = "OK", SessionId = appUser.Session.Id, SessionUpdateTs = appUser.Session.UpdateTs.ToJsTime() };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }
        public UserSessionResponse LoadUserInfo(string language, string sessionId)
        {
            AppUser appUser = this.LoadUserWithSessionId(sessionId);
            ValidationResponse validationResponse = this.ValidateSession(appUser, language);
            if (validationResponse != null)
                throw new MessagesException(validationResponse.Messages);
            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.ServerError;
                this.UpdateAdminSession(appUser);
                return CreateUserSessionResponse(appUser);
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }
        private UserSessionResponse CreateUserSessionResponse(AppUser user)
        {
            UserSessionResponse response= new UserSessionResponse {
                SessionId = user.Session.Id,
                SessionUpdateTs=user.Session.UpdateTs.ToJsTime(),
                UserName = user.Name,
                UpdateTs = user.UpdateTs.ToJsTime(),
                Email = user.Email,
                RoleId = user.RoleId
            };
            if (user.UserInfo != null)
            {
                response.BirthDate = user.UserInfo.BirthDate?.ToDateString();
                response.PhoneNumber = user.UserInfo.PhoneNumber;
                response.PhotoPath = user.UserInfo.PhotoPath;
                response.MaritalStatus = user.UserInfo.MaritalStatus;
            }
            return response;
        }
        private void UpdateUserFromUserRequest(AppUser user,UserRequest request)
        {
            user.Email = request.Email;
            user.Name = request.UserName;
            user.RoleId = request.RoleId;
            if (user.UserInfo == null)
                user.UserInfo = new UserInfo();
            user.UserInfo.BirthDate = DateTimeExtensions.ToDate(request.BirthDate);
            user.UserInfo.PhoneNumber = request.PhoneNumber;
            user.UserInfo.PhotoPath = request.PhotoPath;
            user.UserInfo.MaritalStatus = request.MaritalStatus;
        }

    }
}
