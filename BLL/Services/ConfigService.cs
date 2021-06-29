using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BLL.Services
{
    public class ConfigService:Singleton<ConfigService>
    {
        public dynamic GetUserSettings(IConfiguration configuration)
        {

            dynamic userSetting = new ExpandoObject();            
            userSetting.session = new ExpandoObject();
            userSetting.session.duration = Convert.ToInt32(configuration["UserSetting:Session:Duration"]);
            userSetting.uploadFile = new ExpandoObject();
            userSetting.uploadFile.maxSize= Convert.ToInt32(configuration["UserSetting:UploadFile:MaxSize"]);
            List<string> typeList = new List<string>();
            for (int i = 0; ; i++)
            {
                string vs = configuration[$"UserSetting:UploadFile:Type:{i}"];
                if (string.IsNullOrEmpty(vs))
                    break;
                typeList.Add(vs);

            }
            userSetting.uploadFile.type = typeList;
            return userSetting;
        }
    }
}
