using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models.DataMonster
{
   public class SearchItemModel
    {
        public string BrandName { get; set; }
        public string Subteam { get; set; }
        public string Supplier { get; set; }
        public string ItemDescription { get; set; }
        public string Region { get; set; }
        public bool IncludeLocales { get; set; }
        public bool AllLocales { get; set; }
        public List<string> IncludedStores { get; set; }
    }
}
