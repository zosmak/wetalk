using System.ComponentModel.DataAnnotations;

namespace WetalkAPI.Models.Users
{
    public class AuthenticateUserModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}