using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using MassTransit;
using OOS.Model;
using OOSCommon;
using OOSCommon.DataContext;
//using OutOfStock.Messages;
using StructureMap;

namespace OutOfStock.Models
{

    /// <summary>
    /// View Model based on KNOWN_OOS_DETAIL
    /// </summary>
    
    public class RegionalBuyerViewModel
    {
        public string ID { get; set; }                        // hidden
        public string NAT_UPC { get; set; }                // col 1
        public string LONG_DESCRIPTION { get; set; }       // col 3
        public string ITEM_SIZE { get; set; }              // col 4
        public string ITEM_UOM { get; set; }               // col 5
        public string BRAND { get; set; }                  // col 2
        public string productStatus { get; set; }          // col 6, Should be an enum

        [DataType(DataType.Date)]
        public DateTime? UploadDate { get; set; }
        public string Region { get; set; }
        public string VendorKey { get; set; }
        public string Vin { get; set; }

        [DataType(DataType.Date)]
        public DateTime? expirationDate { get; set; }      // col 7, Should be an enum

        public bool isSelected { get; set; }               // for bulk edit

        public RegionalBuyerViewModel()
        {
        }

        public RegionalBuyerViewModel(
            string ID, string NAT_UPC, string productStatus, DateTime? expirationDate,
            string LONG_DESCRIPTION, string ITEM_SIZE, string ITEM_UOM, string BRAND)
        {
            this.ID = ID;
            this.NAT_UPC = NAT_UPC;
            this.productStatus = productStatus;
            this.expirationDate = expirationDate;
            this.LONG_DESCRIPTION = LONG_DESCRIPTION;
            this.ITEM_SIZE = ITEM_SIZE;
            this.ITEM_UOM = ITEM_UOM;
            this.BRAND = BRAND;
            this.isSelected = false;
        }

        public RegionalBuyerViewModel(DateTime uploadDate, string region, string vendorKey, KNOWN_OOS_DETAIL knownOOSDetail, OOSCommon.VIM.ItemMasterModel itemMaster)
        {
            this.ID = knownOOSDetail.ID + "+" + vendorKey + "+" + region;
            this.NAT_UPC = knownOOSDetail.UPC;  // Not NAT_UPC
            this.productStatus = knownOOSDetail.ProductStatus;
            this.expirationDate = knownOOSDetail.ExpirationDate;
            UploadDate = uploadDate;
            Region = region;
            VendorKey = vendorKey;
            Vin = knownOOSDetail.VIN;

            this.isSelected = false;
            if (itemMaster == null)
            {
                this.LONG_DESCRIPTION = string.Empty;
                this.ITEM_SIZE = string.Empty;
                this.ITEM_UOM = string.Empty;
                this.BRAND = string.Empty;
            }
            else
            {
                this.LONG_DESCRIPTION = itemMaster.LONG_DESCRIPTION;
                this.ITEM_SIZE = itemMaster.ITEM_SIZE;
                this.ITEM_UOM = itemMaster.ITEM_UOM;
                this.BRAND = itemMaster.BRAND;
            }
        }

        //public bool UpdateKnowOOSDetail(out string errorMessage)
        //{
        //    bool isOk = false;
        //    errorMessage = string.Empty;
        //    OOSEntities db = new OOSEntities();
        //    var id = Convert.ToInt32(ID.Split('+')[0]);
        //    KNOWN_OOS_DETAIL knownOOSDetail =
        //        (from kod in db.KNOWN_OOS_DETAIL where kod.ID == id && !kod.isDeleted select kod)
        //        .FirstOrDefault();
        //    if (knownOOSDetail == null)
        //        errorMessage = "Item not found";
        //    else if (!string.IsNullOrWhiteSpace(knownOOSDetail.ProductStatus) &&
        //        knownOOSDetail.ExpirationDate.HasValue &&
        //        knownOOSDetail.ProductStatus.Equals(this.productStatus) &&
        //        knownOOSDetail.ExpirationDate.Equals(this.expirationDate))
        //        errorMessage = "No changes";
        //    else
        //    {
        //        //knownOOSDetail.ProductStatus = this.productStatus;
        //        //knownOOSDetail.ExpirationDate = this.expirationDate;
        //        //db.SaveChanges();

        //        var vin = knownOOSDetail.VIN;
        //        var upc = knownOOSDetail.UPC;
        //        var reason = (from c in db.REASON where knownOOSDetail.REASON_ID == c.ID select c.REASON_DESCRIPTION).FirstOrDefault() ?? Constants.ReasonNotSet;
        //        var startDate = knownOOSDetail.START_DATE.HasValue ? knownOOSDetail.START_DATE.Value : Constants.StartDateNotSet;
        //        var expiry = expirationDate.HasValue ? expirationDate.Value : Constants.StartDateNotSet;
        //        var status = new ProductStatus(0,Region, VendorKey, vin, upc, reason, startDate, productStatus, expiry);
        //        var productStatuses = new List<ProductStatus> { status };
        //        var uploadCommands = new ProductStatusToKnownUploadCommandMapper().Map(knownOOSDetail.KNOWN_OOS_HEADER.CREATED_DATE, productStatuses);
          
        //        //[RDE] write changes to database
        //        //[RDE] remove service bus reference for now.

        //        //foreach (var command in uploadCommands)
        //        //{
        //        //    ObjectFactory.GetInstance<IServiceBus>().Publish(command);
        //        //}
                
        //        isOk = true;
        //    }
        //    return isOk;
        //}

        //public static bool UpdateKnowOOSDetail(int id, string region, string vendorKey, string bulkProductStatus, DateTime? bulkExpirationDatePicker, out string errorMessage)
        //{
        //    //updating table for the values entered for Product status and Expiration date
        //    errorMessage = string.Empty;
        //    bool isOk = false;
        //    var db = new OOSEntities();
        //    var knownOOSDetail =
        //        (from kod in db.KNOWN_OOS_DETAIL where kod.ID == id && !kod.isDeleted select kod)
        //        .FirstOrDefault();
        //    if (knownOOSDetail == null)
        //    {
        //        isOk = false;
        //        errorMessage = "Item not found.";
        //    }
        //    else
        //    {
        //        int changes = 0;
        //        if (!string.IsNullOrWhiteSpace(bulkProductStatus) &&
        //            !knownOOSDetail.ProductStatus.Equals(bulkProductStatus))
        //        {
        //            knownOOSDetail.ProductStatus = bulkProductStatus;
        //            ++changes;
        //        }
        //        if (bulkExpirationDatePicker.HasValue)
        //        {
        //            if (!knownOOSDetail.ExpirationDate.HasValue ||
        //                !knownOOSDetail.ExpirationDate.Equals(bulkExpirationDatePicker))
        //            {
        //                knownOOSDetail.ExpirationDate = bulkExpirationDatePicker.Value;
        //                ++changes;  
        //            }
        //        }
        //        if (changes < 1)
        //        {
        //            isOk = false;
        //            errorMessage = "No change.";
        //        }
        //        else
        //        {
        //            var vin = knownOOSDetail.VIN;
        //            var upc = knownOOSDetail.UPC;
        //            var reason = (from c in db.REASON where knownOOSDetail.REASON_ID == c.ID select c.REASON_DESCRIPTION).FirstOrDefault() ?? Constants.ReasonNotSet;
        //            var startDate = knownOOSDetail.START_DATE.HasValue ? knownOOSDetail.START_DATE.Value : Constants.StartDateNotSet;
        //            var expiry = bulkExpirationDatePicker.HasValue ? bulkExpirationDatePicker.Value : Constants.StartDateNotSet;
        //            var status = new ProductStatus(0,region, vendorKey, vin, upc, reason, startDate, bulkProductStatus, expiry);
        //            var productStatuses = new List<ProductStatus> {status};
        //            var uploadCommands = new ProductStatusToKnownUploadCommandMapper().Map(knownOOSDetail.KNOWN_OOS_HEADER.CREATED_DATE, productStatuses);


        //            System.Text.StringBuilder statements;
                    
        //            foreach (KnownUploadCommand command in uploadCommands)
        //            {
        //                //statements.AppendFormat("update known_oos_detail set productstatus = '{0}' where id = '{1}';", command.Items.);
        //            }

        //            //[RDE] remove service bus reference
        //            //foreach (var command in uploadCommands)
        //            //{
        //            //    ObjectFactory.GetInstance<IServiceBus>().Publish(command);
        //            //}
        //            isOk = true;
        //        }
        //    }
        //    return isOk;
        //}

        //public static bool DeleteKnowOOSDetail(int id, out string errorMessage)
        //{
        //    //deleting records in the table for the records selected on UI
        //    errorMessage = string.Empty;
        //    bool isOk = false;
        //    var db = new OOSEntities();
        //    KNOWN_OOS_DETAIL knownOOSDetail =
        //        (from kod in db.KNOWN_OOS_DETAIL where kod.ID == id && !kod.isDeleted select kod)
        //        .FirstOrDefault();
        //    if (knownOOSDetail == null)
        //    {
        //        isOk = false;
        //        errorMessage = "Item not found.";
        //    }
        //    else if (knownOOSDetail.isDeleted)
        //    {
        //        isOk = false;
        //        errorMessage = "Item already deleted.";
        //    }
        //    else
        //    {
        //        db.KNOWN_OOS_DETAIL.DeleteObject(knownOOSDetail);
        //        db.SaveChanges();
        //        isOk = true;
        //    }
        //    return isOk;
        //}


        //public static IEnumerable<RegionalBuyerViewModel> SearchByProductStatus_New(string productSatus)
        //{

        //    using (var db = new OOSEntities())
        //    {
        //        var q = from ps in db.ProductStatus
        //                where ps.ProductStatus.Contains(productSatus)
        //                select new ProductStatus(ps.id, ps.Region, ps.VendorKey, ps.Vin, ps.UPC, ps.Reason, ps.StartDate,
        //                                         ps.ProductStatus, ps.ExpirationDate);
        //    }
        //        ;

        //    return new List<RegionalBuyerViewModel>();
        //}

        //public static IEnumerable<RegionalBuyerViewModel> SearchByProductStatus(string productStatus)
        //{
        //    OutOfStock.MvcApplication.oosLog.Trace("Enter");
        //    var resultModel = new List<RegionalBuyerViewModel>();
            

        //    var db =new OOSEntities();
        //    /*
        //     * Get DISTINCT list of Known_OOS_Detail.UPC  that:
        //     * isDeleted = false;
        //     * ProductStatus = <search criteria>
        //     *      
        //    */




        //    IQueryable<KNOWN_OOS_DETAIL> knownOOSDetails = null;
        //    knownOOSDetails = db.KNOWN_OOS_DETAIL.Where(kod => !kod.isDeleted);

        //    if (string.IsNullOrWhiteSpace(productStatus))
        //    {
        //        knownOOSDetails = knownOOSDetails.Where(kod => string.IsNullOrEmpty(kod.ProductStatus));
        //    }
        //    else
        //    {
        //        knownOOSDetails = knownOOSDetails.Where(kod => kod.ProductStatus.Equals(productStatus));
        //    }

        //    //knownOOSDetails = knownOOSDetails.OrderBy(kod => kod.UPC).Select(kod => kod);
        //    IEnumerable<string> upcs = knownOOSDetails.OrderBy(d => d.UPC).Select(d => d.UPC).Distinct();
            



        //    /*
        //     * get item information from VIM.
        //     */
        //    Dictionary<string, OOSCommon.VIM.ItemMasterModel> dicUPCDetail = OutOfStock.MvcApplication.vimRepository.GetItemMasterModel(upcs);
            



            
        //    // Form result set
        //    foreach (KNOWN_OOS_DETAIL knownOOSDetail in knownOOSDetails)
        //    {
        //        OOSCommon.VIM.ItemMasterModel itemMaster = null;
        //        // Get the details for the UPC
        //        if (
        //                !string.IsNullOrEmpty(knownOOSDetail.UPC)
        //                && knownOOSDetail.UPC.Length == 13
        //                && dicUPCDetail.ContainsKey(knownOOSDetail.UPC)
        //            ) { 
        //                itemMaster = dicUPCDetail[knownOOSDetail.UPC];
        //            }

        //        var header = knownOOSDetail.KNOWN_OOS_HEADER;
        //        if (header == null  || header.KNOWN_OOS_MAP == null) continue;
        //        foreach (var map in header.KNOWN_OOS_MAP)
        //        {
        //            var region = map.REGION.REGION_ABBR;
        //            var vendorKey = map.VENDOR_KEY;
        //            if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(vendorKey)) continue;
        //            resultModel.Add(new RegionalBuyerViewModel(header.CREATED_DATE, region, vendorKey, knownOOSDetail, itemMaster));
        //        }

        //    }
        //    OutOfStock.MvcApplication.oosLog.Trace("Exit");
        //    return resultModel;
        //}
            

        public static IEnumerable<RegionalBuyerViewModel> SearchByUPC(string upcVIM)
        {
           // OutOfStock.MvcApplication.oosLog.Trace("Enter");
            var resultModel = new List<RegionalBuyerViewModel>();
            var db = new OOSEntities();
            // Get the details for the UPC (from VIM vis linked server)
            OOSCommon.VIM.ItemMasterModel itemMaster =
                OutOfStock.MvcApplication.vimRepository.GetItemMasterModel(upcVIM);
            // Get the OOS details
            IQueryable<KNOWN_OOS_DETAIL> knownOOSDetails = from     kod in db.KNOWN_OOS_DETAIL 
                                                           where    kod.UPC.Equals(upcVIM) 
                                                                    && !kod.isDeleted 
                                                           select   kod;  // Not NAT_UPC
            // Form result set
            foreach (KNOWN_OOS_DETAIL knownOOSDetail in knownOOSDetails)
            {
                var header = knownOOSDetail.KNOWN_OOS_HEADER;
                if (header == null || header.KNOWN_OOS_MAP == null) continue;
                foreach (var map in header.KNOWN_OOS_MAP)
                {
                    var region = map.REGION.REGION_ABBR;
                    var vendorKey = map.VENDOR_KEY;
                    if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(vendorKey)) continue;
                    resultModel.Add(new RegionalBuyerViewModel(header.CREATED_DATE, region, vendorKey, knownOOSDetail, itemMaster));
                } 
            }
           // OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return resultModel;
        }

    }
}