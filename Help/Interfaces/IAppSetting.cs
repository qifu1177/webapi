using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Interfaces
{
    public interface IAppSetting
    {
        int SessionDuration { get; set; }
        int UploadFileMaxSize { get; set; }
        List<string> UploadFileTypes { get; set; }
        string EmailHost { get; set; }
        int EmailPort { get; set; }
        string SenderAccount { get; set; }
        string SenderAccountPassword { get; set; }
    }
}
