using System.Collections.Generic;

namespace WetalkAPI.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Active { get; set; }


        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public int PermissionID { get; set; }
        public UserPermission Permission { get; set; }
        public ICollection<UserFile> Files { get; set; }
    }
}
