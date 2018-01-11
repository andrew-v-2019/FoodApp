using System.Collections.Generic;

namespace ViewModels
{
    public class MenuSectionViewModel
    {
        public string Name { get; set; }
        public List<MenuItemViewModel> Items { get; set; }
        public int Number { get; set; }
        public int MenuSectionId { get; set; }
        public int MenuId { get; set; }
    }
}
