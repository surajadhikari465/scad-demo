using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Icon.Web.Extensions;

namespace Icon.Web.Mvc.Models
{
    public class HierarchySearchViewModel
    {
        [Display(Name = "Hierarchy")]
        public int SelectedHierarchyId { get; set; }
        public IEnumerable<SelectListItem> Hierarchies { get; set; }
        public IQueryable<HierarchyClassGridViewModel> HierarchyClasses { get; set; }
        public Hierarchy Hierarchy { get; set; }
    }
}