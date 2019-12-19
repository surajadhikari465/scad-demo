using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class Product : IProduct
    {
        public Product(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new InvalidUPCException("UPC Invalid: null or empty");

            UPC = new UPC(code);
        }

        public UPC UPC { get; private set; }
        public string Brand { get; set; }
        public string BrandName { get; set; }
        public string LongDescription { get; set; }
        public string Size { get; set; }
        public string UOM { get; set; }
        public string CategoryName { get; set; }
        public string ClassName { get; set; }

    }
}
