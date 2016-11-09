using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class PluCategoryGridViewModel
    {
        public IEnumerable<PluCategoryViewModel> PluCategories { get; set; }
    }
}