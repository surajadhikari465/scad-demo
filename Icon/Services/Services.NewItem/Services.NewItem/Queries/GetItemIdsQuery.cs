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
    public class GetItemIdsQuery : IQuery<Dictionary<string, int>>
    {
        public List<string> ScanCodes { get; set; }
    }
}

