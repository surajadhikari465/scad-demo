using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
//using NUnit.Framework.Constraints;
using OOS.Model;
using OOSCommon.DataContext;
using OutOfStock.Classes;
using OutOfStock.Models;
using OOSCommon;

namespace OutOfStock.Controllers
{

    public class StoreUpdateData
    {
        public string StoreAbbr { get; set; }
        public string StoreName { get; set; }
        public string StoreNumber { get; set; }
        public int RegionId { get; set; }
        public int StoreId { get; set; }
        public bool Hidden { get; set; }
        public string View { get; set; }
    }

    public class StoreUpdateResult
    {
        public bool isError { get; set; }
        public string message { get; set; }
    }

    public class AdminController : Controller
    {


        public static List<STORE> Updatelist;
        public static List<STORE> Insertlist;
        
        //
        // GET: /Admin/

        public ActionResult Index()
        {

            //var udp = OOSUser.EnableUDPLoggingForUser();
            ActionResult result = null;
            if (!OOSUser.HasValidLocationInformation)
            {
                result = View("~/Views/Shared/InvalidLocationInformation.cshtml");
            }
            else
            {
                result = View();
            }

            return result;
        }

        public ActionResult StoreUpdates()
        {
            return View();
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RenameStore(StoreUpdateData store)
        {
            StoreUpdateResult result = null;
            using (var db = new OOSEntities())
            {
                try
                {
                    var existingStore = (from s in db.STORE
                                         where
                                            s.PS_BU== store.StoreNumber
                                         select s).FirstOrDefault();

                    if (existingStore == null)
                    {
                        result = new StoreUpdateResult()
                        {
                            isError = true,
                            message = $"Cannot find a store with PSBU [{store.StoreNumber}]"
                        };
                    }
                    else
                    {
                        if (store.StoreAbbr != null)
                            existingStore.STORE_ABBREVIATION = store.StoreAbbr;
                        if (store.StoreName != null)
                            existingStore.STORE_NAME = store.StoreName;

                        bool changesMade = db.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified).Any();

                        if (changesMade)
                            db.SaveChanges();

                        result = new StoreUpdateResult()
                        {
                            isError = false,
                            message = ""
                        };

                        if (store.StoreAbbr == null && store.StoreName == null)
                            result.message = "No changes made.";
                        else if (store.StoreAbbr != null && store.StoreName == null)
                            result.message = "Abbreviation Updated";
                        else if (store.StoreAbbr == null && store.StoreName != null)
                            result.message = "Name Updated.";
                        else if (store.StoreAbbr != null && store.StoreName != null)
                            result.message = "Abbreviation and Name upated.";


                    }

                }
                catch (Exception ex)
                {
                    result = new StoreUpdateResult()
                    {
                        isError = true,
                        message = ex.Message
                    };

                    if (ex.InnerException != null)
                    {
                        result.message += $"\r\n{ex.InnerException.Message}";
                    }
                    
                } 
            }
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CloseStore(StoreCloseData store)
        {
            StoreUpdateResult result = null;
            using (var db = new OOSEntities())
            {
                try
                {
                    var existingStore = (from s in db.STORE
                                         where
                                             s.REGION_ID == store.RegionId &&
                                             s.PS_BU == store.PSBU
                                         select s).FirstOrDefault();

                    if (existingStore == null)
                    {
                        result = new StoreUpdateResult()
                        {
                            isError = true,
                            message = $"PSBU [{store.PSBU}] does not exist for the selected region."
                        };
                    }
                    else
                    {


                        switch (existingStore.STATUS_ID)
                        {
                            case 4:
                                existingStore.STATUS_ID = 3; // open
                                result = new StoreUpdateResult()
                                {
                                    isError = false,
                                    message = $"[{store.PSBU}] has been set to OPEN"
                                };
                                break;
                            case 3:
                                existingStore.STATUS_ID = 4; // closed
                                result = new StoreUpdateResult()
                                {
                                    isError = false,
                                    message = $"[{store.PSBU}] has been set to CLOSED"
                                };
                                break;                            
                        }

                        bool changesMade = db.ObjectStateManager.GetObjectStateEntries(EntityState.Added |EntityState.Deleted |EntityState.Modified).Any();

                        if (changesMade)
                            db.SaveChanges();

                    }


                }
                catch (Exception ex)
                {
                    result = new StoreUpdateResult()
                    {
                        isError = true,
                        message = ex.Message
                    };

                    if (ex.InnerException != null)
                    {
                        result.message += $"\r\n{ex.InnerException.Message}";
                    }
                    
                }
                
                return Json(result);

            }
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ToggleStoreVisibility(StoreUpdateData store)
        {
            
            var result = new StoreUpdateResult();

            using (var db = new OOSEntities())
            {
                try
                {
                    var storeToToggle = db.STORE.FirstOrDefault(s => s.PS_BU == store.StoreNumber);
                    if (storeToToggle != null)
                    {
                        storeToToggle.Hidden = !storeToToggle.Hidden;
                        db.SaveChanges();
                        var resultMessage = $"{storeToToggle.PS_BU} is now {(storeToToggle.Hidden ? "hidden" : "visible")}.";
                        result.isError = false;
                        result.message = resultMessage;

                    }
                    else
                    {
                        result.isError = true;
                        result.message = $"{store.StoreNumber} was not found.";
                    }
                    
                }
                catch (Exception ex)
                {
                    result.isError = true;
                    result.message = ex.Message;
                    return Json(result);
                }
            }
           
            
     
            return Json(result); 
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddStore(StoreUpdateData store)
        {
            // does store exist?
            using (var db = new OOSEntities())
            {
                StoreUpdateResult result;
                try
                {


                    var existingStore = (from s in db.STORE
                        where
                            s.REGION_ID == store.RegionId &&
                            s.PS_BU == store.StoreNumber
                        select new { s.PS_BU, s.STORE_NAME, s.STORE_ABBREVIATION, s.REGION.REGION_ABBR}).FirstOrDefault();

                    if (existingStore != null)
                    {
                        // store already existed. error.
                        result = new StoreUpdateResult()
                        {
                            isError = true,
                            message =
                                $"{existingStore.PS_BU} [{existingStore.STORE_NAME}] already exists in the database for region {existingStore.REGION_ABBR}"
                        };
                        
                    }
                    else
                    {
                        // not in there. lets add a new one. 

                        var newStore = new STORE
                        {
                            REGION_ID = store.RegionId,
                            STORE_ABBREVIATION = store.StoreAbbr,
                            STORE_NAME = store.StoreName,
                            CREATED_BY = "system",
                            CREATED_DATE = DateTime.Now,
                            LAST_UPDATED_BY = "system",
                            STATUS_ID = 3,
                            LAST_UPDATED_DATE = DateTime.Now,
                            PS_BU = store.StoreNumber
                            
                        };

                        db.STORE.AddObject(newStore);
                        db.SaveChanges();

                        result = new StoreUpdateResult()
                        {
                            isError = false,
                            message = $"{newStore.PS_BU} [{newStore.STORE_NAME}] added."
                        };
                        

                    }

                }
                catch (Exception ex)
                {
                    result = new StoreUpdateResult()
                    {
                        isError = true,
                        message = ex.Message
                    };

                    if (ex.InnerException != null)
                    {
                        result.message += $"\r\n{ex.InnerException.Message}";
                    }
                    
                }

                return Json(result);

            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveStore(StoreUpdateData data)
        {

            using (var db = new OOSEntities())
            {


                switch (data.View)
                {
                    case "updates":
                        var item = (from s in db.STORE where s.PS_BU == data.StoreNumber select s).FirstOrDefault();
                        if (item != null)
                        {
                            item.STORE_NAME = data.StoreName;
                            
                        }
                        db.SaveChanges();
                        break;
                    case "new":
                        var newstore = new STORE()
                        {
                            CREATED_BY = OOSUser.GetUserName(),
                            CREATED_DATE = DateTime.Now,
                            LAST_UPDATED_DATE = DateTime.Now,
                            STORE_NAME = data.StoreName,
                            PS_BU = data.StoreNumber,
                            STORE_ABBREVIATION = data.StoreAbbr,
                            STATUS_ID =  3,
                            REGION_ID =  data.RegionId
                            
                        };
                        db.STORE.AddObject(newstore);
                        db.SaveChanges();

                        break;
                }
            }

            return Json(data.StoreName, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetRegionList()
        {
            return Json(GetRegions(), JsonRequestBehavior.AllowGet);
        }

        protected IEnumerable<SelectListItem> GetRegions()
        {
            using (var dbContext = new OOSEntities())
            {
                IQueryable<REGION> storeQuery = dbContext.REGION
                .OrderBy(m => m.REGION_ABBR);
                return storeQuery.ToList().Select(region => new SelectListItem() { Text = $"{region.REGION_ABBR}", Value = region.ID.ToString(CultureInfo.InvariantCulture) }).ToList();
            }
        }


        [HttpGet]
        public JsonResult GetRecentScansByRegion(int regionid, string regionabbr)
        {
            var recent = new List<RecentScan>();

            using (var db = new OOSEntities())
            {
                 recent = (from h in db.REPORT_HEADER
                    //  join d in db.REPORT_DETAIL on h.ID equals d.REPORT_HEADER_ID
                    //  join s in db.STORE on h.STORE_ID equals s.ID
                    //join r in db.REGION on s.REGION_ID equals r.ID

                    where h.STORE.REGION.REGION_ABBR == regionabbr
                    orderby h.ID descending
                    select new RecentScan()
                    {
                        Id=h.ID,
                        ItemCount = h.REPORT_DETAIL.Count(),
                        StoreAbbr = h.STORE.STORE_ABBREVIATION,
                        OffsetCreatedDate  = h.OffsetCorrectedCreateDate.Value
                    }).Take(30).ToList();
            }
            return Json(recent, JsonRequestBehavior.AllowGet);
        }

        public static SageCall GetStoresByRegion(string Region)
        {
            var tokenResponse = "";

            var publicKey = "pGsFJU9kYGeOrSLsRZfw7ekFsreZ6u72";
            var secret = "hIuu47QQUBwGWFtr";
            var url = "https://api.wholefoodsmarket.com/oauth20/";

            var tokenURL =
                $"{url}token?client_id={publicKey}&grant_type=client_credentials&client_secret={secret}&response_content_type=application/json";

            var tokenRequest = (HttpWebRequest) WebRequest.Create(tokenURL);
            try
            {
                var response = tokenRequest.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);

                    XElement xToken = XElement.Parse(reader.ReadToEnd());
                    if (xToken != null)
                    {
                        tokenResponse = xToken.Element("access_token").Element("token").Value;
                    }
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }



            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timeRangeUnix = (Convert.ToInt64(((DateTime.Now).AddDays(-365) - epoch).TotalSeconds)).ToString(CultureInfo.InvariantCulture);
            var storeURL = "https://api.wholefoodsmarket.com/v1/stores/search/region/";
            var queryURL = $"{storeURL}{Region}?access_token={tokenResponse}&limit=1000";

            var request = (HttpWebRequest)WebRequest.Create(queryURL);
            try
            {
                var response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    return System.Web.Helpers.Json.Decode<SageCall>(reader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }

        }



        [Obsolete("Do not use this anymore. Sage authentication has changed.", true)]
        public JsonResult CompareStoresByRegion(int regionid, string regionabbr)
        {
            var result = new JsonResult();

            var updateCounter = 0;
            var insertCounter = 0;

            if (Updatelist != null)
            {
                Updatelist.Clear();
            }
            else
            {
                Updatelist = new List<STORE>();
            }

            if (Insertlist != null)
            {
                Insertlist.Clear();
            }
            else
            {
                Insertlist = new List<STORE>();
            }

            var returnList = new List<KeyValuePair<string, int>>();

            using (var db = new OOSEntities())
            {
                var oosStoresByRegion = db.STORE.Where(s => s.REGION_ID == regionid ).ToList();

                var sageCall = GetStoresByRegion(regionabbr);
                var sageList = new List<STORE>();

                foreach (var store in sageCall.Stores)
                {
                    if ((store.facility != "Whole Foods Market") || store.bu == "0") continue; // exclude regional offices
                    var newSageStore = new STORE
                    {
                        PS_BU = store.bu,
                        STORE_NAME = store.name,
                        
                        //status
                        STORE_ABBREVIATION = store.tlc,
                        LAST_UPDATED_DATE = DateTime.Now,
                        CREATED_DATE = DateTime.Now,
                        LAST_UPDATED_BY = "sage",
                        CREATED_BY = "sage",
                    };

                    newSageStore.REGION_ID =
                        (from r in db.REGION where r.REGION_ABBR == store.region select r.ID).FirstOrDefault();

                    newSageStore.REGION =
                        (from r in db.REGION where r.ID == newSageStore.REGION_ID select r).FirstOrDefault();

                    newSageStore.STATUS_ID =
                        (from s in db.STATUS where s.STATUS1 == store.status select s.ID).FirstOrDefault();

                    if (newSageStore.STATUS_ID == null)
                        newSageStore.STATUS_ID =
                            (from s in db.STATUS where s.STATUS1 == "OPEN" select s.ID).FirstOrDefault();

                    sageList.Add(newSageStore);
                }

                foreach (var store in sageList)
                {

                    var dbStore = oosStoresByRegion.FirstOrDefault(x => x.STORE_ABBREVIATION == store.STORE_ABBREVIATION);
                    if (dbStore == null)
                    {
                        Insertlist.Add(store);
                        insertCounter++;
                    }
                    else
                    {
                        var compareObjects = CompareStores(dbStore, store);
                        if (compareObjects.Length > 0)
                        {
                           // QA.QuiverLog.Trace(dbStore.TLA + " - " + compareObjects);
                            Updatelist.Add(store);
                            updateCounter++;
                        }

                    }
                }

                returnList.Add(new KeyValuePair<string, int>("update", updateCounter));
                returnList.Add(new KeyValuePair<string, int>("insert", insertCounter));
                returnList.Add(new KeyValuePair<string, int>("current", oosStoresByRegion.Count()));

                result = Json(returnList, JsonRequestBehavior.AllowGet);

            }



            return result;

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAvailableStores(int regionid)
        {
            var result = new JsonResult();
            if (Updatelist == null)
            {
                Updatelist = new List<STORE> { new STORE { STORE_ABBREVIATION = "XXX" } };
            }

            using (var db = new OOSEntities())
            {
                var storeList = (from stores in db.STORE where stores.REGION_ID == regionid
                                 select new
                                 {
                                     StoreAbbr = stores.STORE_ABBREVIATION,
                                     StoreName = stores.STORE_NAME,
                                     StoreNumber = stores.PS_BU,
                                     RegionId = stores.REGION_ID,
                                     StatusId = stores.STATUS_ID,
                                     Region= stores.REGION.ID,
                                     Status = stores.STATUS.STATUS1
                                 }).ToList();

                var finalStore = (from stores in storeList
                                  join upp in Updatelist on stores.StoreName equals upp.STORE_NAME into su
                                  from subsub in su.DefaultIfEmpty()
                                  select new
                                  {
                                      StoreAbbr = stores.StoreAbbr,
                                      StoreName = stores.StoreName,
                                      StoreNumber = stores.StoreNumber,
                                      Region = stores.Region,
                                      RegionId=stores.RegionId,
                                      StatusId=stores.StatusId,
                                      Status = stores.Status,
                                      NeedsUpdate = (subsub == null ? false : true)
                                  }).OrderBy(s => s.StoreName).ToList();

                result = Json(finalStore, JsonRequestBehavior.AllowGet);
                return result;

            }



        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetNewStores()
        {
            var result = new JsonResult();

            var insertingList = (from up in Insertlist
                                 select new
                                 {
                                     StoreAbbr = up.STORE_ABBREVIATION,
                                     StoreName = up.STORE_NAME,
                                     StoreNumber = up.PS_BU,
                                     //Region = up.REGION,
                                     RegionId = up.REGION_ID
                                 }).OrderBy(s => s.StoreName).ToList();

            result = Json(insertingList, JsonRequestBehavior.AllowGet);
            return result;
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetUpdatedStores()
        {
            var result = new JsonResult();

            var insertingList = (from up in Updatelist
                                 select new
                                 {
                                     StoreAbbr = up.STORE_ABBREVIATION,
                                     StoreName = up.STORE_NAME,
                                     StoreNumber = up.PS_BU,
                                     //Region = up.REGION,
                                     RegionId = up.REGION_ID,
                                     
                                 }).OrderBy(s => s.StoreName).ToList();

            result = Json(insertingList, JsonRequestBehavior.AllowGet);
            return result;
        }

           public string CompareStores(STORE currentStore, STORE sageStore)
        {
            var result = "";

            if (currentStore.STORE_NAME != sageStore.STORE_NAME)
                result += "Store Name, ";
            if (currentStore.PS_BU != sageStore.PS_BU)
                result += "Store Number, ";
            if (currentStore.REGION_ID != sageStore.REGION_ID)
                result += "Region, ";
            //if (currentStore.Address != sageStore.Address)
            //    result += "Address, ";
            //if (currentStore.City != sageStore.City)
            //    result += "City, ";
            //if (currentStore.Zip_Code != sageStore.Zip_Code)
            //    result += "Zip Code, ";
            //if (currentStore.Phone != sageStore.Phone)
            //    result += "Phone, ";
            //if (currentStore.Latitude != sageStore.Latitude)
            //    result += "Latitude, ";
            //if (currentStore.Longitude != sageStore.Longitude)
            //    result += "Longitude, ";
            //if (currentStore.Status != sageStore.Status && currentStore.TLA != "CEN")
            //if (currentStore.STATUS_ID != sageStore.STATUS_ID)
            //    result += "Status, ";

            if (result.Length > 0)
            {
                result = result.Substring(0, (result.Length - 2));
            }

            return result;
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


    public class RecentScan
    {
        public int Id { get; set; }
        public int ItemCount { get; set; }
        public string StoreAbbr { get; set; }
        public DateTime OffsetCreatedDate { get; set; }
    }
    public class StoreCloseData
    {
        public int StoreId { get; set; }
        public int RegionId { get; set; }   
        public string PSBU { get; set; }
    }
}
