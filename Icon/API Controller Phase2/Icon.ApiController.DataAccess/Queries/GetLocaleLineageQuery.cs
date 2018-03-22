using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetLocaleLineageQuery : IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetLocaleLineageQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public LocaleLineageModel Search(GetLocaleLineageParameters parameters)
        {
            LocaleLineageModel localeLineage = new LocaleLineageModel();

            using (var context = iconContextFactory.CreateContext())
            {
                switch (parameters.LocaleTypeId)
                {
                    case (LocaleTypes.Region):
                        {
                            localeLineage = BuildLineageFromRegion(context, parameters.LocaleId);
                            break;
                        }
                    case (LocaleTypes.Metro):
                        {
                            localeLineage = BuildLineageFromMetro(context, parameters.LocaleId);
                            break;
                        }
                    case (LocaleTypes.Store):
                        {
                            localeLineage = BuildLineageFromStore(context, parameters.LocaleId);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException("An invalid LocaleTypeId was provided to GetLocaleLineageQuery.");
                        }
                }
            }

            return localeLineage;
        }

        private LocaleLineageModel BuildLineageFromStore(IconContext context, int localeId)
        {
            var store = context.Locale
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

        private LocaleLineageModel BuildLineageFromMetro(IconContext context, int localeId)
        {
            var metro = context.Locale.Single(l => l.localeID == localeId);
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
                                DescendantLocales = BuildStoreDescendantLocales(context, metro)
                            }
                        }
                    }
                }
            };

            return localeLineage;
        }

        private LocaleLineageModel BuildLineageFromRegion(IconContext context, int localeId)
        {
            var region = context.Locale.Single(l => l.localeID == localeId);

            var regionLocaleLineage = new LocaleLineageModel
            {
                LocaleId = region.localeID,
                LocaleName = region.localeName,
                DescendantLocales = new List<LocaleLineageModel>()
            };

            var metros = region.Locale1.ToList();

            foreach (var metro in metros)
            {
                bool metroContainsStores = context.Locale.Any(l => l.parentLocaleID == metro.localeID);

                if (metroContainsStores)
                {
                    regionLocaleLineage.DescendantLocales.Add(new LocaleLineageModel
                    {
                        LocaleId = metro.localeID,
                        LocaleName = metro.localeName,
                        DescendantLocales = new List<LocaleLineageModel>()
                    });

                    var stores = context.Locale.Where(l => l.parentLocaleID == metro.localeID)
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

        private List<LocaleLineageModel> BuildStoreDescendantLocales(IconContext context, Locale metro)
        {
            if (context.Locale.Any(l => l.parentLocaleID == metro.localeID))
            {
                return context.Locale.Where(l => l.parentLocaleID == metro.localeID)
                        .Include(l => l.LocaleTrait.Select(lt => lt.Trait))
                        .Include(l => l.LocaleAddress)
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.City))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Territory))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Country))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.PostalCode))
                        .Include(l => l.LocaleAddress.Select(la => la.Address).Select(a => a.PhysicalAddress).Select(pa => pa.Timezone))
                        .Include(l => l.LocaleAddress.Select(la => la.AddressUsage))
                        .ToList()
                        .Select(l => BuildStoreLocaleLineageModel(l))
                        .ToList();
            }
            else
            {
                return new List<LocaleLineageModel>();
            }
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
