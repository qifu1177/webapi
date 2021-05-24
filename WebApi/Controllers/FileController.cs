﻿using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        [HttpGet("setting")]
        public IActionResult GetUploadFileSetting()
        {
            List<string> typeList = new List<string>();
            long maxSize;
            FileService.Instance.GetUploadFileConfig(_configuration, typeList, out maxSize);
            string typestr = string.Join('|', typeList);
            string json = string.Format("\"filetypes\":\"{0}\",\"maxsize\": {1}", typestr, maxSize);
            return Ok('{'+json+'}');
        }
        [HttpGet("files")]
        public IEnumerable<BLL.Models.FileResult> Files()
        {
            List<BLL.Models.FileResult> list = new List<BLL.Models.FileResult>();           
            var fileDir = GetFileDir();
            DirectoryInfo directoryInfo = new DirectoryInfo(fileDir);
            if (directoryInfo.Exists)
            {
                string url = GetBaseUrl();
                foreach (FileInfo fInfo in directoryInfo.EnumerateFiles())
                {
                    string link, icon, fileType;
                    bool isImage;
                    FileService.Instance.GetFielInfo(fileDir, url, fInfo.Name, out link, out icon, out isImage, out fileType);                    
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
            catch (Exception ex)
            {
                return StatusCode(500, $"File {fileName} is not found. Error: {ex}");
            }
            return Ok("OK");
        }
    }
}
