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
        public string EmailHost { get; set; }
        public int EmailPort { get ; set ; }
        public string SenderAccount { get; set; }
        public string SenderAccountPassword { get; set; }
    }
}
