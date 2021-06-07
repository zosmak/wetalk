using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WetalkAPI.Entities
{
    public class MessageRead
    {
        [Key]
        public int MessageID { get; set; }
        public Message Message { get; set; }


        [Key]
        public int UserID { get; set; }
        public User User { get; set; }

        public DateTime ReadAt { get; set; }
    }
}
