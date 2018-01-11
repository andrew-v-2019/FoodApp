using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class UserLunchItem
    {
        public int UserLunchItemId { get; set; }
        public int UserLunchId { get; set; }
        [ForeignKey("UserLunchId")]
        public UserLunch UserLunch { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; }
        public DateTime Date { get; set; }
    }
}
