using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class ItemTraitModel
    {
        public ItemTraitModel(int itemId, int traitId, string traitValue, int localeId)
        {
            ItemId = itemId;
            TraitId = traitId;
            TraitValue = traitValue;
            LocaleId = localeId;
        }

        public int ItemId { get; set; }
        public int TraitId { get; set; }
        public string TraitValue { get; set; }
        public int LocaleId { get; set; }
    }
}
