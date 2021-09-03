using Data.Interfaces;
using Data.Models;
using Domain.Abstracts;
using Domain.Interfaces;
using Domain.Models.Responses;
using Help.Constents;
using Help.Exceptions;
using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Logics
{
    public class FileLogic : AbstractLogic, IFileLogic
    {
        public FileLogic(ITranslator translator, IUserWorkOfUnit userWorkOfUnit, IAppSetting appSetting):base(translator,userWorkOfUnit,appSetting)
        {
        }
        public string GetUserId(string language, string sessionId)
        {
            AppUser user = LoadUserWithSessionId(sessionId);
            if(user==null)
                throw new TranslationException(_translator, language, ConstentMessages.UserNotExist, null);
            return user.Id;
        }
    }
}
