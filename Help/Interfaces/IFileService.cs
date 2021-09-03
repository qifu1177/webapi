using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Interfaces
{
    public interface IFileService
    {
        string ValidFile(string userId, string fileName, long fileSize, string language);
        string GetFilePath(string userId, string fileName);
        bool IsImage(string fileName);
        string GetIcon(string fileName);
        string GetContentType(string fileName);
        long GetRestFreeSize(string userId);
        List<string> GitFileList(string userId);
    }
}
