using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WetalkAPI.Models.Chat
{
    public class CreateChatMessageModel
    {
        [Required]
        public string Message { get; set; }
    }
}