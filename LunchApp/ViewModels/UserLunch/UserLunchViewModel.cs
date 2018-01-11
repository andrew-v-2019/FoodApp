using System.Collections.Generic;
using ClassLibrary5.UserLunch;
using ViewModels.User;

namespace ViewModels.UserLunch
{
    public class UserLunchViewModel
    {
        public string LunchDate { get; set; }
        public double? Price { get; set; }
        public UserViewModel User { get; set; }
        public int MenuId { get; set; }
        public int UserLunchId { get; set; }
        public List<UserLunchSectionViewModel> Sections { get; set; }
        public bool Editable { get; set; }
    }
}
