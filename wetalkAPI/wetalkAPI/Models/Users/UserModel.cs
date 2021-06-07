namespace WetalkAPI.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public int Active { get; set; }
        public int PermissionID { get; set; }
    }
}