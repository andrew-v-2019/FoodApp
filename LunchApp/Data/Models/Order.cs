using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? SubmitionDate { get; set; }
        public bool Submitted { get; set; }

        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }

        [ForeignKey("OrderId")]
        public ICollection<OrderUserLunch> OrderUserLunches { get; set; }

        public int? SubmittedByUserId { get; set; }
        [ForeignKey("SubmittedByUserId")]
        public User SubmittedByUser { get; set; }
    }
}
