using System;
using System.Collections.Generic;

namespace WetalkAPI.Entities
{
    public class Message
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int SenderID { get; set; }
        public User Sender { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ChatID { get; set; }
        public Chat Chat { get; set; }
    }
}
