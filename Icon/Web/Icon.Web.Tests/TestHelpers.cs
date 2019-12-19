using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Icon.Web.Tests
{
    public static class TestHelpers
    {
        public static object GetDataProperty(this JsonResult json, string propertyName)
        {
            return json
                .Data
                .GetType()
                .GetProperty(propertyName)
                .GetValue(json.Data);
        }

        public static List<Locale> GetFakeLocale()
        {
            List<Locale> locales = new List<Locale>();

            Locale localeWFM = new Locale()
            {
                localeID = 1,
                localeName = "Whole Foods",
                parentLocaleID = null,
                localeTypeID = 1,
                ownerOrgPartyID = 1,
                localeOpenDate = Convert.ToDateTime("1980-09-20"),
                localeCloseDate = null,
                LocaleType = new LocaleType(),
                
            };

            Locale localeRegion = new Locale()
            {
                localeID = 2,
                localeName = "Florida",
                parentLocaleID = 1,
                localeTypeID = 2,
                ownerOrgPartyID = 1,
                localeOpenDate = null,
                localeCloseDate = null,
                LocaleType = new LocaleType()
            };

            Locale localeMetro = new Locale()
            {
                localeID = 14,
                localeName = "MET_FL",
                parentLocaleID = 2,
                localeTypeID = 3,
                ownerOrgPartyID = 1,
                localeOpenDate = Convert.ToDateTime("1980-09-21"),
                localeCloseDate = null,
                LocaleType = new LocaleType()
            };

            Locale localeStore1 = new Locale()
            {
                localeID = 91,
                localeName = "Altamonte Springs",
                parentLocaleID = 14,
                localeTypeID = 4,
                ownerOrgPartyID = 1,
                localeOpenDate = Convert.ToDateTime("1980-09-21"),
                localeCloseDate = null,
                LocaleType = new LocaleType()
            };

            Locale localeStore2 = new Locale()
            {
                localeID = 92,
                localeName = "Bayhill",
                parentLocaleID = 14,
                localeTypeID = 4,
                ownerOrgPartyID = 1,
                localeOpenDate = Convert.ToDateTime("1980-09-21"),
                localeCloseDate = Convert.ToDateTime("2013-03-11"),
                LocaleType = new LocaleType()
            };

            locales.Add(localeWFM);
            locales.Add(localeRegion);
            locales.Add(localeMetro);
            locales.Add(localeStore1);
            locales.Add(localeStore2);

            return locales;
        }

        public static Locale GetFakeLocaleWithAddress()
        {
            Timezone tz = new Timezone()
            {
                timezoneID = 111,
                timezoneCode = "XYZ",
                timezoneName = "(UTC *09:15) martian time",
                posTimeZoneName = "Mars Standard Time"
            };
            AddressType addresssType = new AddressType();
            Address a1 = new Address() { AddressType = addresssType };
            PhysicalAddress physicalAddress = new PhysicalAddress()
            {
                Address = a1,
                Timezone = tz,
                City = new City
                {
                    cityName = "Springfield",
                    County = new County
                    {
                        countyName = "Springfield"
                    }
                },
                PostalCode = new PostalCode
                {
                    postalCode = "78746"
                },
                Territory = new Territory
                {
                    territoryName = "territory",
                    territoryCode = "TT"
                },
                Country = new Country
                {
                    countryCode = "USA"
                }
            };
            Address a2 = new Address() { PhysicalAddress = physicalAddress };
            LocaleAddress localeAddress = new LocaleAddress() { Address = a2 };
            List<LocaleAddress> localeAddressCollection = new List<LocaleAddress>() { localeAddress };

            Locale localeWithAddress = new Locale()
            {
                localeID = 92,
                localeName = "Bayhill",
                parentLocaleID = 14,
                localeTypeID = 4,
                ownerOrgPartyID = 1,
                localeOpenDate = Convert.ToDateTime("1980-09-21"),
                localeCloseDate = Convert.ToDateTime("2013-03-11"),
                LocaleType = new LocaleType(),
                LocaleAddress = localeAddressCollection
            };

            return localeWithAddress;
        }

        public static List<Hierarchy> GetFakeHierarchyList()
        {
            List<Hierarchy> hierarchy = new List<Hierarchy>();
            hierarchy.Add(new Hierarchy() { hierarchyID = 1, hierarchyName = "Fake Hierarchy 1" });

            return hierarchy;
        }

        public static List<Locale> GetFakeLocaleList()
        {
            return new List<Locale>
            {
                new Locale
                {
                    localeID = 1,
                    localeName = "test",
                    localeTypeID = LocaleTypes.Metro,
                    ownerOrgPartyID = 1
                }
            };
        }

        public static Hierarchy GetFakeHierarchy()
        {
            return new Hierarchy
            {
                hierarchyID = 1,

                HierarchyClass = new List<HierarchyClass>
                {
                    new HierarchyClass
                    {
                        hierarchyID = 1,
                        hierarchyClassID = 1,
                        hierarchyLevel = 1,
                        hierarchyClassName = "TestLevel1"
                    },

                    new HierarchyClass
                    {
                        hierarchyID = 1,
                        hierarchyClassID = 2,
                        hierarchyLevel = 2,
                        hierarchyClassName = "TestLevel2"
                    },

                    new HierarchyClass
                    {
                        hierarchyID = 1,
                        hierarchyClassID = 3,
                        hierarchyLevel = 3,
                        hierarchyClassName = "TestLevel3"
                    },

                    new HierarchyClass
                    {
                        hierarchyID = 1,
                        hierarchyClassID = 4,
                        hierarchyLevel = 4,
                        hierarchyClassName = "TestLevel4",
                        Hierarchy = new Hierarchy { hierarchyID = 1, hierarchyName = HierarchyNames.Merchandise }
                    }
                }
            };
        }

        public static BulkImportNewItemModel GetFakeBulkImportNewItemModel()
        {
            return new BulkImportNewItemModel
            {
                ProductDescription = "Test New Item",
                PosDescription = "NEW ITEM",
                PackageUnit = "1",
                FoodStampEligible = "0",
                PosScaleTare = "0",
                RetailSize = "3",
                RetailUom = "CASE",
                IsValidated = "0"
            };
        }

        public static HierarchyClass GetFakeBrand()
        {
            return new HierarchyClass
            {
                hierarchyID = Hierarchies.Brands,
                hierarchyClassName = "Test Brand",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
        }

        public static HierarchyClass GetFakeMerchandise()
        {
            return new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = "Test Merchandise",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
        }

        public static HierarchyClass GetFakeTax()
        {
            return new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "Test Tax",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
        }

        public static HierarchyClass GetFakeBrowsing()
        {
            return new HierarchyClass
            {
                hierarchyID = Hierarchies.Browsing,
                hierarchyClassName = "Test Browsing",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
        }

        public static LocaleTrait MakeTrait(int traitId, string traitCode, string traitValue)
        {
            return new LocaleTrait
            {
                traitID = traitId,
                traitValue = traitValue,
                Trait = new Trait { traitID = traitId, traitCode = traitCode }
            };
        }

        public static List<LocaleTrait> MakeLocaleTraitsForLocale()
        {
            var localeTraits = new List<LocaleTrait>
            {
                MakeTrait(Traits.LocaleSubtype, TraitCodes.LocaleSubtype, "ST"),
                MakeTrait(Traits.PsBusinessUnitId, TraitCodes.PsBusinessUnitId, "11111"),
                MakeTrait(Traits.PhoneNumber, TraitCodes.PhoneNumber, "5551234565"),
                MakeTrait(Traits.ContactPerson, TraitCodes.ContactPerson, "A. Person"),
                MakeTrait(Traits.StoreAbbreviation, TraitCodes.StoreAbbreviation, "XXX"),
                MakeTrait(Traits.IrmaStoreId, TraitCodes.IrmaStoreId, "44444"),
                MakeTrait(Traits.StorePosType, TraitCodes.StorePosType, "1"),
                MakeTrait(Traits.Fax, TraitCodes.Fax, "11111111"),
                MakeTrait(Traits.InsertDate, TraitCodes.InsertDate, new DateTime(2014, 4, 24, 18, 30, 30).ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture)),
                MakeTrait(Traits.ModifiedUser, TraitCodes.ModifiedUser, "test person"),
                MakeTrait(Traits.VenueCode, TraitCodes.VenueCode, "Test Venue Code"),
                MakeTrait(Traits.VenueOccupant, TraitCodes.VenueOccupant, "Test Venue Occupant"),
                MakeTrait(Traits.TouchpointGroupId, TraitCodes.TouchpointGroupId, "Test TouchpointGroupId")
            };
            return localeTraits;
        }
    }
}
