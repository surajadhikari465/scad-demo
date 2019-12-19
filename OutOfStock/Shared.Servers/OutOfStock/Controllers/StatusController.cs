using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using OOSCommon.DataContext;

namespace OutOfStock.Controllers
{
    public class StatusController : Controller
    {
        //
        // GET: /Status/

        public ActionResult Index()
        {
            return View();
        }



        public JsonResult deleteMultipleStatuses(List<int> ids)
        {

            var result = new Dictionary<string, object>();
            result["RecordsAffected"] = 0;
            result["hasError"] = false;

            try
            {
                using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
                {
                    var item = from s in context.ProductStatus where ids.Contains(s.id) select s;

                    if (item.Any())
                    {
                        context.ProductStatus.DeleteObject(item.FirstOrDefault());
                    }
                    result["RecordsAffected"] = context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result["hasError"] = true;
                result["ErrorMessage"] = ex.Message;
            }

            return Json(result);
        }


        public JsonResult deleteStatus(int id)
        {

            var result = new Dictionary<string, object>();
            result["RecordsAffected"] = 0;
            result["hasError"] = false;

            try
            {
                using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
                {
                    var item = from s in context.ProductStatus where s.id == id select s;

                    if (item.Count() == 1)
                    {
                        
                        context.ProductStatus.DeleteObject(item.FirstOrDefault());
                    }
                    result["RecordsAffected"] = context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result["hasError"] = true;
                result["ErrorMessage"] = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult getRecentStatues(string region)
        {

            var data = new List<ItemStatusInformation>();
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                // get last 15 status information
                var statuses = (from d in context.ProductStatus
                                orderby d.StartDate descending
                                select new ItemStatusInformation
                                    {
                                        Id=d.id,
                                        Region=d.Region,
                                        UPC=d.UPC, 
                                        UploadDate =d.StartDate,
                                        Status=d.ProductStatus,
                                        ExpirationDate = d.ExpirationDate
                                    })
                    .Take(15);


                var upcs = (from s in statuses select s.UPC).Distinct().ToList();


                // get vim data for results
                var vim = MvcApplication.vimRepository.GetItemMasterModel( upcs);

                // combine

                if (vim.Count < statuses.Count())
                {
                    foreach (var i in upcs)
                    {
                        if (!vim.ContainsKey(i))
                        {
                            vim.Add(i, null);
                        }
                    }
                }

                data = statuses.ToList();

                foreach (var item in data.Where(item => vim.ContainsKey(item.UPC)).Where(item => vim[item.UPC] != null))
                {
                    item.UOM  = vim[item.UPC].ITEM_UOM;
                    item.Size = vim[item.UPC].ITEM_SIZE;
                    item.Description = vim[item.UPC].LONG_DESCRIPTION;
                    item.Brand = vim[item.UPC].BRAND_NAME;
                }

                data.ForEach(x=> x.FixDate());

            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }

    

    public class  ItemStatusInformation
    {
        public int Id;
        public string Region;
        public string UPC;
        public string Brand;
        public string Size;
        public string UOM;
        public string ExpirationDateString;
        public string UploadDateString;
        public string Description;
        public string Status;
        public DateTime? ExpirationDate;
        public DateTime UploadDate;
        

        public ItemStatusInformation(int id, string region, string upc, string brand, string size, string uom, DateTime? expirationdate, DateTime uploaddate, string description, string status)
        {
            this.Id = id;
            this.Region = region;
            this.UPC = upc;
            this.Brand = brand;
            this.Size = size;
            this.UOM = uom;
            this.ExpirationDate = expirationdate;
            this.ExpirationDateString = ExpirationDate.HasValue ? "" : ExpirationDate.Value.ToShortDateString();
            this.UploadDate = uploaddate;
            this.UploadDateString = uploaddate.ToShortDateString();
            
            this.Description = description;
            this.Status = status;
        }

        public ItemStatusInformation()
        {
            // TODO: Complete member initialization
        }

        public void FixDate()
        {
            this.ExpirationDateString = ExpirationDate.HasValue ?  ExpirationDate.Value.ToShortDateString() : "";
            this.UploadDateString = UploadDate.ToShortDateString();
          
        }
    }
}
