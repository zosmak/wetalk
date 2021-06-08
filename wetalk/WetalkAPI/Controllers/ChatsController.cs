using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using WetalkAPI.Services;
using WetalkAPI.Helpers;
using AutoMapper;
using WetalkAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WetalkAPI.Models.Chat;
using System.Linq;

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
        /// Gets all user chats
        /// </summary>
        /// <response code="200">Returns the user chat</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetAll()
        {
            if (User.Identity.Name != null)
            {
                try
                {
                    // create new chat
                    var userChats = _chatService.GetAllUserChat(int.Parse(User.Identity.Name));
                    return Ok(userChats);
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            }

            return Forbid();
        }

        /// <summary>
        /// Creates a chat
        /// </summary>
        /// <response code="200">Returns the created chat</response>
        /// <response code="400">Bad request</response>            
        /// <response code="401">Unauthorized</response>            
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
                    _chatService.CreateChatMember(newChat.ID, int.Parse(User.Identity.Name));

                    foreach (string username in model.Members)
                    {
                        var user = _userService.GetByUsername(username);
                        _chatService.CreateChatMember(newChat.ID, user.ID);
                    }

                    var result = _mapper.Map<ChatModel>(newChat);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            }

            return Unauthorized();
        }

        /// <summary>
        /// Creates a chat message
        /// </summary>
        /// <response code="200">Returns the created message</response>
        /// <response code="400">Bad request</response>            
        /// <response code="401">Unauthorized</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{id}/message")]
        public IActionResult Create(int id, [FromBody] CreateChatMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "invalid request body");
                return BadRequest(ModelState);
            }

            if (User.Identity.Name != null)
            {
                // check if current user is a member
                var currentUserChats = _chatService.GetAllUserChat(int.Parse(User.Identity.Name));

                if (currentUserChats.Any(x => x.ID == id))
                {
                    try
                    {
                        _chatService.CreateChatMessage(id, int.Parse(User.Identity.Name), model.Message);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        // return error message if there was an exception
                        return BadRequest(new { message = ex.Message });
                    }
                }
                return BadRequest("The chat doens't exist or the user isn't in that chat");
            }

            return Unauthorized();
        }

        /// <summary>
        /// Deletes a chat message
        /// </summary>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request</response>            
        /// <response code="401">Unauthorized</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("message/{messageID}")]
        public IActionResult Delete(int messageID)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    // check if current user is the sender of message
                    var message = _chatService.GetMessageByID(messageID);

                    if (message != null && message.SenderID == int.Parse(User.Identity.Name))
                    {
                        _chatService.DeleteChatMessage(messageID);
                        return NoContent();
                    }
                    return BadRequest("Message not found or user doens't have permission to remove it");
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Marks a chat message as read
        /// </summary>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request</response>            
        /// <response code="401">Unauthorized</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("message/{messageID}/read")]
        public IActionResult ReadMessage(int messageID)
        {
            try
            {
                if (User.Identity.Name != null)
                {

                    _chatService.MarkMessageAsRead(int.Parse(User.Identity.Name), messageID);
                    return NoContent();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
