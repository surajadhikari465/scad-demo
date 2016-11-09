using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    public class ItemsBySearchResultsModel
    {
        public List<ItemSearchModel> Items { get; set; }
        public int ItemsCount { get; set; }
    }
}
