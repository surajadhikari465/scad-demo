using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OOSCommon.DataContext;
using OutOfStock.Models;

namespace OutOfStock.Controllers
{

    public class RegionData
    {
        public string Name;
        public string Abbreviation;

        public RegionData()
        {   
        }

        public RegionData(string name, string abbrev)
        {
            this.Name = name;
            this.Abbreviation = abbrev;
        }

    }

    public class StoreCountData
    {
        public int StoreId;
        public string StoreName;
        public List<DepartmentCountData> DepartmentCountInfo;

        public StoreCountData()
        {
        }

        public StoreCountData(int id, string name, List<DepartmentCountData> countinfo)
        {
            this.StoreId = id;
            this.StoreName = name;
            this.DepartmentCountInfo = countinfo;
        }
    }

    public class DepartmentCountData
    {
        public int DepartmentId;
        public string DepartmentName;
        public int ItemCount;

        public DepartmentCountData()
        {
        }

        public DepartmentCountData(int id, string name, int count)
        {
            this.DepartmentId = id;
            this.DepartmentName = name;
            this.ItemCount = count;
        }

    }

    public class ItemCountRaw
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int Grocery { get; set; }
        public int WholeBody { get; set; }

        private const string SaveSql = "EXEC dbo.SaveItemCount @StoreId = {0}, @TeamName = {1},  @ItemCount = {2} ";

        public bool Save(OOSEntities context)
        {
            var i = 0;
            
            context.ExecuteStoreCommand(SaveSql, this.StoreId.ToString(),"Grocery", this.Grocery);
            context.ExecuteStoreCommand(SaveSql, this.StoreId.ToString(), "Whole Body", this.WholeBody);
                
            return true;
        }

    }

    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {

            var udp = OOSUser.EnableUDPLoggingForUser();

            return View();
        }


        public JsonResult Regions()
        {
            List<RegionData> regionsList = null;
            
            if (OOSUser.isCentral)
            {
                using (var db = new OOSEntities(MvcApplication.oosEFConnectionString))
                {
                    regionsList =
                        (from r in db.REGION where r.REGION_ABBR != "CEN" select new RegionData {Name = r.REGION_NAME, Abbreviation = r.REGION_ABBR})
                            .ToList();
                }
            }
            else if (OOSUser.isRegionalBuyer)
            {
                using (var db = new OOSEntities(MvcApplication.oosEFConnectionString))
                {
                    regionsList =
                        (from r in db.REGION where r.REGION_ABBR != "CEN" && r.REGION_ABBR == OOSUser.userRegion select new RegionData { Name = r.REGION_NAME, Abbreviation = r.REGION_ABBR })
                            .ToList();
                }
            }
            return Json(regionsList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Save(List<ItemCountRaw> data)
        {

            using (var db = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                foreach (var item in data)
                {
                    item.Save(db);
                }
            }

            return new JsonResult();
        }
        

        public JsonResult Data(string region)
        {

            var data = new List<ItemCountRaw>();
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                const string sql = @"

SELECT  STORE_NAME AS StoreName ,
        ID AS StoreId ,
        ISNULL([Grocery], 0) AS [Grocery] ,
        ISNULL([Whole Body], 0) AS [WholeBody]
FROM    ( SELECT    s.id ,
                    STORE_NAME ,
                    teamName ,
                    sku.numberOfSKUs
          FROM      store s
                    INNER JOIN dbo.STATUS st ON s.STATUS_ID = st.ID
                    INNER JOIN region r ON s.REGION_ID = r.ID
                    CROSS APPLY dbo.TEAM_Interim t
                    LEFT JOIN dbo.SKUCount sku ON sku.TEAM_ID = t.idTeam
                                                  AND sku.STORE_PS_BU = s.PS_BU
          WHERE     st.STATUS <> 'CLOSED'
                    AND r.REGION_ABBR = '{0}'
        ) p1 PIVOT ( MAX(p1.numberOfSKUs) FOR p1.teamName IN ( [Grocery], [Whole Body] ) ) AS p;
";
                data = context.ExecuteStoreQuery<ItemCountRaw>(string.Format(sql, region)).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
