using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public int MenuSectionId { get; set; }

        [ForeignKey("MenuSectionId")]
        public MenuSection MenuSection { get; set; }

        public string Name { get; set; }
        public int Number { get; set; }
        public int MenuId { get; set; }

        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
    }
}
