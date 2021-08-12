using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constents
{
    public class ConstentMessages
    {
        // Error message key
        public const string UserNotExist = "UserNotExist";
        public const string LoadRoleError = "LoadRoleError";
        public const string CreateSessionForUserError = "CreateSessionForUserError";
        public const string SaveUserError = "SaveUserError";
        //Regex strings
        public const string EmailFormExpression = @".+@{1}\w+\.{1}\w{2,}";
        public const string AdressFormExpression = @".+\d+\s*\,*\s*\d{5}.*";
    }
}
