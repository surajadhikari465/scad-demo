using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class ConfigurationSearchViewModel
    {
        [Display(Name = "Region Code")]
        public string SelectedRegionCode { get; set; }

        public SelectList RegionList { get; set; }

        public List<ConfigurationViewModel> RegionItemConfigurationList { get; set; }

        public List<ConfigurationViewModel> RegionFinancialSubTeamConfigurationList { get; set; }
    }
}