using System;
using System.Collections.Generic;
using System.Linq;
using OOS.Model;
using OutOfStock.Models;
using SharedKernel;

namespace OutOfStock.Service
{
    public class CustomReportAdapter
    {
        private IProductRepository productRepository;

        public CustomReportAdapter(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public List<CustomReportViewModel> Adapt(OOSCustomReport report)
        {
            var entries = report.Entries();
            var days = (report.Days == 0) ? 1 : report.Days;
            return (from item in entries
                    let timesScanned = item.timesScanned
                    let cost = decimal.Round(((item.CASE_SIZE) == 0 ? 0 : ((item.MOVEMENT) * (item.EFF_COST)) / item.CASE_SIZE), 2)
                    let margin = ((item.EFF_PRICE) == 0 || (item.CASE_SIZE) == 0 ? 0 : Math.Round((item.EFF_PRICE - ((item.EFF_COST)/item.CASE_SIZE))/item.EFF_PRICE*100))
                    let avgUnitOpportunity = decimal.Round((item.MOVEMENT), 2)
                    let avgSalesOpportunity = decimal.Round(item.MOVEMENT * item.EFF_PRICE, 2)
                    let sales = decimal.Round(item.MOVEMENT * item.EFF_PRICE * days, 2)
                    let productStatus = item.ProductStatus
                    select new CustomReportViewModel(item.PS_SUBTEAM, item.UPC, item.BRAND, item.BRAND_NAME, item.LONG_DESCRIPTION, item.ITEM_SIZE, item.ITEM_UOM, item.VENDOR_KEY, item.VIN, sales, timesScanned, item.NOTES, cost, margin, item.EFF_PRICE, item.EFF_PRICETYPE, item.CATEGORY_NAME, item.CLASS_NAME, avgUnitOpportunity, avgSalesOpportunity, productStatus, item.StoresList)).ToList();
        }


        private Dictionary<string, IProduct> GetProductsFor(OOSCustomReport report)
        {
            var upcs = report.ReportUPCs();
            var products = productRepository.For(upcs);
            return products.ToDictionary(p => p.UPC.Code, q => q);
        }

    }
}