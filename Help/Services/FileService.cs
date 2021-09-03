using Help.Constents;
using Help.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Services
{
    public class FileService : IFileService
    { 
        private IAppSetting _appSetting;
        private ITranslator _translator;
        public FileService(IAppSetting appSetting, ITranslator translator)
        {
            _appSetting = appSetting;
            _translator = translator;
        }
        public void GetFielInfo(string sessionId, string path, string url, string fileName, out string link, out string icon, out bool isImage, out string fileType)
        {
            string extension = Path.GetExtension(Path.Combine(path, fileName)).ToLower();
            fileType = extension.TrimStart('.');
            link = $"{url}/file/{sessionId}/{fileName}";
            isImage = extension.EndsWith("jpg") || extension.EndsWith("jpeg") || extension.EndsWith("png") || extension.EndsWith("gif") || extension.EndsWith("bmp");
            icon = link;
            if (!isImage)
            {
                if (extension.EndsWith("pdf"))
                    icon = $"{url}/file/icon/pdf.png";
                else if (extension.EndsWith("doc") || extension.EndsWith("docx"))
                    icon = $"{url}/file/icon/wort.png";
                else
                    icon = $"{url}/file/icon/dokument.png";
            }
        }

        private string GetFileType(string fileName)
        {
            string fileType = "";
            int startIndex = fileName.LastIndexOf('.') + 1;
            if(startIndex>0)
                fileType= fileName.Substring(startIndex).ToLower();
            return fileType;
        }
        private bool CheckFileType(string fileName)
        {
            string fileType = GetFileType(fileName);
            return _appSetting.UploadFileTypes.Contains(fileType);
        }
        private string GetUserDir(string userId)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string userDir=Path.Combine(currentDir, "files", userId);
            return userDir;
        }
        private string GetIconsDir()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string dir = Path.Combine(currentDir, "icons");
            return dir;
        }
        private long GetUsedSize(string userId)
        {
            long size = 0;
            string userDir = GetUserDir(userId);
            if (!Directory.Exists(userDir))
            {
                return 0;
            }
            DirectoryInfo info = new DirectoryInfo(userDir);
            FileInfo[] fileinfos = info.GetFiles();
            foreach(var item in fileinfos)
            {
                size += item.Length;
            }
            return size;
        }
        public List<string> GitFileList(string userId)
        {
            List<string> list = new List<string>();
            string userDir = GetUserDir(userId);
            if (Directory.Exists(userDir))
            {
                DirectoryInfo info = new DirectoryInfo(userDir);
                FileInfo[] fileinfos = info.GetFiles();
                foreach (var item in fileinfos)
                {
                    list.Add(item.Name);
                }
            }
            return list;
        }
        public long GetRestFreeSize(string userId)
        {
            long usedSize = GetUsedSize(userId);
            return _appSetting.UploadFileMaxSize * 1024 * 1024 - usedSize;
        }
        public string ValidFile(string userId,string fileName,long fileSize,string language)
        {
            string message = string.Empty;
            if (!CheckFileType(fileName))
                return _translator[language, ConstentMessages.FileTypeError, fileName];
            if (fileSize > GetRestFreeSize(userId))
                return _translator[language, ConstentMessages.FileSizeToBigError];
            return message;
        }
        public string GetFilePath(string userId,string fileName)
        {
            string userDir = GetUserDir(userId);
            if (!Directory.Exists(userDir))
            {
                Directory.CreateDirectory(userDir);
            }
            return Path.Combine(userDir, fileName);
        }
        public bool IsImage(string fileName)
        {
            string fileType = GetFileType(fileName);
            return _appSetting.ImageTypes.Contains(fileType);
        }
        public string GetIcon(string fileName)
        {
            string fileType = GetFileType(fileName);
            string dir = GetIconsDir();
            if (fileType == "pdf")
                return Path.Combine(dir, "pdf.png");
            else if (fileType == "doc" || fileType == "docx")
                return Path.Combine(dir, "wort.png");
            return Path.Combine(dir, "dokument.png");
        }
        public string GetContentType(string fileName)
        {
            string contentType = "application/octet-stream";
            string fileType = GetFileType(fileName);
            if (_appSetting.ImageTypes.Contains(fileType))
            {
                contentType = "image/jpeg";
            }
            else if (fileType=="pdf")
            {
                contentType = "application/pdf";
            }
            else if (fileType=="json" || fileType=="txt")
            {
                contentType = "text/html; charset=UTF-8";
            }
            return contentType;
        }
    }
}
