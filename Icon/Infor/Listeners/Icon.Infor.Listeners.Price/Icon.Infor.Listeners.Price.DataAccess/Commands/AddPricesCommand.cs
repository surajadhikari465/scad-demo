using Icon.Infor.Listeners.Price.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Price.DataAccess.Commands
{
    public class AddPricesCommand
    {
        public IEnumerable<DbPriceModel> Prices { get; set; }
    }
}
