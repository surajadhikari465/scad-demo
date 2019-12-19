using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OOSCommon;
using OOSCommon.DataContext;
using OOSCommon.VIM;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;
using Telerik.Web.Mvc.UI;
using OutOfStock.Models;
using OOS.Model;

namespace OutOfStock.Controllers
{
    public class UPCEditableModel
    {
        public int Id;
        public string UPC;
        public string Status;
        public DateTime? ExpirationDate;
        
    }

    public class UPCEditController : Controller
    {

    

const string sessionIxUPCEditController = "UPCEditController";

        //private IEnumerable<RegionalBuyerViewModel> viewModel { get; set; }
        private UPCEditViewModel viewModel { get; set; }
        private string currentUPCToFind
        {
            get { return (string)Session[sessionIxUPCEditController + ".currentUPC"] ?? string.Empty; }
            set { Session[sessionIxUPCEditController + ".currentUPC"] = value; }
        }
        private string currentUPCVim
        {
            get { return (string)Session[sessionIxUPCEditController + ".currentUPCVim"] ?? string.Empty; }
            set { Session[sessionIxUPCEditController + ".currentUPCVim"] = value; }
        }

        //
        // GET: /UPCEdit/
        public ActionResult Index()
        {
            OutOfStock.MvcApplication.oosLog.Trace("Enter");
            ActionResult result = null;
            if (!OOSUser.isRegionalBuyer)
                result = new RedirectResult("~/Home/Index");
            else
            {
                ViewBag.currentUPC = currentUPCToFind;
                result = View();
            }
            OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
        }


        

        //
        // POST: /UPCEdit/Search
        public ActionResult Search(string upcToFind, string submitButton, string newUPC, string newStatus, DateTime? newExpirationDate)
        {


            OutOfStock.MvcApplication.oosLog.Trace("Enter");

            
            ActionResult result = null;

            switch (submitButton)
            {
                case "Add":

                    if (AddItem(newUPC, OOSUser.userRegion, newStatus, newExpirationDate))
                    {
                        viewModel = SearchByUPC(newUPC);
                        result = View("Index", viewModel);

                    }
                    break;

                case "Search":
                    if (!OOSUser.isRegionalBuyer)
                        result = new RedirectResult("~/Home/Index");
                    else
                    {
                        // While this ought never be NULL, it is with Safari. Not supported but we ought not allow the exception
                        if (string.IsNullOrEmpty(upcToFind))
                            upcToFind = string.Empty;
                        string upcVIM;
                        string errorMessage;
                        ViewBag.currentUPC = currentUPCToFind = upcToFind;
                        if (!IsUPCGood(upcToFind, out upcVIM, out errorMessage))
                            // to check if the entered UPC is valid per contraints
                        {
                            ViewBag.Error = errorMessage;
                            result = View("Index");
                        }
                        else
                        {
                            currentUPCVim = upcVIM;
                            //viewModel = GetData(upcVIM); // to get data based on UPC
                            viewModel = SearchByUPC(upcVIM);
                            result = View("Index", viewModel);
                        }
                    }
                    break;
            }





            OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
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
                             new UPCEditViewModel.ProductStatusWithVIMDetails(  ps.Id, 
                                                                                ps.Upc,
                                                                                ps.Region,
                                                                                ps.Status,
                                                                                ps.StartDate,
                                                                                ps.ExpirationDate,
                                                                                (v.Value == null ?  string.Empty : v.Value.BRAND),
                                                                                (v.Value == null ?  string.Empty : v.Value.LONG_DESCRIPTION),
                                                                                (v.Value == null ? string.Empty : v.Value.ITEM_SIZE),
                                                                                (v.Value == null ? string.Empty : v.Value.ITEM_UOM) ));


                combined = q.ToList();

            }
            return new UPCEditViewModel(statuses, upcs, vim, combined);
        }
        [GridAction]
        public ActionResult GridSelect(string id, GridEditMode? mode)
        {
            ViewBag.currentUPC = currentUPCToFind;
            //viewModel = GetData(currentUPCVim);
            viewModel = SearchByUPC(currentUPCVim);
            return View("Index", viewModel);
        }
        
        [HttpPost]
        [GridAction]
        [ValidateInput(false)]
        //public ActionResult GridUpdate(UPCEditViewModel.ProductStatusWithVIMDetails item, GridEditMode? mode)
        public ActionResult GridUpdate(string UPC, int id,  DateTime? ExpirationDate, string Status, GridEditMode? mode)
        {

            if (!OOSUser.isRegionalBuyer)
                return new RedirectResult("~/Home/Index");

            if (UpdateData(id, UPC, Status, ExpirationDate))
            {
                RouteValueDictionary routeValues = this.GridRouteValues();
                    // add the editing mode to the route values
                    routeValues.Add("mode", mode);
                    return RedirectToAction("GridSelect", routeValues);
            }
            else
            {
                return View();
            }
            
        }

        [GridAction]
        [ValidateInput(false)]
        public ActionResult GridDelete(string UPC, int id,  DateTime? ExpirationDate, string Status, GridEditMode? mode)
        {
            if (!OOSUser.isRegionalBuyer)
                return new RedirectResult("~/Home/Index");
            string errorMessage = string.Empty;

            var deletedItem = new UPCEditViewModel.ProductStatusWithVIMDetails(id, UPC, Status,ExpirationDate);
            deletedItem.Delete(out errorMessage);
            
            //RegionalBuyerViewModel.DeleteKnowOOSDetail(id.Value, out errorMessage);
            ViewBag.Error = errorMessage;
            return GridSelect(string.Empty, mode);
        }

        /// <summary>
        /// Get all OOS records for the UPC provided as a 13 digit string (not enforced here)
        /// </summary>
        /// <param name="upcVIM"></param>
        /// <returns></returns>
        private IEnumerable<RegionalBuyerViewModel> GetData(string upcVIM)
        {
            return RegionalBuyerViewModel.SearchByUPC(upcVIM);
        }


        private bool AddItem(string UPC, string Region, string Status, DateTime? ExpirationDate)
        {
            if (Region == "CE")
            {
                Region = "NC"; 
            }
            var result = false;
            var errorMessage = string.Empty;

            const string sql = "exec SaveProductStatus '{0}', '{1}', '{2}', '{3}';\r\n";
            try
            {
                using (var datacontext = new OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString))
                {
                    var query = string.Format(sql, Region, UPC, ExpirationDate, Status.Replace("'", "''"));
                    datacontext.ExecuteStoreCommand(query, null);
             
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }


            return result;
        }
        
        private bool UpdateData(int id, string UPC, string Status, DateTime? ExpirationDate)
        {
            var result = false;
            var editedItem = new Models.UPCEditViewModel.ProductStatusWithVIMDetails(id, UPC, Status, ExpirationDate);
            string errorMessage = null;
            if (UPC == string.Empty)
                ViewBag.Error = "No item was selected. No changes have been saved.";
            else if (!editedItem.Save(out errorMessage))
                ViewBag.Error = errorMessage;
            else {
                result = true;
                ViewBag.Error = string.Format("Chagnes have been saved for {0}", editedItem.UPC);
            }
            return result;
        }

        /// <summary>
        /// Validate a UPC
        /// </summary>
        /// <param name="upcEntered">UPC string to validate</param>
        /// <param name="upcVIM">output UPC for VIM lookup</param>
        /// <param name="errorMessage">output error message provided when the return is false</param>
        /// <returns>true is the validation succeeds</returns>
        private bool IsUPCGood(string upcEntered, out string upcVIM, out string errorMessage)
        {
#if (true)
            if (UpcSpecification.IsSatisfiedBy(upcEntered))
            {
                upcVIM = UpcSpecification.FormValid(new List<string> { upcEntered }).First();
                errorMessage = string.Empty;
                return true;
            }
            upcVIM = string.Empty;
            errorMessage = GetErrorMessage(upcEntered);
            return false;
#else
            // Per Joe Knize -- First, discard the check digit, which is the right-most digit. 
            // On UPC-As, it’s a small number, and separated from the other digits. On EANs, 
            // it’s not small, nor separated from the rest of the digits. Take the remaining 
            // 10, 11, or 12 digits, and left pad with zeros to 13 digits.
            if (upcEntered.Length < 12)    // Joe N. says 11
                errorMessage = "The UPC, " + upcEntered + ", is too short.  Please correct it.";
            else if (upcEntered.Length > 12)    // Joe N. says 13
                errorMessage = "The UPC, " + upcEntered + ", is too long.  Please correct it.";
            else if (upcEntered.Length != 12 && upcEntered != "000000000000" && upcEntered.Substring(0, 4) == "0000")
                errorMessage = "The PLU's like " + upcEntered + " are not currently supported.";
            else
            {
                upcVIM = "000".Substring(0, 13 - upcEntered.Length + 1) + upcEntered.Substring(0, upcEntered.Length - 1);
                isOk = true;
            }
#endif
        }

        private string GetErrorMessage(string upc)
        {
            switch (UpcSpecification.GetUpcCheckValue(upc))
            {
                case Utility.eUPCCheck.Empty:
                    return "Please enter a UPC.";
                case Utility.eUPCCheck.NotNumeric:
                    return "The UPC, " + upc + ", contains non-numeric information.  Please correct it.";
                case Utility.eUPCCheck.TooShort:
                    return "The UPC, " + upc + ", is too short.  Please correct it.";
                case Utility.eUPCCheck.TooLong:
                    return "The UPC, " + upc + ", is too long.  Please correct it.";
                case Utility.eUPCCheck.IsPLU:
                    return "The PLU's like " + upc + " are not currently supported.";
                default:
                    return string.Empty;
            }

        }
    }
}
