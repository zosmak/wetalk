using System.Collections.Generic;

namespace WetalkAPI.Entities
{
    public class UserFile
    {
        public string FileName { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
