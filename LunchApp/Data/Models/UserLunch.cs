using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class UserLunch
    {
        public int UserLunchId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string FilePath { get; set; }

        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }

        public bool Submitted { get; set; }
        public bool Editable { get; set; }

        public DateTime SubmitionDate { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("UserLunchId")]
        public ICollection<UserLunchItem> UserLunchItems { get; set; }
    }
}
