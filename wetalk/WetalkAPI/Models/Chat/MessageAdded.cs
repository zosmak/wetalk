using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WetalkAPI.Models.Chat
{
    public class MessageAdded
    {
        [Required]
        public int ChatID { get; set; }

        [Required]
        public int SenderID { get; set; }

        [Required]
        public string Message { get; set; }
    }
}