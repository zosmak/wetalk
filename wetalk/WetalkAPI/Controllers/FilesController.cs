using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WetalkAPI.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.IO;
using WetalkAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WetalkAPI.Services;
using System.Security.Claims;
using System;
using WetalkAPI.Entities;
using System.Collections.Generic;
using WetalkAPI.Models.Files;

namespace WetalkAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public FilesController(
            IFileService fileService,
            IMapper mapper
            )
        {
            _fileService = fileService;
            _mapper = mapper;

        }

        /// <summary>
        /// Upload an user file
        /// </summary>
        /// <response code="200">Returns OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("upload")]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // associate filename to user
            UserFile newFile = new UserFile
            {
                FileName = file.FileName,
                UserID = int.Parse(User.Identity.Name)
            };
            _fileService.Create(newFile);

            return Ok();
        }

        /// <summary>
        /// Download an user file
        /// </summary>
        /// <response code="200">Returns the file</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        [HttpGet("download")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            // get file and check if current user has that file
            UserFile dbFile = _fileService.GetByName(filename);
            try
            {
                if (dbFile != null && dbFile.UserID == int.Parse(User.Identity.Name))
                {

                    var path = Path.Combine(
                                         Directory.GetCurrentDirectory(),
                                         "wwwroot", filename);

                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        stream.CopyTo(memory);
                    }
                    memory.Position = 0;
                    string mimeType = MimeTypes.GetMimeType(filename);
                    return File(memory, mimeType, Path.GetFileName(path));
                }
            }
            catch (Exception)
            {
            }

            return BadRequest("File doesn't exist or user doesn't have access to it.");
        }

        /// <summary>
        /// Get user files list
        /// </summary>
        /// <response code="200">Returns the users</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        [HttpGet("getUserFiles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUserFiles()
        {
            List<UserFile> files = _fileService.GetUserFiles(int.Parse(User.Identity.Name));
            var result = _mapper.Map<List<FileModel>>(files);
            return Ok(result);
        }

        /// <summary>
        /// Remove an user file
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        [HttpDelete("deleteUserFile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteUserFile(string fileName)
        {
            List<UserFile> files = _fileService.GetUserFiles(int.Parse(User.Identity.Name));

            var fileToDelete = files.Find(x => x.FileName == fileName);
            if (fileToDelete != null)
            {
                _fileService.Delete(fileName);
                return Ok();
            }

            return NotFound("File doesn't exist or user doens't have the permission to delete it");
        }
    }
}
