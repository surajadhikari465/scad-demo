using System;
using System.Collections.Generic;
using System.Linq;
using OOS.Model;
using OOSCommon.DataContext;

namespace OutOfStock.Service
{
    public class ReadProductStatusService
    {
        private ICreateDisposableEntities factory;

        public ReadProductStatusService(ICreateDisposableEntities factory)
        {
            this.factory = factory;
        }

        public IEnumerable<ProductStatus> For(OOSCustomReport report)
        {
            var result = new List<ProductStatus>();
            var statusKeys = (from c in report.Entries() select new { c.VENDOR_KEY, c.VIN, c.UPC }).Distinct();
            using (var objContext = factory.New())
            {
                foreach (var statusKey in statusKeys)
                {
                    var key = statusKey;
                    var regionalStatus = (from c in objContext.ProductStatus
                                          where c.UPC == key.UPC && c.Vin == key.VIN && c.VendorKey == key.VENDOR_KEY
                                          select new {Region = c.Region, VendorKey = c.VendorKey, Vin = c.Vin, Upc = c.UPC, Reason = c.Reason, StartDate = c.StartDate, Status = c.ProductStatus, ExpirationDate = c.ExpirationDate});

                    foreach (var status in regionalStatus)
                    {
                        //var productStatus = new ProductStatus(status.Region, status.VendorKey, status.Vin, status.Upc, status.Reason, status.StartDate, status.Status, status.ExpirationDate.HasValue ? status.ExpirationDate.Value : DateTime.MinValue);
                        var productStatus = new ProductStatus(0,status.Region, status.VendorKey, status.Vin, status.Upc, status.Reason, status.StartDate, status.Status, status.ExpirationDate.HasValue ? status.ExpirationDate.Value : DateTime.MinValue);
                        if (!result.Contains(productStatus))
                            result.Add(productStatus);
                    }
                }
                return result;
            }
        }
    }
}