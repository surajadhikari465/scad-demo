using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Models.DataMonster;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemsQuery : ItemComposite, IQuery<ItemComposite>
    {
        public List<string> ScanCodes { get; set; }
    }
}
