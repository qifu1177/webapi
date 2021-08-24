using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Constents
{
    public class ConstentConfigKey
    {
        public const string MongoDB_ConnectionStr = "MongoDB:ConnectionStr";
        public const string MongoDB_DBName = "MongoDB:DBName";
        public const string Session_Duration = "UserSetting:Session:Duration";
        public const string UploadFile_MaxSize = "UserSetting:UploadFile:MaxSize";
        public const string UploadFile_Type = "UserSetting:UploadFile:Type";
        public const string Config_LoggingFile = "LoggingFile";

        public const string EmailServer_Host = "UserSetting:EmailServer:Host";
        public const string EmailServer_Port = "UserSetting:EmailServer:Port";
        public const string EmailServer_From = "UserSetting:EmailServer:From";
        public const string EmailServer_FromPassword = "UserSetting:EmailServer:FromPassword";

    }
}
