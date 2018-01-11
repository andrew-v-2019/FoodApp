using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class MenuSection
    {
        public int MenuSectionId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
