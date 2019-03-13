using KitBuilder.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class LinkGroupView
    {
        public LinkGroupView()
        {
        }

        public int LinkGroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string LinkGroupProperties { get; set; }
        public string FormattedLinkGroupProperties { get; set; }
        public bool Excluded { get; set; }
        public ICollection<ModifierView> Modifiers { get; set; }
    }
}
