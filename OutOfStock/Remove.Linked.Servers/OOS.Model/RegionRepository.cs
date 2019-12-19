using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private IConfigurator config;
        private IOOSEntitiesFactory dbFactory;


        public RegionRepository(IConfigurator config, IOOSEntitiesFactory dbFactory)
        {
            this.config = config;
            this.dbFactory = dbFactory;
        }

        public Region ForAbbrev(string abbrev)
        {
            var region = GetRegionDB(abbrev);
            if (region != null)
            {
                var r = SQLRegionFactory.Reconstitute(region);
                var stores = GetStoresDB(abbrev);
                stores.ForEach(r.AddStore);
                return r;
            }
            return null;
        }

        private REGION GetRegionDB(string abbrev)
        {
            using (var db = dbFactory.New())
            {
                var regions = from r in db.REGION where r.REGION_ABBR == abbrev select r;
                return regions.FirstOrDefault();
            }
        }

        private List<Store> GetStoresDB(string regionAbbrev)
        {
            using (var db = dbFactory.New())
            {
                var stores = (from s in db.STORE
                          join r in db.REGION on s.REGION_ID equals r.ID
                          join ss in db.STATUS on s.STATUS_ID equals ss.ID
                          where r.REGION_ABBR.Equals(regionAbbrev, StringComparison.OrdinalIgnoreCase) && !s.Hidden
                          orderby s.STORE_ABBREVIATION
                          select new { Id = s.ID, Name = s.STORE_NAME, Abbrev = s.STORE_ABBREVIATION, Status = ss.STATUS1, BusinessUnitNo = s.PS_BU }).ToList();
                return stores.Select(store => new Store(store.Id) {Name = store.Name, Abbrev = store.Abbrev, Status = store.Status, BusinessUnitNo = store.BusinessUnitNo}).ToList();

            }
        }


        public IEnumerable<string> RegionAbbreviations()
        {
            var regions = GetRegionNameAbbreviations();
            return regions.Select(r => r.Value);
        }

        private IEnumerable<KeyValuePair<string, string>> GetRegionNameAbbreviations()
        {
            using (var dbContext = dbFactory.New())
            {
                var regions = (from r in dbContext.REGION 
                               where r.IS_VISIBLE.Equals("true", StringComparison.InvariantCultureIgnoreCase)
                               orderby r.REGION_ABBR select new { Key = r.REGION_NAME.Trim() , Value = r.REGION_ABBR.Trim() }).ToList();
                return regions.Select(r => new KeyValuePair<string, string>(r.Key, r.Value));
            }
        }
        
        public IEnumerable<string> RegionNames()
        {
            var regions = GetRegionNameAbbreviations();
            return regions.Select(r => r.Key);
        }

        public Region ForName(string name)
        {
            var regionAbbrev = RegionAbbrevFor(name.Trim());
            return ForAbbrev(regionAbbrev);
        }

        private string RegionAbbrevFor(string name)
        {
            var regionWithName = GetRegionNameAbbreviations().Where(r => r.Key == name);
            return regionWithName.Any() ? regionWithName.First().Value : string.Empty;
        }
    }

}
