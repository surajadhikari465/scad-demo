using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OOSCommon.DataContext;
using OutOfStock.Classes;

namespace OutOfStock.Controllers
{


    public class ServerInfo
    {
        public ServerInfo(string address)
        {
            Address = address;
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { if (value == "::1") _address = "localhost"; }
        }
    }
    public class UtilityController : Controller
    {
        //
        // GET: /Utility/

        public ActionResult Index()
        {

            var si = new ServerInfo(System.Net.Dns.GetHostEntry(Request.Url.Host).AddressList[0].ToString());
            

            return View("Index",si);
        }

        public JsonResult GetActiveRegions()
        {
            var data = new List<string>();
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                const string sql =
                    @"  select  distinct RR.Region_Abbr as Region 
                        from    REPORT_HEADER RH 
                            inner join Store SS on RH.STORE_ID = SS.ID 
                            inner join Region RR on SS.REGION_ID = RR.ID 
                        order by RR.Region_Abbr";
                data = context.ExecuteStoreQuery<string>(sql).ToList();

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTimeZoneOffsetData()
        {

            var data = new List<TimeZoneOffsetForRegion>();
            const string sql = @"SELECT ID AS RegionId ,
                                        REGION_ABBR AS RegionAbbreviation ,
                                        REGION_NAME AS RegionName ,
                                        TimeOffsetFromCentral AS TimezoneOffset
                                FROM    region
                                WHERE   IS_VISIBLE = 'true'";
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                data = context.ExecuteStoreQuery<TimeZoneOffsetForRegion>(sql).ToList();
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveTimezoneOffsetData(string data)
        {
            var list = new JavaScriptSerializer().Deserialize<List<TimeZoneOffsetForRegion>>(data);


            var sql = string.Empty;
            var result = new ResultObject();

            list.ForEach(i =>
            {
                sql += string.Format("update Region set TimeOffsetFromCentral = {0} where Id = {1}; ", i.TimezoneOffset, i.RegionId);
            });

            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                try
                {
                    
                    context.ExecuteStoreCommand(sql);

                    result.Result = "Success";
                    result.Message = "Timezone Offsets have been saved.";


                }
                catch (Exception ex)
                {
                    result.Result = "Error";
                    result.Message = ex.Message;
                }
            }


            return Json(result);
        }



        public JsonResult GetBadScans()
        {
            var data = new List<ReportHeaderMetaData>();
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                const string sql = @"select 
                                            id, 
                                            store_id, 
                                            CREATED_DATE, 
                                            LAST_UPDATED_DATE 
                                    from    Report_Header 
                                    where   (datediff(day, Created_Date, Last_Updated_Date)) > 1 
                                    order by id desc";
                
                data = context.ExecuteStoreQuery<ReportHeaderMetaData>(sql).ToList();

                foreach (var scan in data)
                {
                    var storeName = context.STORE.FirstOrDefault(x => x.ID == scan.Store_Id);
                    if (storeName == null) continue;
                    scan.StoreName = storeName.STORE_NAME;
                    scan.StoreAbbreviation = storeName.STORE_ABBREVIATION;
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
    }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FixTheseScans(int[] badScanIds)
        {
            var allScans = string.Join(",",  badScanIds.Select(x => x.ToString()));
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                var sqlCommand =
                    String.Format(@"update [REPORT_HEADER] 
                                    set [CREATED_DATE] = [LAST_UPDATED_DATE] 
                                    where id in ({0})", allScans);
                context.ExecuteStoreCommand(sqlCommand);
            }
            
            
            return Json(allScans);

}


        public JsonResult GetStoresForRegion(int regionId)
        {
            var data = new List<StoreList>();
            
                using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
                {
                    var sqlCommand =
                        String.Format("select distinct SS.ID, SS.STORE_NAME, MAX(RH.CREATED_DATE) as Scandate  from Store SS inner join Report_Header RH on SS.ID = RH.STORE_ID where SS.REGION_ID = {0} group by SS.Store_Name, SS.ID order by Ss.STORE_NAME",
                                      regionId);

                    data = context.ExecuteStoreQuery<StoreList>(sqlCommand).ToList();
                }
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult KnockOutStoresForRegion(string region)
        {
            var data = new List<StoreList>();

            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                var sqlCommand =
                    String.Format("select distinct SS.ID, SS.STORE_NAME, MAX(RH.CREATED_DATE) as Scandate  from Store SS inner join Report_Header RH on SS.ID = RH.STORE_ID inner join Region RR on SS.REGION_ID = RR.ID where RR.REGION_ABBR = '{0}' group by SS.Store_Name, SS.ID order by Ss.STORE_NAME",
                                  region);

                data = context.ExecuteStoreQuery<StoreList>(sqlCommand).ToList();
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetStoreScans(int storeId)
        {
            var data = new List<ScanCount>();
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                var sqlCommand = String.Format(
                    "select top 20 RH.ID, RH.Last_Updated_Date, COUNT(RD.ID) as ItemsScanned from Report_Header RH inner join REPORT_DETAIL RD on RH.ID = RD.REPORT_HEADER_ID where STORE_ID={0} group by RH.ID, RH.LAST_UPDATED_Date order by RH.LAST_UPDATED_DATE desc", storeId);

                data = context.ExecuteStoreQuery<ScanCount>(sqlCommand).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CompareScans(int reportOne, int reportTwo)
        {
            var data = new List<ReportCompare>();
            var condensed = new List<CondensedReport>();

            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                var sqlCommand = String.Format(
                    "select Report_Header_ID, UPC, Brand_Name, Long_Description, 'First' as CompareResult from REPORT_DETAIL where Report_Header_Id={0}", reportOne);
                data = context.ExecuteStoreQuery<ReportCompare>(sqlCommand).ToList();

                var secondList = new List<ReportCompare>();
                var secondSql = String.Format(
                    "select Report_Header_ID, UPC, Brand_Name, Long_Description, 'Second' as CompareResult from REPORT_DETAIL where Report_Header_Id={0}", reportTwo);
                secondList = context.ExecuteStoreQuery<ReportCompare>(secondSql).ToList();

                foreach (var scan in secondList)
                {
                    var firstEquivalent = data.FirstOrDefault(x => x.UPC == scan.UPC);
                    if (firstEquivalent != null)
                    {
                        firstEquivalent.CompareResult = "Both";
                    }
                    else
                    {
                        data.Add(scan);
                    }
                }

                

                var firstPicks = data.Count(x => x.CompareResult == "First");
                var secondString = data.Count(x => x.CompareResult == "Second");
                var bofOfThem = data.Count(x => x.CompareResult == "Both");

                condensed.Add(new CondensedReport{ Count = firstPicks, Description = "First"});
                condensed.Add(new CondensedReport{ Count = secondString, Description = "Second"});
                condensed.Add(new CondensedReport{ Count = bofOfThem, Description = "Both"});



            }
            return Json(condensed, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public int DeleteReport(int reportId)
        {
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                var sqlCommand = String.Format("update REPORT_HEADER set EXCLUDE_FLAG = 1 where ID={0}",reportId);

                var response = context.ExecuteStoreCommand(sqlCommand);
                //context.ExecuteStoreQuery<ScanCount>(sqlCommand).ToList();
                return response;
            }

        }

    }
}

