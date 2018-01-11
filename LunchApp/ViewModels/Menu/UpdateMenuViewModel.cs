using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ViewModels.Menu
{
    public class UpdateMenuViewModel
    {
      
        [JsonProperty(PropertyName = "menuId")]
        public int MenuId { get; set; }
        [JsonProperty(PropertyName = "sections")]
        public List<MenuSectionViewModel> Sections { get; set; }
        [JsonProperty(PropertyName = "lunchDate")]
        public string LunchDate { get; set; }
        [JsonProperty(PropertyName = "price")]
        public double? Price { get; set; }

        public bool Editable { get; set; }
    }
}
