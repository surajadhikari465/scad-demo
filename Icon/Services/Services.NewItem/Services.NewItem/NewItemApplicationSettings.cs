using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem
{
    public class NewItemApplicationSettings
    {
        public List<string> Regions { get; set; }
        public int NumberOfItemsPerMessage { get; set; }
        public bool SendOrganic { get; set; }
        public bool IncludeCustomerFacingDescription { get; set; }

        public NewItemApplicationSettings()
        {
            Regions = new List<string>();
        }

        public static NewItemApplicationSettings CreateFromConfig()
        {
            return new NewItemApplicationSettings
            {
                Regions = AppSettingsAccessor.GetStringSetting("Regions").Split(',').ToList(),
                NumberOfItemsPerMessage = AppSettingsAccessor.GetIntSetting("NumberOfItemsPerMessage", 100),
                SendOrganic = AppSettingsAccessor.GetBoolSetting("SendOrganic", false),
                IncludeCustomerFacingDescription = AppSettingsAccessor.GetBoolSetting("IncludeCustomerFacingDescription", false)
            };
        }
    }
}
