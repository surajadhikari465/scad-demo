using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item
{
    public class ItemListenerSettings
    {
        public bool ValidateSequenceId { get; set; }

        public static ItemListenerSettings CreateFromConfig()
        {
            return new ItemListenerSettings
            {
                ValidateSequenceId = AppSettingsAccessor.GetBoolSetting(nameof(ValidateSequenceId))
            };
        }
    }
}
