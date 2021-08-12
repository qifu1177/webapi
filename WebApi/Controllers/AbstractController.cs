﻿using BLL;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;

namespace WebApi.Controllers
{
    public abstract class AbstractController : ControllerBase
    {
        protected readonly ILogger<DataController> _logger;
        protected readonly IConfiguration _configuration;
        protected string _connectString;
        public AbstractController(IConfiguration configuration, ILogger<DataController> logger)
        {
            _configuration = configuration;
            _connectString = _configuration.GetConnectionString("DB");
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("live");
        }
            

        [HttpGet("{sessionid}/{fileName}")]
        public IActionResult GetFile(string sessionid,string fileName)
        {
            DateTime updateTs = DateTime.UtcNow;
            try
            {
                var fileDir = GetFileDirWithSessionId(sessionid, updateTs);
                if (string.IsNullOrEmpty(fileDir))
                    return BadRequest();
                string filePath = Path.Combine(fileDir, fileName);
                FileInfo fileInfo = new FileInfo(filePath);
                if(fileInfo.Exists)
                {
                    string extension = fileInfo.Extension.ToLower();
                    string contentType = "application/octet-stream";
                    if (IsImage(extension))
                    {                        
                        contentType= "image/jpeg";
                    }
                    else if(extension.EndsWith("pdf"))
                    {
                        contentType = "application/pdf";
                    }
                    else if (extension.EndsWith("json") || extension.EndsWith("txt") )
                    {
                        contentType = "text/html; charset=UTF-8";
                    }
                    else
                    {
                        contentType = "application/octet-stream";                        
                    }
                    FileStream stream = System.IO.File.OpenRead(filePath);
                    return File(stream, contentType);
                }
                else
                {
                    return StatusCode(500, $"File {fileName} is not found.");
                }                
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"File {fileName} is not found. Error: {ex}");
            }            
        }
        [HttpGet("icon/{fileName}")]
        public IActionResult GetIcon(string fileName)
        {
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                var fileDir = Path.Combine(currentDir, "icons");
                string filePath = Path.Combine(fileDir, fileName);
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    string extension = fileInfo.Extension.ToLower();
                    string contentType = "image/jpeg";                                     
                    FileStream stream = System.IO.File.OpenRead(filePath);
                    return File(stream, contentType);
                }
                else
                {
                    return StatusCode(500, $"File {fileName} is not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"File {fileName} is not found. Error: {ex}");
            }
        }
        private bool IsImage(string extension)
        {            
            if (extension.EndsWith("jpg") || extension.EndsWith("jpeg") || extension.EndsWith("png") || extension.EndsWith("gif") || extension.EndsWith("bmp"))
                return true;
            return false;
        }

        protected string GetFileDir(string userId)
        {
            string currentDir = Directory.GetCurrentDirectory();
            return Path.Combine(currentDir, "files",userId);
        }

        protected string GetFileDirWithSessionId(string sessionId,DateTime updateTs)
        {
            string userId = UserLogic.Instance.SetConnectString(_connectString).GetUserIdAndUpdateUpdateTs(sessionId, updateTs);
            if (string.IsNullOrEmpty(userId))
                return string.Empty;
            var fileDir = GetFileDir(userId);
            return fileDir;
        }

        protected string GetBaseUrl()
        {
            return $"{Request.Scheme}://{Request.Host}";
        }
    }
}
