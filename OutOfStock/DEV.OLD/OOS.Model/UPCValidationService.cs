using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class UPCValidationService
    {
        private IProductRepository productRepository;

        public UPCValidationService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public bool ValidateProduct(string upc)
        {
            var product = productRepository.For(upc);
            return (product != null && product.UPC.Code == upc) ;
        }

        public IDictionary<string, bool> ValidateProduct(IEnumerable<string> upcs)
        {
            var products = productRepository.For(upcs);
            return upcs.ToDictionary(p => p, r => products.Select(q => q.UPC.Code).ToList().Contains(r));
        }
    }
}
