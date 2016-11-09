using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetLocaleLineageQuery : IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetLocaleLineageQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public LocaleLineageModel Search(GetLocaleLineageParameters parameters)
        {
            LocaleLineageModel localeLineage = new LocaleLineageModel();

            switch (parameters.LocaleTypeId)
            {
                case (LocaleTypes.Region):
                    {
                        localeLineage = BuildLineageFromRegion(parameters.LocaleId);
                        break;
                    }
                case (LocaleTypes.Metro):
                    {
                        localeLineage = BuildLineageFromMetro(parameters.LocaleId);
                        break;
                    }
                case (LocaleTypes.Store):
                    {
                        localeLineage = BuildLineageFromStore(parameters.LocaleId);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("An invalid LocaleTypeId was provided to GetLocaleLineageQuery.");
                    }
            }

            return localeLineage;
        }

        private LocaleLineageModel BuildLineageFromStore(int localeId)
        {
            var store = globalContext.Context.Locale
                .Include(l => l.LocaleTrait.Select(lt => lt.Trait))
                .Include(l => l.LocaleAddress)
                .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.City))
                .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Territory))
                .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Country))
                .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.PostalCode))
                .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Timezone))
                .Include(l => l.LocaleAddress.Select(la => la.AddressUsage))
                .Single(l => l.localeID == localeId);

            var metro = store.Locale2;
            var region = metro.Locale2;
            var chain = region.Locale2;

            var localeLineage = new LocaleLineageModel
            {
                LocaleId = chain.localeID,
                LocaleName = chain.localeName,
                DescendantLocales = new List<LocaleLineageModel>
                {
                    new LocaleLineageModel
                    {
                        LocaleId = region.localeID,
                        LocaleName = region.localeName,
                        DescendantLocales = new List<LocaleLineageModel>
                        {
                            new LocaleLineageModel
                            {
                                LocaleId = metro.localeID,
                                LocaleName = metro.localeName,
                                DescendantLocales = new List<LocaleLineageModel>
                                {
                                    BuildStoreLocaleLineageModel(store)
                                }
                            }
                        }
                    }
                }
            };

            return localeLineage;
        }

        private LocaleLineageModel BuildLineageFromMetro(int localeId)
        {
            var metro = globalContext.Context.Locale.Single(l => l.localeID == localeId);
            var region = metro.Locale2;
            var chain = region.Locale2;

            var localeLineage = new LocaleLineageModel
            {
                LocaleId = chain.localeID,
                LocaleName = chain.localeName,
                DescendantLocales = new List<LocaleLineageModel>
                {
                    new LocaleLineageModel
                    {
                        LocaleId = region.localeID,
                        LocaleName = region.localeName,
                        DescendantLocales = new List<LocaleLineageModel>
                        {
                            new LocaleLineageModel
                            {
                                LocaleId = metro.localeID,
                                LocaleName = metro.localeName,
                                DescendantLocales = new List<LocaleLineageModel>()
                            }
                        }
                    }
                }
            };

            return localeLineage;
        }

        private LocaleLineageModel BuildLineageFromRegion(int localeId)
        {
            var region = globalContext.Context.Locale.Single(l => l.localeID == localeId);

            var regionLocaleLineage = new LocaleLineageModel
            {
                LocaleId = region.localeID,
                LocaleName = region.localeName,
                DescendantLocales = new List<LocaleLineageModel>()
            };

            var metros = region.Locale1.ToList();

            foreach (var metro in metros)
            {
                bool metroContainsStores = globalContext.Context.Locale.Any(l => l.parentLocaleID == metro.localeID);

                if (metroContainsStores)
                {
                    regionLocaleLineage.DescendantLocales.Add(new LocaleLineageModel
                    {
                        LocaleId = metro.localeID,
                        LocaleName = metro.localeName,
                        DescendantLocales = new List<LocaleLineageModel>()
                    });

                    var stores = globalContext.Context.Locale.Where(l => l.parentLocaleID == metro.localeID)
                        .Include(l => l.LocaleTrait.Select(lt => lt.Trait))
                        .Include(l => l.LocaleAddress)
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.City))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Territory))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Country))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.PostalCode))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Timezone))
                        .Include(l => l.LocaleAddress.Select(la => la.AddressUsage))
                        .ToList();

                    foreach (var store in stores)
                    {
                        regionLocaleLineage.DescendantLocales.Single(l => l.LocaleId == metro.localeID).DescendantLocales.Add(BuildStoreLocaleLineageModel(store));
                    }
                }
            }

            var chain = region.Locale2;

            var localeLineage = new LocaleLineageModel
            {
                LocaleId = chain.localeID,
                LocaleName = chain.localeName,
                DescendantLocales = new List<LocaleLineageModel> { regionLocaleLineage }
            };

            return localeLineage;
        }

        private LocaleLineageModel BuildStoreLocaleLineageModel(Locale store)
        {
            return new LocaleLineageModel
            {
                LocaleId = store.localeID,
                LocaleName = store.localeName,
                StoreAbbreviation = store.LocaleTrait.Single(lt => lt.Trait.traitCode == TraitCodes.StoreAbbreviation).traitValue,
                BusinessUnitId = store.LocaleTrait.Single(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId).traitValue,
                PhoneNumber = store.LocaleTrait.Single(lt => lt.Trait.traitCode == TraitCodes.PhoneNumber).traitValue,
                AddressId = store.LocaleAddress.Single(la => la.localeID == store.localeID).addressID,
                AddressUsageCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).AddressUsage.addressUsageCode,
                AddressLine1 = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.addressLine1,
                AddressLine2 = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.addressLine2,
                AddressLine3 = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.addressLine3,
                CityName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.City.cityName,
                TerritoryCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Territory.territoryCode,
                TerritoryName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Territory.territoryName,
                CountryCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Country.countryCode,
                CountryName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Country.countryName,
                PostalCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.PostalCode.postalCode,
                TimezoneName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Timezone.timezoneName
            };
        }
    }
}
