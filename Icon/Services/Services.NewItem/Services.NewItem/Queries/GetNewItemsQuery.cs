using Icon.Common.DataAccess;
using Services.NewItem.Infrastructure;
using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Queries
{
    public class GetNewItemsQuery : IQuery<IEnumerable<NewItemModel>>, IRegionalParameter
    {
        public int Instance { get; set; }
        public int NumberOfItemsInMessage { get; set; }
        public string Region { get; set; }
    }
}
