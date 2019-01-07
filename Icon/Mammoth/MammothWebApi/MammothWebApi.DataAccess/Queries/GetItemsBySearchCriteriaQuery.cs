using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models.DataMonster;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemsBySearchCriteriaQuery : ItemDetail, IQuery<IEnumerable<ItemDetail>>
    {
        public string BrandName { get; set; }
        public string Subteam { get; set; }
        public string Supplier { get; set; }
        public string ItemDescription { get; set; }
        public string Region { get; set; }
        public bool IncludeLocales { get; set; }
        public List<string> IncludedStores { get; set; }
    }
}