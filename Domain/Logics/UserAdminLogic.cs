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
using Help.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Logics
{
    public class UserAdminLogic : AbstractLogic, ILogic<UserRequest, UserListResponse>
    {
        private IUserWorkOfUnit _work;
        private IValidatorWithTranslator<UserRequest> _userValidator;
        private IPasswordService _passwordService;
        private IEmailService _emailService;

        public UserAdminLogic(IUserWorkOfUnit work, ITranslator translator, IValidatorWithTranslator<UserRequest> userValidator, IAppSetting appSetting, IPasswordService passwordService, IEmailService emailService) : base(translator, work, appSetting)
        {
            _work = work;
            _userValidator = userValidator;
            _passwordService = passwordService;
            _emailService = emailService;
        }
        public MessageSessionResponse DeleteWithId(string language, string sessionId, object id)
        {
            AppUser admin = this.LoadUserWithSessionId(sessionId);
            ValidateSessionAndRight(language, admin, EnumData.AppModules.AppUser.ToString(), new List<string> { EnumData.RoleRight.all.ToString(), EnumData.RoleRight.account.ToString() });
            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.UserNotExist;
                _work.DeleteUserWithId(id.ToString());
                this.UpdateAdminSession(admin);
                return new MessageSessionResponse { Message = "OK", SessionId = admin.Session.Id, SessionUpdateTs = admin.Session.UpdateTs.ToJsTime() };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public IdResponse Insert(string language, string sessionId, UserRequest request)
        {
            AppUser admin = this.LoadUserWithSessionId(sessionId);
            ValidateSessionAndRight(language, admin, EnumData.AppModules.AppUser.ToString(), new List<string> { EnumData.RoleRight.all.ToString(), EnumData.RoleRight.account.ToString() });
            ValidationResponse validationResponse = Validat<UserRequest>(_userValidator, request, language);
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
                string password = _passwordService.Generate();
                string subject = _translator[language, ConstentMessages.EmailCreateSuject];
                string body = _translator[language, ConstentMessages.EmailCreateBody, password];
                _emailService.Send(request.Email, subject, body);
                AppUser user = CreateUserFromUserRequest(request, password);
                user.AccountUserId = user.Id;
                _work.InserUser(user);
                this.UpdateAdminSession(admin);
                return new IdResponse { Id = user.Id, SessionId = admin.Session.Id, SessionUpdateTs = admin.Session.UpdateTs.ToJsTime() };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public UserListResponse Load(string language, string sessionId, object parentId)
        {
            AppUser admin = this.LoadUserWithSessionId(sessionId);
            ValidateSessionAndRight(language, admin, EnumData.AppModules.AppUser.ToString(), new List<string> { EnumData.RoleRight.account.ToString() });
            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.ServerError;
                IEnumerable<AppUser> userList = _work.LoadUsersWithParentId(parentId.ToString());
                List<UserResponse> list = new List<UserResponse>();
                foreach (AppUser user in userList)
                    list.Add(CreateUserResponseFromUser(user));
                this.UpdateAdminSession(admin);
                return new UserListResponse
                {
                    SessionId = admin.Session.Id,
                    SessionUpdateTs = admin.Session.UpdateTs.ToJsTime(),
                    UserList = list
                };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public UserListResponse LoadAll(string language, string sessionId)
        {
            AppUser admin = this.LoadUserWithSessionId(sessionId);
            ValidateSessionAndRight(language, admin, EnumData.AppModules.AppUser.ToString(), new List<string> { EnumData.RoleRight.all.ToString() });
            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.ServerError;
                IEnumerable<AppUser> userList = _work.LoadAllUser();
                List<UserResponse> list = new List<UserResponse>();
                foreach (AppUser user in userList)
                    list.Add(CreateUserResponseFromUser(user));
                this.UpdateAdminSession(admin);
                return new UserListResponse
                {
                    SessionId = admin.Session.Id,
                    SessionUpdateTs = admin.Session.UpdateTs.ToJsTime(),
                    UserList = list
                };
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public UserListResponse LoadWithId(string language, string sessionId, object id)
        {
            AppUser admin = this.LoadUserWithSessionId(sessionId);
            ValidateSessionAndRight(language, admin, EnumData.AppModules.AppUser.ToString(), new List<string> { EnumData.RoleRight.all.ToString(), EnumData.RoleRight.account.ToString() });
            string errorMessageKey = "";
            try
            {
                errorMessageKey = ConstentMessages.ServerError;
                AppUser user = _work.LoadUserWithId(id.ToString());
                if (user == null)
                    throw new TranslationException(_translator, language, ConstentMessages.UserNotExist, null);
                this.UpdateAdminSession(admin);
                UserResponse userResponse = CreateUserResponseFromUser(user);
                return new UserListResponse
                {
                    SessionUpdateTs = admin.Session.UpdateTs.ToJsTime(),
                    SessionId = admin.Session.Id,
                    UserList = new List<UserResponse> { userResponse }
                };
            }
            catch (TranslationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }

        public MessageSessionResponse Update(string language, string sessionId, UserRequest request)
        {
            AppUser admin = this.LoadUserWithSessionId(sessionId);
            ValidateSessionAndRight(language, admin, EnumData.AppModules.AppUser.ToString(),
                new List<string> { EnumData.RoleRight.all.ToString(), EnumData.RoleRight.account.ToString() });
            ValidationResponse validationResponse = Validat<UserRequest>(_userValidator, request, language);
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
                AppUser appUser = _work.LoadUserWithId(request.Id);
                if (appUser == null)
                    throw new TranslationException(_translator, language, ConstentMessages.UserNotExist, null);
                UpdateUserFromUserRequest(appUser, request);
                if (request.UpdateTs != appUser.UpdateTs.ToJsTime())
                    throw new TranslationException(_translator, language, ConstentMessages.UserIsChanged, null);
                _work.UpdateAppUser(appUser);
                this.UpdateAdminSession(admin);
                return new MessageSessionResponse { Message = "OK", SessionId = admin.Session.Id, SessionUpdateTs = admin.Session.UpdateTs.ToJsTime() };
            }
            catch (TranslationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TranslationException(_translator, language, errorMessageKey, ex);
            }
        }        

        private UserResponse CreateUserResponseFromUser(AppUser user)
        {
            UserResponse userResponse= new UserResponse
            {
                UserId = user.Id,
                UserName = user.Name,
                UpdateTs = user.UpdateTs.ToJsTime(),
                Email = user.Email,
                RoleId=user.RoleId
            };
            if(user.UserInfo!=null)
            {
                userResponse.BirthDate = user.UserInfo.BirthDate?.ToDateString();
                userResponse.PhoneNumber = user.UserInfo.PhoneNumber;
                userResponse.PhotoPath = user.UserInfo.PhotoPath;
                userResponse.MaritalStatus = user.UserInfo.MaritalStatus;
            }
            return userResponse;
        }
        private AppUser CreateUserFromUserRequest(UserRequest request,string password)
        {
            DateTime utcNow = DateTime.UtcNow;
            return new AppUser
            {
                CreateTs = utcNow,
                UpdateTs = utcNow,
                Name = request.UserName,
                Email = request.Email,
                Password = MD5HashService.Instance.CreateMD5Hash(password),
                RoleId = request.RoleId,
                UserInfo = new UserInfo
                {
                    BirthDate = DateTimeExtensions.ToDate(request.BirthDate),
                    PhoneNumber = request.PhoneNumber,
                    PhotoPath = request.PhotoPath,
                    MaritalStatus = request.MaritalStatus
                }
            };
        }
        private void UpdateUserFromUserRequest(AppUser user, UserRequest request)
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
