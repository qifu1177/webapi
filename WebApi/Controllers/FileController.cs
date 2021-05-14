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
                    list.Add(new BLL.Models.FileResult
                    {
                        Name = fInfo.Name,
                        FileType = extension,
                        Size=fInfo.Length,
                        IsImage=extension.EndsWith("jpg") || extension.EndsWith("jpeg") || extension.EndsWith("png") || extension.EndsWith("gif") || extension.EndsWith("bmp")
                    }) ;
                }
            }
            return list;
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
