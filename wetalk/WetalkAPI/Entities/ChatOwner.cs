using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WetalkAPI.Entities
{
    public class ChatOwner
    {
        [Key]
        public int ChatID { get; set; }
        public Chat Chat { get; set; }

        [Key]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
