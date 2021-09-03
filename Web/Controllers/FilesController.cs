using Domain.Interfaces;
using Domain.Models.Responses;
using Help.Exceptions;
using Help.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : AbstractController<FilesController>
    {
        IFileService _fileService;
        IFileLogic _logic;
        public FilesController(IConfiguration configuration, ILogger<FilesController> logger, ITranslator translator, IFileLogic logic, IFileService fileService) : base(configuration, logger, translator)
        {
            _logic = logic;
            _fileService = fileService;
        }
        [HttpPost("upload/{sessionid}/{language}")]
        public IActionResult Upload(string sessionid, string language)
        {
            return this.RequestHandler(language, () =>
            {
                string userId = _logic.GetUserId(language, sessionid);
                var file = Request.Form.Files[0];
                string message = _fileService.ValidFile(userId, file.FileName, file.Length, language);
                if (!string.IsNullOrEmpty(message))
                {
                    throw new MessagesException(new List<string> { message });
                }
                string filePath = _fileService.GetFilePath(userId, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok(new MessageResponse { Message = "OK" });
            });
        }

        [HttpDelete("{fileName}/{sessionid}/{language}")]
        public IActionResult DeleteFile(string sessionid, string fileName, string language)
        {
            return this.RequestHandler(language, () =>
            {
                string userId = _logic.GetUserId(language, sessionid);
                string filePath = _fileService.GetFilePath(userId, fileName);
                System.IO.File.Delete(filePath);
                return Ok(new MessageResponse { Message = "OK" });
            });
        }
        [HttpGet("icon/{fileName}/{sessionid}/{language}")]
        public IActionResult GetIcon(string sessionid, string fileName, string language)
        {
            return this.RequestHandler(language, () =>
            {
                string contentType = "image/jpeg";
                string filePath = "";
                bool isImage = _fileService.IsImage(fileName);
                if (isImage)
                {
                    string userId = _logic.GetUserId(language, sessionid);
                    filePath = _fileService.GetFilePath(userId, fileName);
                }
                else
                {
                    filePath = _fileService.GetIcon(fileName);
                }
                FileStream stream = System.IO.File.OpenRead(filePath);
                return File(stream, contentType);
            });
        }
        [HttpGet("{fileName}/{sessionid}/{language}")]
        public IActionResult GetFile(string sessionid, string fileName, string language)
        {
            return this.RequestHandler(language, () =>
            {
                string userId = _logic.GetUserId(language, sessionid);
                string contentType = _fileService.GetContentType(fileName);
                string filePath = _fileService.GetFilePath(userId, fileName);
                FileStream stream = System.IO.File.OpenRead(filePath);
                return File(stream, contentType);
            });
        }
        [HttpGet("{sessionid}/{language}")]
        public IActionResult GetFileList(string sessionid, string language)
        {
            return this.RequestHandler(language, () =>
            {
                string userId = _logic.GetUserId(language, sessionid);
                List<string> list = _fileService.GitFileList(userId);
                return Ok(list);
            });
        }
    }
}
