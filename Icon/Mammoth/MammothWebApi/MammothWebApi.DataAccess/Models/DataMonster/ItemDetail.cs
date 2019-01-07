using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models.DataMonster
{
    public class ItemDetail
    {
        public ItemDetailInformation ItemInformation { get; set; }
        public List<ItemDetailLocaleInformation> ItemDetailLocaleInformation { get; set; }

    }
}