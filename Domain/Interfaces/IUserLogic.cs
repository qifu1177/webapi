using Domain.Models.Requests;
using Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserLogic
    {
        UserLoginResponse Login(string language,UserLoginRequest request);
        MessageResponse Register(string language, UserRegisterRequest request);
        MessageSessionResponse ChangePassword(string language, string sessionId, PasswordRequest request);
        MessageSessionResponse UpdateUserInfo(string language, string sessionId, UserRequest request);
        UserSessionResponse LoadUserInfo(string language, string sessionId);
    }
}
