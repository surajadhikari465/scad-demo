using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Mappers
{
    public class HierarchyClassIdMapper : IHierarchyClassIdMapper
    {
        private IHierarchyClassCache cache;

        public HierarchyClassIdMapper(IHierarchyClassCache cache)
        {
            this.cache = cache;
        }

        public void PopulateHierarchyClassDatabaseIds(List<ProductModel> products)
        {
            var taxHierarchyClasses = cache.GetTaxDictionary();

            foreach (var product in products)
            {
                product.TaxClassHCID = taxHierarchyClasses[product.MessageTaxClassHCID];
            }
        }
    }
}
