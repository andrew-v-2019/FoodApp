using System;
namespace Data.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public DateTime LunchDate { get; set; }

        public DateTime CreationDate { get; set; }

        public double? Price { get; set; }

        public string FilePath { get; set; }

        public bool Active { get; set; }

        public bool Editable { get; set; }

    }
}
