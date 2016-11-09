using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateBrandHierarchyClassTraitsCommand
    {
        public HierarchyClass Brand { get; set; }
        public string BrandAbbreviation { get; set; }
    }
}
