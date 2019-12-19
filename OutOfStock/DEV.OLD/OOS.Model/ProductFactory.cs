using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon;
using SharedKernel;

namespace OOS.Model
{
    public class ProductFactory : IProductFactory
    {
        private static IOOSLog logger;

        public ProductFactory(ILogService logService)
        {
            logger = logService.GetLogger();
        }

        public IEnumerable<IProduct> Reconstitute(IEnumerable<string> upcs, IEnumerable<ProductDTO> productDtos)
        {
            var products = CreateProducts(productDtos);
            var productToDictionary = products.ToDictionary(p => p.UPC.Code, q => q);
            return upcs.Select(p => productToDictionary.Keys.Contains(p) ? productToDictionary[p] : NewProduct(p));
        }

        private IEnumerable<IProduct> CreateProducts(IEnumerable<ProductDTO> productDtos)
        {
            var firstProducts = GetFirstProducts(productDtos);
            var products = firstProducts.Select(dto => new Product(dto.UPC)
            {
                Brand = dto.BRAND,
                BrandName = dto.BRAND_NAME,
                LongDescription = dto.LONG_DESCRIPTION,
                Size = dto.ITEM_SIZE,
                UOM = dto.ITEM_UOM,
                CategoryName = dto.CATEGORY_NAME,
                ClassName = dto.CLASS_NAME
            });
            return products;
        }

        private IEnumerable<ProductDTO> GetFirstProducts(IEnumerable<ProductDTO> productDtos)
        {
            var from = (from c in productDtos group c by c.UPC into g select new { g.Key, products = g, n = g.Count() });
            foreach (var upc in from.Where(p => p.n > 1))
            {
                logger.Trace(string.Format("GetFirstProducts(): UPC='{0}' Appears more than once", upc.Key));
            }
            return from.Select(g => g.products.FirstOrDefault());
        }

        private static IProduct NewProduct(string upc)
        {
            return new Product(upc)
            {
                Brand = string.Empty,
                LongDescription = string.Empty,
                CategoryName = string.Empty,
                ClassName = string.Empty,
                Size = string.Empty,
                UOM = string.Empty
            };
        }


    }
}
