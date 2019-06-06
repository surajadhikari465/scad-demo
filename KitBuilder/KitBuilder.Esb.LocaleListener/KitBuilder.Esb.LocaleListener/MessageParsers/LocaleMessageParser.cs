﻿using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using KitBuilder.Esb.LocaleListener.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace KitBuilder.Esb.LocaleListener.MessageParsers
{
    public class LocaleMessageParser : IMessageParser<List<LocaleModel>>
    {
        private const string PhoneNumberTraitCode = "PHN";
        private const string StoreAbbreviationTraitCode = "SAB";
		private const string BusinessUnitTraitCode = "BU";
		private const string VenueCodeTraitCode = "VNC";
		private const string VenueOccopantTraitCode = "VNO";
		private const string VenueSubTypeTraitCode = "LST";
		private const string CurrencyCodeTraitCode = "CUR";
		private const int IsoCodeForUSA = 840;
		private const int IsoCodeForCAN = 124;
		private const int IsoCodeForGBR = 826;
		private Dictionary<string, string> regionNameToCodeDictionary;
        private XmlSerializer serializer;
        private TextReader textReader;

        public LocaleMessageParser()
        {
            regionNameToCodeDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Florida",                    "FL" },
                { "Mid Atlantic",               "MA" },
                { "Mid West",                   "MW" },
                { "North Atlantic",             "NA" },
                { "Northern California",        "NC" },
                { "North East",                 "NE" },
                { "Pacific Northwest",          "PN" },
                { "Rocky Mountain",             "RM" },
                { "South",                      "SO" },
                { "Southern Pacific",           "SP" },
                { "Southwest",                  "SW" },
                { "United Kingdom",             "UK" },
                { "365_Florida",                "TS" },
                { "365_Mid Atlantic",           "TS" },
                { "365_Mid West",               "TS" },
                { "365_North Atlantic",         "TS" },
                { "365_Northern California",    "TS" },
                { "365_North East",             "TS" },
                { "365_Pacific Northwest",      "TS" },
                { "365_Rocky Mountain",         "TS" },
                { "365_South",                  "TS" },
                { "365_Southern Pacific",       "TS" },
                { "365_Southwest",              "TS" },
                { "365_United Kingdom",         "TS" }
            };
            serializer = new XmlSerializer(typeof(LocaleType));
        }

        public List<LocaleModel> ParseMessage(IEsbMessage message)
        {
            List<LocaleModel> locales = new List<LocaleModel>();
            LocaleType localeMessage;
            using (textReader = new StringReader(message.MessageText))
            {
                localeMessage = serializer.Deserialize(textReader) as LocaleType;
            }
			var chain = localeMessage.locales[0];
			foreach (var region in chain.locales)
			{
				foreach (var metro in region.locales)
				{
					foreach (var store in metro.locales)
					{
                        if (store.locales != null)
                        {
                            foreach (var venue in store.locales)
                            {
                                locales.Add(ParseLocale(chain, region, metro, store, venue));
                            }
                        }
                        else
                        {
                            locales.Add(ParseLocale(chain, region, metro, store, null));
                        }
                    }
				}
			}
			return locales;
		}

		private LocaleModel ParseLocale(LocaleType chain, LocaleType region, LocaleType metro, LocaleType store, LocaleType venue)
        {
			if (venue == null)
            {
                return ParseLocaleAsStore(chain, region, metro, store);
            }
			else
            {
                return ParseLocaleAsVenue(chain, region, metro, store, venue);
            }
		}

        private LocaleModel ParseLocaleAsVenue(LocaleType chain, LocaleType region, LocaleType metro, LocaleType store, LocaleType venue)
        {
            return new LocaleModel()
            {
                LocaleID = int.Parse(venue.id),
                LocaleName = venue.name,
                LocaleTypeID = Icon.Framework.LocaleTypes.Venue,
                StoreID = ParseIconLocaleIdFromStoreLocaleType(store),
                MetroID = int.Parse(metro?.id ?? null),
                RegionID = int.Parse(region?.id ?? null),
                ChainID = int.Parse(chain?.id ?? null),
                RegionCode = regionNameToCodeDictionary.ContainsKey(region.name)
                    ? regionNameToCodeDictionary[region.name]
                    : default(string),
                LocaleOpenDate = venue.openDateSpecified ? venue.openDate : (DateTime?) null,
                LocaleCloseDate = venue.closeDateSpecified ? venue.closeDate : (DateTime?) null,
                BusinessUnitID = null,
                StoreAbbreviation = null,
                CurrencyCode = null,
                Hospitality = string.Compare(GetTraitValue(venue, VenueSubTypeTraitCode), "Hospitality", StringComparison.OrdinalIgnoreCase) == 0
            };
        }

        private LocaleModel ParseLocaleAsStore(LocaleType chain, LocaleType region, LocaleType metro, LocaleType store)
        {
            var address = store.addresses[0].type.Item as PhysicalAddressType;

            var storeLocaleModel = new LocaleModel()
            {
                LocaleID = ParseIconLocaleIdFromStoreLocaleType(store),
                LocaleName = store.name,
                LocaleTypeID = Icon.Framework.LocaleTypes.Store,
                StoreID = null,
                MetroID = int.Parse(metro?.id ?? null),
                RegionID = int.Parse(region?.id ?? null),
                ChainID = int.Parse(chain?.id ?? null),
                RegionCode = regionNameToCodeDictionary.ContainsKey(region.name)
                    ? regionNameToCodeDictionary[region.name]
                    : default(string),
                LocaleOpenDate = store.openDateSpecified ? store.openDate : (DateTime?) null,
                LocaleCloseDate = store.closeDateSpecified ? store.closeDate : (DateTime?) null,
                BusinessUnitID = int.Parse(store.id),
                StoreAbbreviation = GetTraitValue(store, StoreAbbreviationTraitCode),
                CurrencyCode = GetTraitValue(store, CurrencyCodeTraitCode),
                Hospitality = String.Compare(GetTraitValue(store, VenueSubTypeTraitCode), "Hospitality",
                                  StringComparison.OrdinalIgnoreCase) == 0
            };

            return storeLocaleModel;
        }

        private int ParseIconLocaleIdFromStoreLocaleType(LocaleType store)
        {
            if (store.store == null)
                throw new Exception("Unable to find Store Type in the Store Locale Message. Required to parse Icon Locale id");

            var parsedLocaleId = 0;
            var validLocaleId = int.TryParse(store.store.id, out parsedLocaleId);

            if (!validLocaleId)
                throw new Exception(
                    $"Unable to parse icon locale id from Store Type in the Store Locale Message. Found: {store.store.id}");
            return parsedLocaleId;
        }

        private static string GetTraitValue(LocaleType store, string traitCode)
        {
			var trait = store.traits.FirstOrDefault(t => string.Compare(t.code,traitCode,StringComparison.OrdinalIgnoreCase) == 0 ? t.code == traitCode : t.code == null);
			return trait != null ? (trait.type.value[0].value != null ? trait.type.value[0].value : null): null;
        }
    }
}
