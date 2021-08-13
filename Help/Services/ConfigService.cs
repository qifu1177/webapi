using Help.Constents;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Services
{
    public class ConfigService : Singleton<ConfigService>
    {
        public dynamic GetUserSettings(IConfiguration configuration)
        {

            dynamic userSetting = new ExpandoObject();
            userSetting.session = new ExpandoObject();
            userSetting.session.duration = Convert.ToInt32(configuration[ConstentConfigKey.Session_Duration]);
            userSetting.uploadFile = new ExpandoObject();
            userSetting.uploadFile.maxSize = Convert.ToInt32(configuration[ConstentConfigKey.UploadFile_MaxSize]);
            List<string> typeList = new List<string>();
            for (int i = 0; ; i++)
            {
                string vs = configuration[$"{ConstentConfigKey.UploadFile_Type}:{i}"];
                if (string.IsNullOrEmpty(vs))
                    break;
                typeList.Add(vs);

            }
            userSetting.uploadFile.type = typeList;
            return userSetting;
        }
    }
}
