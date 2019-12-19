using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;

namespace OffshelfUploadBoundedContext
{
    public interface IProductStatusRepository
    {
        void Insert(ProductStatus productStatus);
        void Modify(ProductStatus productStatus);
        void Remove(ProductStatus productStatus);
        ProductStatus For(string region, string vendorKey, string vin, string upc);
        int Count { get; }
    }
}
