using System.Collections.Generic;
using ViewModels.UserLunch;

namespace ClassLibrary5.UserLunch
{
    public class UserLunchSectionViewModel
    {
        public string Name { get; set; }
        public List<UserLunchItemViewModel> Items { get; set; }
        public int Number { get; set; }
        public int MenuSectionId { get; set; }
        public int MenuId { get; set; }
    }
}
