using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models.DataMonster
{
    public class ItemComposite
    {
        public List<ItemInformation> ItemInformation { get; set; }
        public List<ItemLocaleInformation> ItemLocaleInformation { get; set; }
    }
}