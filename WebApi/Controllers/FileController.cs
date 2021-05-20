using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : AbstractController
    {
        public FileController(IConfiguration configuration, ILogger<DataController> logger) : base(configuration, logger)
        {
        }
        [HttpGet("files")]
        public IEnumerable<BLL.Models.FileResult> Files()
        {
            List<BLL.Models.FileResult> list = new List<BLL.Models.FileResult>();
            string currentDir = Directory.GetCurrentDirectory();
            var fileDir = Path.Combine(currentDir, "files");

            DirectoryInfo directoryInfo = new DirectoryInfo(fileDir);
            if (directoryInfo.Exists )
            {
                foreach(FileInfo fInfo in directoryInfo.EnumerateFiles())
                {
                    string extension = fInfo.Extension.ToLower();
                    string url=$"{Request.Scheme}://{Request.Host}";
                    BLL.Models.FileResult fileResult= new BLL.Models.FileResult
                    {
                        Name = fInfo.Name,
                        FileType = extension,
                        Size = fInfo.Length,
                        Link = $"{url}/file/{fInfo.Name}",                        
                        IsImage = extension.EndsWith("jpg") || extension.EndsWith("jpeg") || extension.EndsWith("png") || extension.EndsWith("gif") || extension.EndsWith("bmp")
                    };
                    SetIcon(fileResult,url);
                    list.Add(fileResult);
                }
            }
            return list;
        }
        private void SetIcon(BLL.Models.FileResult fileResult,string url)
        {
            if (fileResult.IsImage)
                fileResult.Icon = fileResult.Link;
            else
            {
                if (fileResult.FileType.ToLower().EndsWith("pdf"))
                    fileResult.Icon = $"{url}/file/icon/pdf.png";
                else if (fileResult.FileType.ToLower().EndsWith("doc") || fileResult.FileType.ToLower().EndsWith("docx"))
                    fileResult.Icon = $"{url}/file/icon/wort.png";
                else 
                    fileResult.Icon = $"{url}/file/icon/dokument.png";
            }
        }
        [HttpDelete("{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                var fileDir = Path.Combine(currentDir, "files");
                string filePath = Path.Combine(fileDir, fileName);
                System.IO.File.Delete(filePath);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"File {fileName} is not found. Error: {ex}");
            }
            return Ok("OK");
        }
    }
}
