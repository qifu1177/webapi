using Domain.Models.Requests;
using Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserLogic<Request,Response>:ILogic<Request,Response> where Request:IRequest where Response:IResponse
    {
        UserLoginResponse Login(string language,UserLoginRequest request);
        MessageResponse Register(string language, UserRegisterRequest request);
    }
}
