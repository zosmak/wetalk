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

namespace WetalkAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <response code="200">Returns the user with access token</response>
        /// <response code="400">Bad request</response>            
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthenticateUserModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null || (user != null && user.Active == 0))
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info, movies and authentication token
            return Ok(new
            {
                ID = user.ID,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString,
            });
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <response code="200">Returns the created user autenticated</response>
        /// <response code="400">Bad request</response>            
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "invalid request body");
                return BadRequest(ModelState);
            }

            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);

                AuthenticateUserModel authModel = new AuthenticateUserModel()
                {
                    Username = user.Username,
                    Password = model.Password
                };
                return Authenticate(authModel);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <response code="201">Returns the users</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetAll()
        {
            if (User.Identity.Name != null)
            {
                // get user permissions
                var currentUser = _userService.GetById(int.Parse(User.Identity.Name));

                if (currentUser == null)
                {
                    BadRequest("Current user not found");
                }

                if (currentUser.PermissionID == 1)
                {
                    var users = _userService.GetAll();
                    var model = _mapper.Map<IList<UserModel>>(users);
                    return Ok(model);
                }
            }

            return Forbid();
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <response code="200">Returns the user</response>
        /// <response code="401">Unauthorized</response>            
        /// <response code="403">Forbidden</response>
        /// <response code="404">User wasn't found</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (User.Identity.Name != null)
            {
                // get user permissions
                var currentUser = _userService.GetById(int.Parse(User.Identity.Name));

                if (currentUser == null)
                {
                    BadRequest("Current user not found");
                }

                if (currentUser.PermissionID == 1)
                {
                    var user = _userService.GetById(id);
                    if (user != null)
                    {
                        var model = _mapper.Map<UserModel>(user);
                        return Ok(model);
                    }
                    return NotFound();
                }
            }

            return Forbid();
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <response code="200">Returns the created user</response>
        /// <response code="400">Bad request</response>            
        /// <response code="403">Forbidden</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "invalid request body");
                return BadRequest(ModelState);
            }

            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                if (User.Identity.Name != null)
                {
                    // get user permissions
                    var currentUser = _userService.GetById(int.Parse(User.Identity.Name));

                    if (currentUser == null)
                    {
                        BadRequest("Current user not found");
                    }

                    if (currentUser.PermissionID == 1)
                    {
                        // create user
                        _userService.Create(user, model.Password);

                        var result = _mapper.Map<UserModel>(user);
                        return Ok(result);
                    }
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an user
        /// </summary>
        /// <response code="200">Returns the updated user</response>
        /// <response code="400">Bad request</response>            
        /// <response code="401">Unauthorized</response>            
        /// <response code="404">User wasn't found</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "invalid request body");
                return BadRequest(ModelState);
            }

            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.ID = id;

            try
            {
                if (User.Identity.Name != null)
                {
                    // get user permissions
                    var currentUser = _userService.GetById(int.Parse(User.Identity.Name));

                    if (currentUser == null)
                    {
                        BadRequest("Current user not found");
                    }

                    if (currentUser.PermissionID == 1)
                    {
                        // update user
                        _userService.Update(user, model.Password);
                        var updatedUser = _userService.GetById(id);
                        var mappedUser = _mapper.Map<UserModel>(updatedUser);
                        return Ok(mappedUser);
                    }
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an user
        /// </summary>
        /// <response code="200">Returns OK</response>
        /// <response code="400">Bad request</response>            
        /// <response code="401">Unauthorized</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel userPasswordModel)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    // get user permissions
                    var currentUser = _userService.GetById(int.Parse(User.Identity.Name));

                    if (currentUser == null)
                    {
                        BadRequest("Current user not found");
                    }
                    // update user
                    _userService.Update(currentUser, userPasswordModel.Password);

                    return Ok();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <response code="204"></response>
        /// <response code="401">Unauthorized</response>            
        /// <response code="404">User wasn't found</response>            
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    // get user permissions
                    var currentUser = _userService.GetById(int.Parse(User.Identity.Name));

                    if (currentUser == null)
                    {
                        BadRequest("Current user not found");
                    }

                    if (currentUser.PermissionID == 1)
                    {
                        var user = _userService.GetById(id);
                        if (user != null)
                        {
                            _userService.Delete(id);
                            return NoContent();
                        }
                        return NotFound();
                    }
                }

                return Forbid();
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
