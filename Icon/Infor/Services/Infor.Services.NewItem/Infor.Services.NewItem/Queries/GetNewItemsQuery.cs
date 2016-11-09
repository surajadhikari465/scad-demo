using Icon.Common.DataAccess;
using Infor.Services.NewItem.Infrastructure;
using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Queries
{
    public class GetNewItemsQuery : IQuery<IEnumerable<NewItemModel>>, IRegionalParameter
    {
        public int Instance { get; set; }
        public int NumberOfItemsInMessage { get; set; }
        public string Region { get; set; }
    }
}
