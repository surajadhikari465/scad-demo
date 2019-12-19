using System;
using System.Collections.Generic;
using System.Linq;
using OOS.Model;
using OOSCommon.DataContext;

namespace OffshelfUploadBoundedContext
{
    public class ProductStatusRepository : IProductStatusRepository
    {
        private List<ProductStatus> items = new List<ProductStatus>();
        private ICreateDisposableEntities entityFactory;

        public ProductStatusRepository(ICreateDisposableEntities entityFactory)
        {
            this.entityFactory = entityFactory;
        }

        public ProductStatus For(string region, string vendorKey, string vin, string upc)
        {
            var productStatus = new ProductStatus(region, vendorKey, vin, upc);
            return For(productStatus);
        }

        public int Count
        {
            get { return items.Count; }
        }

        private ProductStatus For(ProductStatus productStatus)
        {
            if (!items.Any(p => p == productStatus))
            {
                var searched = Search(productStatus);
                if (searched != null)
                {
                    items.Add(searched);
                }
            }
            return items.FirstOrDefault(p => p == productStatus);
        }

        private ProductStatus Search(ProductStatus productStatus)
        {
            using (var dbContext = entityFactory.New())
            {
                var searched = (from c in dbContext.ProductStatus 
                                where c.UPC == productStatus.Upc && c.Region == productStatus.Region && c.VendorKey == productStatus.VendorKey && c.Vin == productStatus.Vin
                                select c).FirstOrDefault();
                return searched == null ? null : new ProductStatus(searched.Region, searched.VendorKey, searched.Vin, searched.UPC);
                    //new ProductStatus(searched.Region, searched.VendorKey, searched.Vin, searched.UPC, searched.Reason, searched.StartDate, 
                        //searched.ProductStatus, searched.ExpirationDate.HasValue ? searched.ExpirationDate.Value : DateTime.MinValue);
            }
        }

        public void Insert(ProductStatus productStatus)
        {
            if (For(productStatus) == null)
            {
                items.Add(productStatus);
                Save(productStatus);
            }
        }

        private void Save(ProductStatus productStatus)
        {
            using (var dbContext = entityFactory.New())
            {
                var status = new ProductStatu
                                 {
                                     UPC = productStatus.Upc,
                                     Region = productStatus.Region,
                                     VendorKey = productStatus.VendorKey,
                                     Vin = productStatus.Vin,
                                     Reason = productStatus.Reason,
                                     StartDate = productStatus.StartDate,
                                     ProductStatus = productStatus.Status, 
                                     ExpirationDate = productStatus.ExpirationDate
                                 };
                dbContext.ProductStatus.AddObject(status);
                dbContext.SaveChanges();
            }
        }

        public void Modify(ProductStatus productStatus)
        {
            if (For(productStatus) != null)
            {
                items.Remove(productStatus);
                items.Add(productStatus);
                Update(productStatus);
            }
        }

        private void Update(ProductStatus productStatus)
        {
            using (var dbContext = entityFactory.New())
            {
                var searched = (from c in dbContext.ProductStatus 
                                where c.UPC == productStatus.Upc && c.Region == productStatus.Region && c.VendorKey == productStatus.VendorKey && c.Vin == productStatus.Vin
                                select c).FirstOrDefault();
                if (searched == null) return;
                searched.ProductStatus = productStatus.Status;
                searched.ExpirationDate = productStatus.ExpirationDate;
                dbContext.SaveChanges();
            }
        }

        public void Remove(ProductStatus productStatus)
        {
            if (For(productStatus) != null)
            {
                items.Remove(productStatus);
                Delete(productStatus);
            }
        }

        private void Delete(ProductStatus productStatus)
        {
            using (var dbContext = entityFactory.New())
            {
                var searched = (from c in dbContext.ProductStatus where c.UPC == productStatus.Upc select c).FirstOrDefault();
                if (searched == null) return;
                dbContext.ProductStatus.DeleteObject(searched);
                dbContext.SaveChanges();
            }
        }
    }
}
