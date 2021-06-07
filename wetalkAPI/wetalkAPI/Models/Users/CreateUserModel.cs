using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WetalkAPI.Models.Users
{
    public class CreateUserModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public int PermissionID { get; set; }
    }
}