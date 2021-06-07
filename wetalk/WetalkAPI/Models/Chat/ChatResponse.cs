using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WetalkAPI.Models.Chat
{
    public class ChatResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public List<ResponseMessage> Messages { get; set; }

        public class ResponseMessage
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public int SenderID { get; set; }
            public DateTime CreatedAt { get; set; }
            public int ChatID { get; set; }
            public bool Read { get; set; }
        }
    }
}