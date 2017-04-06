using Icon.Infor.Listeners.Price.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.DataAccess.Commands
{
    public class DeletePricesCommand
    {
        public IEnumerable<DbPriceModel> Prices { get; set; }
    }
}
