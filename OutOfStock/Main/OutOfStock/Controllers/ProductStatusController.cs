using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OOS.Model;
using OOSCommon.DataContext;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;
using Telerik.Web.Mvc.UI;
using OutOfStock.Models;

namespace OutOfStock.Controllers
{
    public class ProductStatusController : Controller
    {
        private const string sessionIxProductStatusController = "ProductStatusController";

        private UPCEditViewModel viewModel { get; set; }

        private string currentProductStatusToFind
        {
            get { return (string) Session[sessionIxProductStatusController + ".currentProductStatus"] ?? string.Empty; }
            set { Session[sessionIxProductStatusController + ".currentProductStatus"] = value; }
        }

        private OOSCommon.DataContext.IOOSEntities ef
        {
            get {
                return _ef ??
                       (_ef = new OOSCommon.DataContext.OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString));
            }
            set { _ef = value; }
        }

        private OOSCommon.DataContext.IOOSEntities _ef = null;

        private List<string> productStatuses
        {
            get
            {
                if (_productStatuses == null)
                {
                    //_productStatuses = ef.KNOWN_OOS_DETAIL
                    //    .Where(k => !string.IsNullOrEmpty(k.ProductStatus))
                    //    .OrderBy(k => k.ProductStatus)
                    //    .Select(k => k.ProductStatus)
                    //    .Distinct()
                    //    .ToList();

                    _productStatuses =
                        ef.ProductStatus.Where(s => !string.IsNullOrEmpty(s.ProductStatus))
                          .OrderBy(s => s.ProductStatus)
                          .Select(s => s.ProductStatus)
                          .Distinct()
                          .ToList();

                }
                return _productStatuses;
            }
            set { _productStatuses = value; }
        }

        private List<string> _productStatuses = null;


        // GET: /ProductStatus/
        public ActionResult Index()
        {
            //OutOfStock.MvcApplication.oosLog.Trace("Enter");
            ActionResult result = null;
            if (!OOSUser.isRegionalBuyer)
                result = new RedirectResult("~/Home/Index");
            else
            {
                ViewBag.productStatuses = productStatuses;
                ViewBag.currentProductStatus = currentProductStatusToFind;
                ViewBag.SelectedRegion = OOSUser.userRegion;

                using (var db = new OOSEntities())
                {

                    var regionsList = (from r in db.REGION
                                      where r.IS_VISIBLE == "true"
                                      orderby r.REGION_NAME ascending 
                                      select new {r.REGION_NAME, r.REGION_ABBR}).ToList();

                    if (OOSUser.isCentral)
                    {
                        ViewBag.SelectedRegion = OOSUser.userRegion;
                        regionsList.Insert(0, new {REGION_NAME = "Select Region", REGION_ABBR = ""});
                    }


                    ViewBag.Regions = regionsList;
                }


                result = View();
            }
            // OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
        }

        //
        // POST: /ProductStatus/Search
        public ActionResult Search(string productStatusToFind)
        {

            ActionResult result = null;
            if (!OOSUser.isRegionalBuyer)
                result = new RedirectResult("~/Home/Index");
            else
            {
                ViewBag.productStatuses = productStatuses;
                ViewBag.currentProductStatus = currentProductStatusToFind;
                string errorMessage = string.Empty;

                //if (string.IsNullOrWhiteSpace(productStatusToFind))
                //{
                //    currentProductStatusToFind = string.Empty;
                //    ViewBag.currentProductStatus = string.Empty;

                //    ViewBag.Error = "Please enter a product status to find";
                //    result = View("Index");
                //}
                //else

                currentProductStatusToFind = productStatusToFind;
                ViewBag.currentProductStatus = productStatusToFind;

                // get unique UPCs from Known_OOS_Detail that arent NOT deleted and match product status. 
                // pair up with Item data from VIM. 
                viewModel = SearchByProductStatus(productStatusToFind);
                //RegionalBuyerViewModel.SearchByProductStatus(productStatusToFind);
                result = View("Index", viewModel);

            }
            //OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
        }
        
        public List<ProductStatus> SearchByOOSValues(string upc, string status, DateTime? expirationdate, string region, int? maxrecords)
        {
            IQueryable<ProductStatus> results = null;
            List<ProductStatus> returnvalue = null;

            using (var db = new OOSEntities())
            {
                results = (from s in db.ProductStatus
                           where s.Region == region 
                           select new OOS.Model.ProductStatus
                               {
                                   Id = s.id,
                                   Upc = s.UPC,
                                   Status = s.ProductStatus,
                                   ExpirationDate = s.ExpirationDate,
                                   StartDate = s.StartDate,
                                   Region = s.Region,
                               });

                if (!string.IsNullOrEmpty(upc))
                {
                    results = results.Where(r => r.Upc.Contains(upc));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    results = results.Where(r => r.Status.Contains(status));
                }

                if (expirationdate != null)
                {
                    results = results.Where(r => r.ExpirationDate == expirationdate);
                }

                if (maxrecords != null)
                {
                    if (maxrecords > 0)
                    {
                        results = results.OrderByDescending(r => r.StartDate).Take((int)maxrecords);

                    }
                }

                results=results.OrderByDescending(r => r.StartDate);

                returnvalue = results.ToList();
            }

            return returnvalue;
        }

        public List<UPCEditViewModel.ProductStatusWithVIMDetails> CombineWithVIMInfo(List<ProductStatus> OOSItems)
        {
            IEnumerable<UPCEditViewModel.ProductStatusWithVIMDetails> CombinedItems;

            using (var db = new OOSEntities())
            {
                var UPCs = (from oos in OOSItems select oos.Upc).Distinct().OrderBy(oos => oos).ToList();
                var VIMItems = MvcApplication.vimRepository.GetItemMasterModel(UPCs);

                if (VIMItems.Count < UPCs.Count())
                {
                    foreach (var upc in UPCs.Where(upc => !VIMItems.ContainsKey(upc)))
                    {
                        VIMItems.Add(upc, null);
                    }
                }

              CombinedItems  = (from s in OOSItems
                                     join v in VIMItems on s.Upc equals v.Key into result
                                     from v in result.DefaultIfEmpty()
                                     select
                                         new UPCEditViewModel.ProductStatusWithVIMDetails(s.Id,
                                                                                          s.Upc,
                                                                                          s.Region,
                                                                                          s.Status,
                                                                                          s.StartDate,
                                                                                          s.ExpirationDate,
                                                                                          (v.Value == null
                                                                                               ? string.Empty
                                                                                               : v.Value.BRAND),
                                                                                          (v.Value == null
                                                                                               ? string.Empty
                                                                                               : v.Value
                                                                                                  .LONG_DESCRIPTION),
                                                                                          (v.Value == null
                                                                                               ? string.Empty
                                                                                               : v.Value.ITEM_SIZE),
                                                                                          (v.Value == null
                                                                                               ? string.Empty
                                                                                               : v.Value.ITEM_UOM)));

            }

            return CombinedItems.ToList();

        }

        public UPCEditViewModel SearchByProductStatus(string StatusSearch)
        {
            var resultModel = new UPCEditViewModel();
            using (var db = new OOSEntities())
            {
                IEnumerable<OOS.Model.ProductStatus> statusResults;

                if (!string.IsNullOrWhiteSpace(StatusSearch))
                {
                    statusResults = (from s in db.ProductStatus
                                     where s.ProductStatus.Contains(StatusSearch)
                                     select new OOS.Model.ProductStatus
                                         {
                                             Id = s.id,
                                             Upc = s.UPC,
                                             Status = s.ProductStatus,
                                             ExpirationDate = s.ExpirationDate,
                                             StartDate = s.StartDate,
                                             Region = s.Region,
                                         });
                }
                else
                {
                    statusResults = (from s in db.ProductStatus
                                     where s.ProductStatus.Equals(StatusSearch)
                                     select new OOS.Model.ProductStatus
                                         {
                                             Id = s.id,
                                             Upc = s.UPC,
                                             Status = s.ProductStatus,
                                             ExpirationDate = s.ExpirationDate,
                                             StartDate = s.StartDate,
                                             Region = s.Region,
                                         });
                }


                var UPCs = (from s in statusResults select s.Upc).Distinct().OrderBy(s => s).ToList();

                var vim = MvcApplication.vimRepository.GetItemMasterModel(UPCs);

                resultModel.UPCs = UPCs;
                resultModel.ProductStatuses = string.IsNullOrWhiteSpace(StatusSearch)
                                                  ? statusResults.Take(100).ToList()
                                                  : statusResults.ToList();
                // limit to 100 results if empty search string.
                resultModel.VimDetails = vim;

                // if vim didnt return results for each UPC. add blank vim objects for missing UPCs so we can do a left join.
                if (vim.Count < UPCs.Count())
                {
                    foreach (var upc in UPCs)
                    {
                        if (!vim.ContainsKey(upc))
                        {
                            vim.Add(upc, null);
                        }
                    }
                }
                var q = (from s in statusResults
                         join v in vim on s.Upc equals v.Key into result
                         from v in result.DefaultIfEmpty()
                         select
                             new UPCEditViewModel.ProductStatusWithVIMDetails(s.Id,
                                                                              s.Upc,
                                                                              s.Region,
                                                                              s.Status,
                                                                              s.StartDate,
                                                                              s.ExpirationDate,
                                                                              (v.Value == null
                                                                                   ? string.Empty
                                                                                   : v.Value.BRAND),
                                                                              (v.Value == null
                                                                                   ? string.Empty
                                                                                   : v.Value.LONG_DESCRIPTION),
                                                                              (v.Value == null
                                                                                   ? string.Empty
                                                                                   : v.Value.ITEM_SIZE),
                                                                              (v.Value == null
                                                                                   ? string.Empty
                                                                                   : v.Value.ITEM_UOM)));

                resultModel.CombinedResults = q.ToList();
                return resultModel;
            }
        }

        public UPCEditViewModel SearchByUPC(string upc)
        {

            List<ProductStatus> statuses;
            var upcs = new List<string>();
            Dictionary<string, OOSCommon.VIM.ItemMasterModel> vim;
            List<UPCEditViewModel.ProductStatusWithVIMDetails> combined;
            //KeyValuePair<ProductStatus, OOSCommon.VIM.ItemMasterModel> combined;

            using (var db = new OOSEntities())
            {
                DateTime querydate = DateTime.Now;
                var results = from ps in db.ProductStatus
                              where ps.UPC.Contains(upc)
                              select new ProductStatus
                              {
                                  Id = ps.id,
                                  Upc = ps.UPC,
                                  Status = ps.ProductStatus,
                                  ExpirationDate = ps.ExpirationDate,
                                  StartDate = ps.StartDate,
                                  Region = ps.Region,
                              };
                statuses = results.ToList();

                if (statuses.Count > 0)
                {
                    upcs = (from p in statuses select p.Upc).Distinct().ToList();
                }
                else
                {
                    upcs.Add(upc);
                }

                vim = MvcApplication.vimRepository.GetItemMasterModel(upcs);

                // if vim didnt return results for each UPC. add blank vim objects for missing UPCs so we can do a left join.
                if (vim.Count < upcs.Count())
                {
                    foreach (var i in upcs)
                    {
                        if (!vim.ContainsKey(i))
                        {
                            vim.Add(i, null);
                        }
                    }
                }


                var q = (from ps in statuses
                         join v in vim on ps.Upc equals v.Key into result
                         from v in result.DefaultIfEmpty()

                         select
                             new UPCEditViewModel.ProductStatusWithVIMDetails(ps.Id,
                                                                                ps.Upc,
                                                                                ps.Region,
                                                                                ps.Status,
                                                                                ps.StartDate,
                                                                                ps.ExpirationDate,
                                                                                (v.Value == null ? string.Empty : v.Value.BRAND),
                                                                                (v.Value == null ? string.Empty : v.Value.LONG_DESCRIPTION),
                                                                                (v.Value == null ? string.Empty : v.Value.ITEM_SIZE),
                                                                                (v.Value == null ? string.Empty : v.Value.ITEM_UOM)));


                combined = q.ToList();

            }
            return new UPCEditViewModel(statuses, upcs, vim, combined);
        }


        [HttpPost]
        [GridAction]
        public ActionResult BulkEdit(string gridSelectedItems, string bulkProductStatus,
                                     DateTime? bulkExpirationDatePicker)
        {
            OutOfStock.MvcApplication.oosLog.Trace(string.Format("Bulk Edit Ids: {0}", gridSelectedItems));
            ActionResult result = null;
            if (!OOSUser.isRegionalBuyer)
                result = new RedirectResult("~/Home/Index");
            else
            {
                var updateCount = 0;
                var resultDictionary = new Dictionary<string, int>();
                // If there is any change requested 
                if (!string.IsNullOrWhiteSpace(gridSelectedItems) || bulkExpirationDatePicker.HasValue)
                {
                    // ... and to which id's are the changes to be applied?
                    var idsAsText = new string[] {};
                    if (!string.IsNullOrWhiteSpace(gridSelectedItems))
                        idsAsText = gridSelectedItems.Split(new char[] {','});
                    // Apply changes
                    var errorMessage = string.Empty;
                    foreach (var idParts in idsAsText.Select(x => x.Split('|')))
                    {
                        int currentId;
                        errorMessage = string.Empty;

                        if (!int.TryParse(idParts[0], out currentId)) currentId = 0;
                        var currentUPC = idParts[1];
                        if (currentId == 0 || currentUPC == string.Empty) continue; // skip to next item.

                        var currentItem = new UPCEditViewModel.ProductStatusWithVIMDetails(currentId, currentUPC,bulkProductStatus,bulkExpirationDatePicker);

                        //if (RegionalBuyerViewModel.UpdateKnowOOSDetail(id, region, vendorKey, bulkProductStatus,bulk  ExpirationDatePicker, out errorMessage))
                        if (currentItem.Save(out errorMessage))
                            ++updateCount;
                        else if (resultDictionary.Keys.Contains(errorMessage))
                            ++resultDictionary[errorMessage];
                        else
                            resultDictionary.Add(errorMessage, 1);
                    }
                }
                // Now communicate what has happened

                var summary = string.Empty;
                if (resultDictionary.Count == 0 && updateCount > 0)
                {
                    ViewBag.Error = string.Format("{0} item(s) updated.", updateCount);
                }
                else
                    if (resultDictionary.Count == 0 && updateCount == 0)
                    {
                        const string msg = @"<div>You must select items to apply Status and Expiration changes to. </br><label>Example:</label><p><img src='/Content/images/itemselectionexample.png' alt='item selection example' /> </p> </div>";
                        ViewBag.Error = msg;
                    }

                    else
                    {
                        foreach (var kvp in resultDictionary)
                        {
                            if (summary.Length > 0)
                                summary += ", ";
                            summary += kvp.Key + " " + kvp.Value;
                        }
                        if (summary.Length > 0) summary += ", ";

                        if (summary.Length == 0)
                        {
                            ViewBag.Error = string.Format("Updated {0} items successfully.", updateCount);
                        }
                        else
                        {
                            ViewBag.Error = summary;
                        }

                    }
                result = GridSelect(bulkProductStatus, string.Empty);
            }
            return result;
        }

        [GridAction]
        public ActionResult GridSelect(string id, string upc)
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(upc) )
            {
                currentProductStatusToFind = id;
            }
            ViewBag.productStatuses = productStatuses;
            ViewBag.currentProductStatus = currentProductStatusToFind;
            viewModel = SearchByProductStatus(currentProductStatusToFind);
            //viewModel = GetData(currentProductStatusToFind);
            return View("Index", viewModel);
        }

        [GridAction (EnableCustomBinding = true)]
        public ActionResult GridSelectAjax(bool Filtered, string filterupc, string filterstatus, string filterexpiration , string filterregion)
        {
            System.Threading.Thread.Sleep(1000);

            if (filterupc == "null") filterupc = null;
            if (filterstatus == "null") filterstatus = null;
            
            //if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(upc))
            //{
            //    currentProductStatusToFind = id;
            //}

            var searchResults = new List<UPCEditViewModel.ProductStatusWithVIMDetails>();

            if (!Filtered)
            {
                // no filter criteria, show 5 most recent
                var results = SearchByOOSValues(null, null, null, filterregion, 15);
                var combined = CombineWithVIMInfo(results.ToList());
                searchResults = combined.ToList();

            }
            else
            {
                DateTime? expdate = null;
                DateTime tempvalue;

                if (DateTime.TryParse(filterexpiration, out tempvalue))
                {
                    expdate = tempvalue;
                };
                    var results = SearchByOOSValues(filterupc, filterstatus, expdate ,filterregion,  null);
                   var combined = CombineWithVIMInfo(results.ToList());
                    searchResults = combined.ToList();
            }
            
            ViewBag.productStatuses = productStatuses;
            ViewBag.currentProductStatus = currentProductStatusToFind;
            //viewModel = SearchByProductStatus(currentProductStatusToFind);            
            //viewModel = GetData(currentProductStatusToFind);

            HttpContext.Session["GridResults"] = searchResults;
            var returnvalues = new GridModel<UPCEditViewModel.ProductStatusWithVIMDetails>(searchResults);
            return View(returnvalues);

        }

       

        [GridAction]
        //public ActionResult GridUpdate(RegionalBuyerViewModel itemValue) 
        public ActionResult GridUpdate(string UPC, int id, DateTime? ExpirationDate, string Status, GridEditMode? mode)
        {
            if (!OOSUser.isRegionalBuyer)
                return new RedirectResult("~/Home/Index");
            //var id = (itemValue == null ? string.Empty : itemValue.ID);
            var item = new UPCEditViewModel.ProductStatusWithVIMDetails(id, UPC, Status, ExpirationDate);
            if (UpdateData(item)) //UpdateData is to undate items in grid
            {
                RouteValueDictionary routeValues = this.GridRouteValues();
                return RedirectToAction("GridSelect", routeValues);
            }
            return GridSelect(id.ToString(), UPC);
        }

        [GridAction]
        public ActionResult GridUpdateAjax(UPCEditViewModel.ProductStatusWithVIMDetails updatedItem)
        {
            var griddata = (List<UPCEditViewModel.ProductStatusWithVIMDetails>) HttpContext.Session["GridResults"];

            var item = (from i in griddata where i.Id == updatedItem.Id select i).FirstOrDefault();

            if (item != null)
            {
                
                if (!string.IsNullOrEmpty(updatedItem.Status))
                    item.Status = updatedItem.Status;
                if (updatedItem.ExpirationDate != null)
                    item.ExpirationDate = updatedItem.ExpirationDate;
                if (UpdateData(item))
                {
                    (from g in griddata where g.Id == item.Id select g).ToList().ForEach(i => i.Status = item.Status);
                }
            }

            return View(new GridModel<UPCEditViewModel.ProductStatusWithVIMDetails>(griddata));

        }

        public JsonResult IsValidUPC(string upc)
        {
            var data = new isValidUPCResponse();

            if (upc.Length < 13)
            {
                upc = upc.PadLeft(13, '0');
            }
             data.VimInfo  = MvcApplication.vimRepository.GetItemMasterModel(upc);

             using (var db = new OOSEntities())
             {
                 data.ExistsInOOS = (from i in db.ProductStatus where i.UPC == upc select i).Any();
             }

            return Json(data);
        }

        public JsonResult AddNew(string status, DateTime? expiration, string upc, string region)
        {
            var vimInfo =  MvcApplication.vimRepository.GetItemMasterModel(upc);


            if (region == "CE")
                region = "NC";
            
            var result = false;
            var errorMessage = string.Empty;

            const string sql = "exec SaveProductStatus '{0}', '{1}', {2}, '{3}';\r\n";
            try
            {
                using (var datacontext = new OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString))
                {
                    var query = string.Format(sql,
                            region,
                            upc,
                            expiration.HasValue ?  "'" + expiration.Value.ToShortDateString() + "'" : "null",
                            status.Replace("'", "''")
                            );
                    
                    datacontext.ExecuteStoreCommand(query, null);

                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            var returnvalue = new JsonResult {Data = Json(result)};
            return returnvalue;
        }
        public JsonResult BulkDelete(List<string> Ids)
        {
            var result = new JsonResult();
            var recordsRemoved = 0;
            var griddata = (List<UPCEditViewModel.ProductStatusWithVIMDetails>)HttpContext.Session["GridResults"];
            if (Ids != null)
            {
                Ids.ForEach(
                    id => (from g in griddata where g.Id == int.Parse(id.Split('|')[0]) select g).ToList().ForEach(i =>
                        {
                            if (DeleteData(i))
                            {
                                recordsRemoved++;
                            }
                        }));
            }
            result = Json(recordsRemoved);
            return result;
        }
        public JsonResult BulkSave(string status, DateTime? expiration, List<string> Ids)
        {
            var recordsSaved = 0;
            var result = new JsonResult();
            var griddata = (List<UPCEditViewModel.ProductStatusWithVIMDetails>)HttpContext.Session["GridResults"];
            Ids.ForEach(id => (from g in griddata where g.Id == int.Parse(id.Split('|')[0]) select g).ToList().ForEach(i =>
                {
                    if (status != "") i.Status = status;
                    if (expiration != null) i.ExpirationDate = expiration;
                    if (UpdateData(i))
                    {
                        recordsSaved++;
                    }
                }));
            result = Json(recordsSaved);


            return result;

        }

        [GridAction]
        //public ActionResult GridDelete(int? id) // to delete items from grid
        public ActionResult GridDelete(string UPC, int id, DateTime? ExpirationDate, string Status, GridEditMode? mode)
        {
            var errorMessage = string.Empty;
            if (!OOSUser.isRegionalBuyer)
                return new RedirectResult("~/Home/Index");
            if (id == 0)
                errorMessage = "No item selected.";
            else
            {
                var item = new UPCEditViewModel.ProductStatusWithVIMDetails(id,UPC, Status,ExpirationDate);
                item.Delete(out errorMessage);
            }

            return GridSelect(string.Empty, string.Empty);
        }

        [GridAction]
        public ActionResult GridDeleteAjax(UPCEditViewModel.ProductStatusWithVIMDetails deletedItem)
        {
            var griddata = (List<UPCEditViewModel.ProductStatusWithVIMDetails>)HttpContext.Session["GridResults"];
            //var displayedItemToDelete = (from i in griddata where i.Id == deletedItem.Id select i).FirstOrDefault();
            var errorMessage = string.Empty;
            if (!OOSUser.isRegionalBuyer)
                return new RedirectResult("~/Home/Index");
            if (deletedItem == null)
                errorMessage = "No item selected.";
            else
            {
                if (!deletedItem.Delete(out errorMessage))
                {
                    throw new Exception(errorMessage);    
                }
                else
                {
                    griddata.RemoveAll(g => g.Id == deletedItem.Id);
                }
            }

            return View(new GridModel<UPCEditViewModel.ProductStatusWithVIMDetails>(griddata));
        }


        private bool UpdateData(UPCEditViewModel.ProductStatusWithVIMDetails editedItem)
        {
            var errorMessage = string.Empty;
            var result = false;
            if (editedItem == null)
                ViewBag.Error = "No item selected";
            else if (!editedItem.Save(out errorMessage))
                ViewBag.Error = errorMessage;
            else
            {
                ViewBag.Error = "Item saved.";
                result = true;
            }
            return result;
        }


        public bool DeleteData(UPCEditViewModel.ProductStatusWithVIMDetails deletedItem)
        {
            var errorMessage = string.Empty;
            deletedItem.Delete(out errorMessage);
            return true;
        }

    }

    public class isValidUPCResponse
    {
        public bool ExistsInOOS;
        public OOSCommon.VIM.ItemMasterModel VimInfo;
    }
}
