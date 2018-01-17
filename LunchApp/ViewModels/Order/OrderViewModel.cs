using System.Collections.Generic;
using ViewModels.UserLunch;

namespace ViewModels.Order
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public bool Submitted { get; set; }
        public string SubmitionDate { get; set; }
        public List<UserLunchViewModel> UserLunches { get; set; }
        public int MenuId { get; set; }
    }
}
