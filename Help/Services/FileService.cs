using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Services
{
    public class FileService : Singleton<FileService>
    {
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

        public bool ValidFile(string fileName, long fileSize, IConfiguration configuration, out string message)
        {
            message = string.Empty;
            List<string> typeList = new List<string>();
            long maxSize;
            dynamic setting = ConfigService.Instance.GetUserSettings(configuration);
            //GetUploadFileConfig(configuration, typeList, out maxSize);
            int startIndex = fileName.LastIndexOf('.') + 1;
            if (startIndex < 0 || startIndex > fileName.Length)
            {
                message = "The type of the uploaded file is invalid.";
                return false;
            }
            string extension = fileName.Substring(startIndex).ToLower();
            if (!setting.uploadFile.type.Contains(extension.ToLower()))
            {
                message = "The type of the uploaded file is invalid.";
                return false;
            }
            if (fileSize > setting.uploadFile.maxSize * 1024 * 1024)
            {
                message = "The uploaded file is too large.";
                return false;
            }
            return true;
        }

        //public void GetUploadFileConfig(IConfiguration configuration, List<string> typeList, out long maxSize)
        //{
        //    maxSize = Convert.ToInt64(configuration["UploadFile:MaxSize"]);
        //    for (int i = 0; ; i++)
        //    {
        //        string vs = configuration[$"UploadFile:Type:{i}"];
        //        if (string.IsNullOrEmpty(vs))
        //            break;
        //        typeList.Add(vs);

        //    }
        //}

    }
}
