using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Infor.Services.NewItem.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Infor.Services.NewItem.Models;

namespace Infor.Services.NewItem.Cache
{
    public class IconCache : IIconCache
    {
        private const string TaxCache = "TaxCache";
        private const string NationalCache = "NationalCache";
        private const string BrandCache = "BrandCache";
        private const string BrandModelCache = "BrandModelCache";
        private const string TaxModelCache = "TaxModelCache";
        private const string NationalClassModelCache = "NationalModelCache";
        private const string FinancialClassModelCache = "FinancialModelCache";

        private MemoryCache cache;
        private IQueryHandler<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery, Dictionary<string, int>> getHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler;
        private IQueryHandler<GetHierarchyClassTraitQuery, Dictionary<int, string>> getHierarchyClassTraitQueryHandler;
        private IQueryHandler<GetBrandDictionaryQuery, Dictionary<int, BrandModel>> getBrandDictionaryQueryHandler;
        private IQueryHandler<GetTaxClassDictionaryQuery, Dictionary<string, TaxClassModel>> getTaxDictionaryQueryHandler;
        private IQueryHandler<GetNationalClassDictionaryQuery, Dictionary<string, NationalClassModel>> getNationalClassDictionaryQueryHandler;
        private IQueryHandler<GetSubTeamsDictionaryQuery, Dictionary<string, SubTeamModel>> getSubTeamModelDictionaryQueryHandler;

        private int cacheExpirationTimeInMinutes;

        public IconCache(
            IQueryHandler<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery, Dictionary<string, int>> getHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler,
            IQueryHandler<GetHierarchyClassTraitQuery, Dictionary<int, string>> getHierarchyClassTraitQueryHandler,
            IQueryHandler<GetBrandDictionaryQuery, Dictionary<int, BrandModel>> getBrandDictionaryQueryHandler,
            IQueryHandler<GetTaxClassDictionaryQuery, Dictionary<string, TaxClassModel>> getTaxDictionaryQueryHandler,
            IQueryHandler<GetNationalClassDictionaryQuery, Dictionary<string, NationalClassModel>> getNationalClassDictionaryQueryHandler,
            IQueryHandler<GetSubTeamsDictionaryQuery, Dictionary<string, SubTeamModel>> getSubTeamModelDictionaryQueryHandler)
        {
            cache = new MemoryCache(Guid.NewGuid().ToString());
            this.getHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler = getHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler;
            this.getHierarchyClassTraitQueryHandler = getHierarchyClassTraitQueryHandler;
            this.cacheExpirationTimeInMinutes = AppSettingsAccessor.GetIntSetting("CacheExpirationTimeInMinutes", 15);
            this.getBrandDictionaryQueryHandler = getBrandDictionaryQueryHandler;
            this.getTaxDictionaryQueryHandler = getTaxDictionaryQueryHandler;
            this.getNationalClassDictionaryQueryHandler = getNationalClassDictionaryQueryHandler;
            this.getSubTeamModelDictionaryQueryHandler = getSubTeamModelDictionaryQueryHandler;
        }
        
        public Dictionary<string, int> TaxClassCodesToIdDictionary
        {
            get
            {
                var taxDictionary = cache[TaxCache] as Dictionary<string, int>;
                if(taxDictionary == null)
                {
                    taxDictionary = getHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Search(new GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery { HierarchyId = Hierarchies.Tax });

                    cache.Set(TaxCache, taxDictionary, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return taxDictionary;
            }
        }

        public Dictionary<string, int> NationalClassCodesToIdDictionary
        {
            get
            {
                var nationalDictionary = cache[NationalCache] as Dictionary<string, int>;
                if (nationalDictionary == null)
                {
                    nationalDictionary = getHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Search(new GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery { HierarchyId = Hierarchies.National });

                    cache.Set(NationalCache, nationalDictionary, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return nationalDictionary;
            }
        }

        public Dictionary<int, string> BrandIdToAbbreviationDictionary
        {
            get
            {
                var brandAbbreviationDictionary = cache[BrandCache] as Dictionary<int, string>;
                if (brandAbbreviationDictionary == null)
                {
                    brandAbbreviationDictionary = getHierarchyClassTraitQueryHandler.Search(new GetHierarchyClassTraitQuery { TraitId = Traits.BrandAbbreviation });
                    cache.Set(BrandCache, brandAbbreviationDictionary, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return brandAbbreviationDictionary;
            }
        }

        public Dictionary<int, BrandModel> BrandDictionary
        {
            get
            {
                var brandModelCache = cache[BrandModelCache] as Dictionary<int, BrandModel>;
                if (brandModelCache == null)
                {
                    brandModelCache = getBrandDictionaryQueryHandler.Search(new GetBrandDictionaryQuery());
                    cache.Set(BrandModelCache, brandModelCache, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return brandModelCache;
            }
        }

        public Dictionary<string, TaxClassModel> TaxDictionary
        {
            get
            {
                var taxModelDictionary = cache[TaxModelCache] as Dictionary<string, TaxClassModel>;
                if (taxModelDictionary == null)
                {
                    taxModelDictionary = getTaxDictionaryQueryHandler.Search(new GetTaxClassDictionaryQuery());
                    cache.Set(TaxModelCache, taxModelDictionary, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return taxModelDictionary;
            }
        }

        public Dictionary<string, NationalClassModel> NationalClassModels
        {
            get
            {
                var nationalClassDictionary = cache[NationalClassModelCache] as Dictionary<string, NationalClassModel>;
                if (nationalClassDictionary == null)
                {
                    nationalClassDictionary = getNationalClassDictionaryQueryHandler.Search(new GetNationalClassDictionaryQuery());
                    cache.Set(NationalClassModelCache, nationalClassDictionary, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return nationalClassDictionary;
            }
        }

        public Dictionary<string, SubTeamModel> SubTeamModels
        {
            get
            {
                var subTeamsDictionary = cache[FinancialClassModelCache] as Dictionary<string, SubTeamModel>;
                if(subTeamsDictionary == null)
                {
                    subTeamsDictionary = getSubTeamModelDictionaryQueryHandler.Search(new GetSubTeamsDictionaryQuery());
                    cache.Set(FinancialClassModelCache, subTeamsDictionary, DateTime.Now.AddMinutes(cacheExpirationTimeInMinutes));
                }
                return subTeamsDictionary;
            }
        }
    }
}
