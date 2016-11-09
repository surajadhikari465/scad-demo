using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using System.Collections.Generic;

namespace Mammoth.ItemLocale.Controller.DataAccess.Queries
{
    public class GetItemLocaleDataParameters : IQuery<List<ItemLocaleEventModel>>
    {
        public int Instance { get; set; }
        public string Region { get; set; }
    }
}
