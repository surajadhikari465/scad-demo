using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Models
{
    public class RegionModel
    {
        public string RegionAbbr { get; set; }

        public static List<RegionModel> CreateRegionModelsFromConfig()
        {
            List<RegionModel> regions = AppSettingsAccessor.GetStringSetting("IconToIrmaTaxHierarchyUpdateEnabledRegionsList", false)
                .Split(',')
                .Where(r => !String.IsNullOrWhiteSpace(r))
                .Select(r => new RegionModel { RegionAbbr = r.Trim() })
                .ToList();

            return regions;
        }
    }
}
