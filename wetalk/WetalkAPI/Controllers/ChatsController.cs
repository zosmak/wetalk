using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WetalkAPI.Services;
using WetalkAPI.Models.Users;
using WetalkAPI.Helpers;
using AutoMapper;
using WetalkAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WetalkAPI.Models.Chat;

namespace WetalkAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ChatsController(
            IUserService userService,
            IChatService chatService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _chatService = chatService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <response code="200">Returns the user chat</response>
        /// <response code="401">Unauthorized</response>
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var userChats = _chatService.GetAll(1);

            return Ok(userChats);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <response code="200">Returns the created user</response>
        /// <response code="400">Bad request</response>            
        /// <response code="403">Forbidden</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Create([FromBody] CreateChatModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "invalid request body");
                return BadRequest(ModelState);
            }

            if (User.Identity.Name != null)
            {
                // map model to entity
                var chatEntity = new Chat()
                {
                    Name = model.Name,
                    OwnerID = int.Parse(User.Identity.Name),
                };
                try
                {
                    // create new chat
                    Chat newChat = _chatService.CreateChat(chatEntity);

                    // update chat members
                    foreach (int memberID in model.Members)
                    {
                        _chatService.CreateChatMember(newChat.ID, memberID);
                    }

                    return Ok(newChat);
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            }

            return Forbid();
        }
    }
}
