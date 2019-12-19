using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class RetailItem : IRetailItem
    {
        private double cost;
        private double price;
        private IProduct product;

        public RetailItem(IProduct product)
        {
            this.product = product;
        }

        public UPC UPC
        {
            get { return product.UPC; }
        }

        public string Brand
        {
            get { return product.Brand; }
        }

        public string BrandName { get { return product.BrandName; } }

        public string LongDescription
        {
            get { return product.LongDescription; }
        }

        public string Size
        {
            get { return product.Size; }
        }

        public string UOM
        {
            get { return product.UOM; }
        }

        public string CategoryName
        {
            get { return product.CategoryName; }
        }

        public string ClassName
        {
            get { return product.ClassName; }
        }

        public string VendorKey { get; set; }

        public string VendorItemNumber { get; set; }

        public string TeamName { get; set; }

        public string SubTeamName { get; set; }

        public string PriceType { get; set; }

        public string PepoleSoftBusinessUnit { get; set; }

        public double Cost
        {
            get { return Math.Round(cost, 2); }
            set { cost = value; }
        }

        public double Price
        {
            get { return Math.Round(price, 2); }
            set { price = value; }
        }

        public decimal CaseSize { get; set; }
    }
}
