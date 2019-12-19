using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model
{
    public class SkuCountRepository : ISkuCountRepository
    {
        private List<SkuCount> items = new List<SkuCount>();
        private IOOSEntitiesFactory entitiesFactory;

        public int Count {get { return items.Count; }}

        public SkuCountRepository(IOOSEntitiesFactory entitiesFactory)
        {
            this.entitiesFactory = entitiesFactory;
        }

        public SkuCount For(string storeAbbrev, string team)
        {
            if (!items.Any(p => p == new SkuCount(storeAbbrev, team, -1)))
            {
                var searched = Search(storeAbbrev, team);
                if (searched != null)
                {
                    items.Add(searched);
                }
            }
            return items.FirstOrDefault(p => p == new SkuCount(storeAbbrev, team, -1));
        }

        private SkuCount Search(string storeAbbreviation, string team)
        {
            using (var dbContext = entitiesFactory.New())
            {
                var psBu = PsBuFor(storeAbbreviation);
                var teamId = TeamIdFor(team);
                if (psBu <= 0 || teamId <= 0) return null;
                var searched = (from c in dbContext.SKUCount
                                where c.STORE_PS_BU == psBu && c.TEAM_ID == teamId
                                select new { StoreAbbrevation = storeAbbreviation, Team = team, Count = c.numberOfSKUs }).FirstOrDefault();
                return searched != null ? new SkuCount(searched.StoreAbbrevation, searched.Team, searched.Count.HasValue ?  searched.Count.Value : -1) : null;
            }
        }

        private int PsBuFor(string storeAbbreviation)
        {
            using (var dbContext = entitiesFactory.New())
            {
                var psBu = (from c in dbContext.STORE where c.STORE_ABBREVIATION == storeAbbreviation select c.PS_BU).FirstOrDefault();
                return psBu != null ? Convert.ToInt32(psBu) : 0;
            }
        }

        private int TeamIdFor(string team)
        {
            using (var dbContext = entitiesFactory.New())
            {
                return (from c in dbContext.TEAM_Interim where c.teamName == team select c.idTeam).FirstOrDefault();
            }
        }

        public void Insert(string storeAbbrev, string team, int count)
        {
            var item = new SkuCount(storeAbbrev, team, count);
            if (For(storeAbbrev, team) == null)
            {
                items.Add(item);
                Save(item);
            }
        }

        public void Modify(string storeAbbrev, string team, int count)
        {
            var item = new SkuCount(storeAbbrev, team, count);
            if (For(storeAbbrev, team) != null)
            {
                items.Remove(item);
                items.Add(item);
                Update(item);
            }
        }

        private void Save(SkuCount item)
        {
            using (var dbContext = entitiesFactory.New())
            {
                var psBu = PsBuFor(item.StoreAbbreviation);
                var teamId = TeamIdFor(item.Team);
                if (psBu <= 0 || teamId <= 0) return;
                dbContext.SKUCount.AddObject(new SKUCount { numberOfSKUs = item.Count, STORE_PS_BU = psBu, TEAM_ID = teamId }); 
                dbContext.SaveChanges();
            }
        }

        private void Update(SkuCount item)
        {
            using (var dbContext = entitiesFactory.New())
            {
                var psBu = PsBuFor(item.StoreAbbreviation);
                var teamId = TeamIdFor(item.Team);
                if (psBu <= 0 || teamId <= 0) return;
                var searched = (from c in dbContext.SKUCount where c.STORE_PS_BU == psBu && c.TEAM_ID == teamId 
                                select c).FirstOrDefault();
                if (searched == null) return;
                searched.numberOfSKUs = item.Count;
                dbContext.SaveChanges();
            }
        }

        public void Remove(string storeAbbrev, string team)
        {
            var item = new SkuCount(storeAbbrev, team, -1);
            if (For(storeAbbrev, team) != null)
            {
                items.Remove(item);
                Delete(item);
            }
        }

        private void Delete(SkuCount item)
        {
            using(var dbContext = entitiesFactory.New())
            {
                var psBu = PsBuFor(item.StoreAbbreviation);
                var teamId = TeamIdFor(item.Team);
                if (psBu <= 0 || teamId <= 0) return;
                var searched = (from c in dbContext.SKUCount
                                where c.STORE_PS_BU == psBu && c.TEAM_ID == teamId
                                select c).FirstOrDefault();
                if (searched == null) return;
                dbContext.SKUCount.DeleteObject(searched);
                dbContext.SaveChanges();
            }
        }
    }
}
