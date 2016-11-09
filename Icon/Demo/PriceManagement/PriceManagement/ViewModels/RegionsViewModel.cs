using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PriceManagement.ViewModels
{
    public static class RegionsViewModel
    {
        public static List<SelectListItem> All = new List<SelectListItem>
        {
            new SelectListItem { Text = "FL", Value = "FL" },
            new SelectListItem { Text = "MA", Value = "MA" },
            new SelectListItem { Text = "MW", Value = "MW" },
            new SelectListItem { Text = "NA", Value = "NA" },
            new SelectListItem { Text = "NC", Value = "NC" },
            new SelectListItem { Text = "NE", Value = "NE" },
            new SelectListItem { Text = "PN", Value = "PN" },
            new SelectListItem { Text = "RM", Value = "RM" },
            new SelectListItem { Text = "SO", Value = "SO" },
            new SelectListItem { Text = "SP", Value = "SP" },
            new SelectListItem { Text = "SW", Value = "SW" },
            new SelectListItem { Text = "UK", Value = "UK" }
        };
    }
}
