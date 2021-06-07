using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WetalkAPI.Entities
{
    public class UserFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FileName { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
