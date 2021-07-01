using BLL;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : AbstractController
    {
        public FileController(IConfiguration configuration, ILogger<DataController> logger) : base(configuration, logger)
        {
        }
        //[HttpGet("setting")]
        //public IActionResult GetUploadFileSetting()
        //{            
        //    dynamic userSetting = ConfigService.Instance.GetUserSettings(_configuration);
        //    List<string> typeList = userSetting.UploadFile.Type;
        //    long maxSize= userSetting.UploadFile.MaxSize;
        //    FileService.Instance.GetUploadFileConfig(_configuration, typeList, out maxSize);
        //    string typestr = string.Join('|', typeList);
        //    string json = string.Format("\"filetypes\":\"{0}\",\"maxsize\": {1}", typestr, maxSize);
        //    return Ok('{'+json+'}');
        //}
        [HttpGet("files/{sessionid}")]
        public IEnumerable<BLL.Models.FileResult> Files(string sessionid)
        {
            List<BLL.Models.FileResult> list = new List<BLL.Models.FileResult>();
            var fileDir = GetFileDirWithSessionId(sessionid);
            if (string.IsNullOrEmpty(fileDir))
                return list;
            DirectoryInfo directoryInfo = new DirectoryInfo(fileDir);
            if (directoryInfo.Exists)
            {
                string url = GetBaseUrl();
                foreach (FileInfo fInfo in directoryInfo.EnumerateFiles())
                {
                    string link, icon, fileType;
                    bool isImage;
                    FileService.Instance.GetFielInfo(sessionid,fileDir, url, fInfo.Name, out link, out icon, out isImage, out fileType);                    
                    BLL.Models.FileResult fileResult = new BLL.Models.FileResult
                    {
                        Name = fInfo.Name,
                        Size = fInfo.Length,
                        FileType = fileType,
                        Icon = icon,
                        Link = link,
                        IsImage = isImage
                    };
                    list.Add(fileResult);
                }
            }
            return list;
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {               
                var file = Request.Form.Files[0];
                string message;
                bool b = FileService.Instance.ValidFile(file.FileName, file.Length, _configuration, out message);
                if (!b)
                {
                    return StatusCode(500, message);
                }
               
                var fileDir = GetFileDirWithSessionId(Request.Form["sessionid"]);
                if (string.IsNullOrEmpty(fileDir))
                    return BadRequest();

                if (file.Length > 0)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(fileDir);
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(fileDir, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok("OK");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{sessionid}/{fileName}")]
        public IActionResult DeleteFile(string sessionid,string fileName)
        {
            try
            {                 
                var fileDir = GetFileDirWithSessionId(sessionid);
                if (string.IsNullOrEmpty(fileDir))
                    return BadRequest();
                string filePath = Path.Combine(fileDir, fileName);
                System.IO.File.Delete(filePath);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"File {fileName} is not found. Error: {ex}");
            }
            return Ok("OK");
        }
    }
}
