using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class OrderUserLunch
    {
        public int OrderUserLunchId { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int UserLunchId { get; set; }
        [ForeignKey("UserLunchId")]
        public UserLunch UserLunch { get; set; }
    }
}
