using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WetalkAPI.Models.Chat
{
    public class CreateChatModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<int> Members { get; set; }
    }
}