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

namespace MovieseekAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public FilesController(
            IFileService fileService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _fileService = fileService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Upload an user file
        /// </summary>
        /// <response code="200">Returns OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
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

            return Ok();
        }

        /// <summary>
        /// Download an user file
        /// </summary>
        /// <response code="200">Returns the users</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


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
}
