using Icon.ApiController.Common;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
                    case (LocaleTypes.Chain):
                        {
                            localeLineage = BuildLineageFromChain(context, parameters.LocaleId);
                            break;
                        }
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
					case (LocaleTypes.Venue):
						{
							localeLineage = BuildLineageFromVenue(context, parameters.LocaleId);
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

        private LocaleLineageModel BuildLineageFromChain(IconContext context, int localeId)
        {
            var chain = context.Locale.Single(l => l.localeID == localeId);

            var localeLineage = new LocaleLineageModel
            {
                LocaleId = chain.localeID,
                LocaleName = chain.localeName,
                DescendantLocales = new List<LocaleLineageModel>()
            };
            var regions = chain.Locale1.ToList();

            foreach (var region in regions)
            {
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
							regionLocaleLineage.DescendantLocales.Single(l => l.LocaleId == metro.localeID).DescendantLocales.Add(BuildLocaleLineageModel(store));
                        }
                    }
                }

                localeLineage.DescendantLocales.Add(regionLocaleLineage);
            }

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
                        regionLocaleLineage.DescendantLocales.Single(l => l.LocaleId == metro.localeID).DescendantLocales.Add(BuildLocaleLineageModel(store));
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
                                DescendantLocales = BuildDescendantLocales(context, metro)
                            }
                        }
                    }
                }
            };

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
                                    BuildLocaleLineageModel(store)
                                }
                            }
                        }
                    }
                }
            };

            return localeLineage;
        }

		private LocaleLineageModel BuildLineageFromVenue(IconContext context, int localeId)
		{
			var venue = context.Locale
				.Include(l => l.LocaleTrait.Select(lt => lt.Trait))
				.Single(l => l.localeID == localeId);

			var store = venue.Locale2;
			var metro = store.Locale2;
			var region = metro.Locale2;
			var chain = region.Locale2;

			var localeLineage = new LocaleLineageModel// chain
			{
				LocaleId = chain.localeID,
				LocaleName = chain.localeName,
				DescendantLocales = new List<LocaleLineageModel>
				{
					new LocaleLineageModel// region
					{
						LocaleId = region.localeID,
						LocaleName = region.localeName,
						DescendantLocales = new List<LocaleLineageModel>
						{
							new LocaleLineageModel// metro
							{
								LocaleId = metro.localeID,
								LocaleName = metro.localeName,
								DescendantLocales = new List<LocaleLineageModel>
								{
									new LocaleLineageModel//store
									{
										LocaleId = store.localeID,
										LocaleName = store.localeName,
										DescendantLocales = new List<LocaleLineageModel>
										{
											new LocaleLineageModel// venue
											{
												LocaleId = store.localeID,
												LocaleName = store.localeName,
												DescendantLocales = new List<LocaleLineageModel>
												{
													BuildVenueLocaleLineageModel(venue)
												}
											}
										}
									}
								}
							}
						}
					}
				}
			};

			return localeLineage;
		}

		private List<LocaleLineageModel> BuildDescendantLocales(IconContext context, Locale metro)
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
                        .Select(l => BuildLocaleLineageModel(l))
                        .ToList();
            }
            else
            {
                return new List<LocaleLineageModel>();
            }
        }

		private LocaleLineageModel BuildVenueLocaleLineageModel(Locale venue)
		{
			return new LocaleLineageModel
			{
				LocaleId = venue.localeID,
				LocaleName = venue.localeName,
				VenueCode = venue.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.VenueCode)?.traitValue ?? string.Empty,
				VenueOccupant = venue.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.VenueOccupant)?.traitValue ?? string.Empty,
				VenueSubType = venue.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.LocaleSubtype)?.traitValue ?? string.Empty,
				LocaleOpenDate = venue?.localeOpenDate ?? DateTime.UtcNow,
				LocaleCloseDate = venue?.localeCloseDate ?? DateTime.MaxValue
			};
		}


		private LocaleLineageModel BuildLocaleLineageModel(Locale store)
        {
			return new LocaleLineageModel
            {
                LocaleId = store.localeID,
                LocaleName = store.localeName,
				CurrencyCode = store.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.CurrencyCode)?.traitValue ?? string.Empty,
				StoreAbbreviation = store.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.StoreAbbreviation)?.traitValue?? string.Empty,
                BusinessUnitId = store.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId)?.traitValue??string.Empty,
                PhoneNumber = store.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == TraitCodes.PhoneNumber)?.traitValue?? string.Empty,
				AddressId = store.LocaleAddress.SingleOrDefault(la => la.localeID == store.localeID)?.addressID ?? 0,
				AddressUsageCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).AddressUsage?.addressUsageCode ?? string.Empty,
				AddressLine1 = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress?.addressLine1 ?? string.Empty,
				AddressLine2 = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress?.addressLine2 ?? string.Empty,
				AddressLine3 = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress?.addressLine3 ?? string.Empty,
				CityName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.City?.cityName ?? string.Empty,
				TerritoryCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Territory?.territoryCode ?? string.Empty,
				TerritoryName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Territory?.territoryName ?? string.Empty,
				CountryCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Country?.countryCode ?? string.Empty,
				CountryName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Country?.countryName ?? string.Empty,
				PostalCode = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.PostalCode?.postalCode ?? string.Empty,
				TimezoneName = store.LocaleAddress.Single(la => la.localeID == store.localeID).Address.PhysicalAddress.Timezone?.timezoneName ?? string.Empty,
				LocaleOpenDate = store?.localeOpenDate??DateTime.UtcNow,
                LocaleCloseDate = store?.localeCloseDate ?? DateTime.MaxValue
			};
        }

	}
}
