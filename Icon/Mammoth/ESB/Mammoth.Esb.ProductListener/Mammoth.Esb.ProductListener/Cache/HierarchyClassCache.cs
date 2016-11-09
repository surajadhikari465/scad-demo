using Icon.Framework;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Mammoth.Esb.ProductListener.Cache
{
    public class HierarchyClassCache : IHierarchyClassCache
    {
        private const string TaxHierarchyClassKey = "Tax";
        private const int CacheExpirationTime = 30;

        private IQueryHandler<GetHierarchyClassesParameters, List<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        private MemoryCache cache;

        public HierarchyClassCache(IQueryHandler<GetHierarchyClassesParameters, List<HierarchyClassModel>> getHierarchyClassesQueryHandler)
        {
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;
            cache = MemoryCache.Default;
        }

        public void Initialize()
        {
            GetTaxDictionary();
        }

        public Dictionary<string, int> GetTaxDictionary()
        {
            var dictionary = cache.Get(TaxHierarchyClassKey) as Dictionary<string, int>;
            if (dictionary == null)
            {
                var parameters = new GetHierarchyClassesParameters { HierarchyId = Hierarchies.Tax };
                var hierarchyClasses = getHierarchyClassesQueryHandler.Search(parameters);

                dictionary = new Dictionary<string, int>();
                foreach (var hierarchyClass in hierarchyClasses)
                {
                    dictionary.Add(ParseTaxCode(hierarchyClass.HierarchyClassName), hierarchyClass.HierarchyClassId);
                }

                cache.Set(TaxHierarchyClassKey, dictionary, DateTime.Now.AddMinutes(CacheExpirationTime));

                return dictionary;
            }
            else
            {
                return dictionary;
            }
        }

        private string ParseTaxCode(string hierarchyClassName)
        {
            return hierarchyClassName.Substring(0, hierarchyClassName.IndexOf(' '));
        }
    }
}
