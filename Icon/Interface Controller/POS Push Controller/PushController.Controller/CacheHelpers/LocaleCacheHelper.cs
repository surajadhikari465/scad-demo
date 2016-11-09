using Icon.Framework;
using PushController.Common;
using PushController.Common.Exceptions;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushController.Controller.CacheHelpers
{
    public class LocaleCacheHelper : ICacheHelper<int, Locale>
    {
        private IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler;

        public LocaleCacheHelper(IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler)
        {
            this.getLocaleQueryHandler = getLocaleQueryHandler;
        }

        public void Populate(List<int> businessUnitsToCache)
        {
            var cachedBusinessUnits = Cache.businessUnitToLocale.Select(cache => cache.Key).ToList();
            var nonCachedBusinessUnits = businessUnitsToCache.Except(cachedBusinessUnits).ToList();

            if (nonCachedBusinessUnits.Count > 0)
            {
                var query = new GetLocalesByBusinessUnitIdQuery
                {
                    BusinessUnits = nonCachedBusinessUnits.Select(bu => bu.ToString()).ToList()
                };

                var locales = getLocaleQueryHandler.Execute(query);

                foreach (var locale in locales)
                {
                    string businessUnitId = locale.LocaleTrait.Single(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId).traitValue;
                    Cache.businessUnitToLocale.Add(Int32.Parse(businessUnitId), locale);
                }
            }
        }

        public Locale Retrieve(int businessUnitId)
        {
            Locale locale;
            if (Cache.businessUnitToLocale.TryGetValue(businessUnitId, out locale))
            {
                return locale;
            }
            else
            {
                throw new BusinessUnitNotFoundException(String.Format("Business Unit ID {0} was not found in Icon.", businessUnitId));
            }
        }
    }
}
