using KitBuilder.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class ModifierView
    {
        public ModifierView()
        {
        }
        public bool AuthorizedByStore { get; set; }
        public bool Excluded { get; set; }
        public int ItemID { get; set; }
        public string ModifierName { get; set; }
        public string ModifierProperties { get; set; }
        public string Price { get; set; }
        public string Calories { get; set; }
        public string FormattedModifierProperties { get; set; }
    }
}
