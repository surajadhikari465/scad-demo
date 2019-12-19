using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace ProductDataBoundedContext
{
    public class ProductDataService
    {
        private IRetailItemRepository retailItemRepository;

        public ProductDataService(IRetailItemRepository retailItemRepository)
        {
            this.retailItemRepository = retailItemRepository;
        }

        public List<IRetailItem> QueryProductData(IEnumerable<string> upcs, string storeAbbrev)
        {
            return retailItemRepository.For(upcs, storeAbbrev);
        }


    }
}
