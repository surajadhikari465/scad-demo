using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Price.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Price.Controller.DataAccess.Queries
{
    public class GetCancelAllSalesDataParameters : IQuery<List<CancelAllSalesEventModel>>
    {
        public int Instance { get; set; }
    }
}
