using Help.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Datas
{
    public class AppSetting : Singleton<AppSetting>, IAppSetting
    {
        public int SessionDuration { get; set; } = 600;
        public int UploadFileMaxSize { get; set; } = 1;
        public List<string> UploadFileTypes { get; set; } = new List<string>();
    }
}
