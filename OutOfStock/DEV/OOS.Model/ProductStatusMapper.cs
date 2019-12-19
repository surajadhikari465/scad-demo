using System;
using OutOfStock.Messages;

namespace OOS.Model
{
    public class ProductStatusMapper
    {
        //public static ProductStatus ToProductStatus(ProductStatusInsertedEvent statusEvent)
        //{
        //    //return ProductStatusFor(statusEvent.Region, statusEvent.VendorKey, statusEvent.Vin, statusEvent.Upc, 
        //    //    statusEvent.Reason, statusEvent.StartDate, statusEvent.ProductStatus, statusEvent.ExpirationDate);
        //}

        //[Obsolete("shouldnt use this",true)]
        //private static ProductStatus ProductStatusFor(string region, string vendorKey, string vin, string upc, 
        //    string reason, DateTime startDate, string status, DateTime expiryDate)
        //{
        //    return new ProductStatus(0,region, vendorKey, vin, upc, reason, startDate, status, expiryDate);
        //}

        //public static ProductStatus ToProductStatus(ProductStatusModifiedEvent statusEvent)
        //{
        //    //return ProductStatusFor(statusEvent.Region, statusEvent.VendorKey, statusEvent.Vin, statusEvent.Upc,
        //    //    statusEvent.Reason, statusEvent.StartDate, statusEvent.ProductStatus, statusEvent.ExpirationDate);
        //}

        //public static ProductStatus ToProductStatus(ProductStatusRemovedEvent statusEvent)
        //{
        //    //return ProductStatusFor(statusEvent.Region, statusEvent.VendorKey, statusEvent.Vin, statusEvent.Upc,
        //    //    statusEvent.Reason, statusEvent.StartDate, statusEvent.ProductStatus, statusEvent.ExpirationDate);
        //}

        //public static ProductStatusModifiedEvent ToProductStatusModifiedEvent(ProductStatus projection)
        //{
        //    //var e = new ProductStatusModifiedEvent
        //    //            {
        //    //                ExpirationDate = projection.ExpirationDate,
        //    //                ProductStatus = projection.Status,
        //    //                //Reason = projection.Reason,
        //    //                Region = projection.Region,
        //    //                //StartDate = projection.StartDate,
        //    //                Upc = projection.Upc,
        //    //                //VendorKey = projection.VendorKey
        //    //            };
        //   // return e;
        //    return new ProductStatusModifiedEvent();
        //}

        //public static ProductStatusInsertedEvent ToProductStatusInsertedEvent(ProductStatus projection)
        //{
        ////    var e = new ProductStatusInsertedEvent
        ////                {
        ////                    ExpirationDate = projection.ExpirationDate,
        ////                    ProductStatus = projection.Status,
        ////                    //Reason = projection.Reason,
        ////                    Region = projection.Region,
        ////                    //StartDate = projection.StartDate,
        ////                    Upc = projection.Upc,
        ////                    //VendorKey = projection.VendorKey
        ////                };
        //    //return e;
        //return new ProductStatusInsertedEvent();
        //}
    }
}
