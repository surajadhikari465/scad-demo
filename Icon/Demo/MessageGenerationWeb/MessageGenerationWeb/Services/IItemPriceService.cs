using MessageGenerationWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGenerationWeb.Services
{
    public interface IItemPriceService
    {
        void DeleteItemPrices(List<ItemPriceDeleteModel> itemPrices);
    }
}
