using Domain.Interfaces;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{

    public class UserLoginResponse : SessionResponse
    {
        public string UserName { get; set; }

        public Dictionary<string, string> ModuleRights { get; set; }
        public IAppSetting AppSetting { get; set; }
    }
}
