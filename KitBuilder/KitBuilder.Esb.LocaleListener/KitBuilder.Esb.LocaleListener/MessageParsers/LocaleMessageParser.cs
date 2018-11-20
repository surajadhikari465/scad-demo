using Icon.Esb.MessageParsers;
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
						if (store.addresses == null)
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
				var address = store.addresses[0].type.Item as PhysicalAddressType;

				return new LocaleModel()
				{
					LocaleID = int.Parse(store.id),
					LocaleName = store.name,
					LocaleTypeID = Icon.Framework.LocaleTypes.Store,
					StoreID = null,
					MetroID = int.Parse(metro?.id ?? null),
					RegionID = int.Parse(region?.id ?? null),
					ChainID = int.Parse(chain?.id ?? null),
					RegionCode = regionNameToCodeDictionary.ContainsKey(region.name) ? regionNameToCodeDictionary[region.name] : default(string),
					LocaleOpenDate = store.openDateSpecified ? store.openDate : (DateTime?)null,
					LocaleCloseDate = store.closeDateSpecified ? store.closeDate : (DateTime?)null,
					BusinessUnitID = int.Parse(store.id),
					StoreAbbreviation = GetTraitValue(store, StoreAbbreviationTraitCode),
					CurrencyCode = GetTraitValue(store, CurrencyCodeTraitCode),
					Hospitality = String.Compare(GetTraitValue(store, VenueSubTypeTraitCode), "Hospitality", true) == 0
			};
			}
			else
			{
				return new LocaleModel()
				{
					LocaleID = int.Parse(venue.id),
					LocaleName = venue.name,
					LocaleTypeID = Icon.Framework.LocaleTypes.Venue,
					StoreID = int.Parse(store?.id ?? null),
					MetroID = int.Parse(metro?.id ?? null),
					RegionID = int.Parse(region?.id ?? null),
					ChainID = int.Parse(chain?.id ?? null),
					RegionCode = regionNameToCodeDictionary.ContainsKey(region.name) ? regionNameToCodeDictionary[region.name] : default(string),
					LocaleOpenDate = venue.openDateSpecified ? venue.openDate : (DateTime?)null,
					LocaleCloseDate = venue.closeDateSpecified ? venue.closeDate : (DateTime?)null,
					BusinessUnitID = null,
					StoreAbbreviation = null,
					CurrencyCode = null,
					Hospitality = String.Compare(GetTraitValue(venue, VenueSubTypeTraitCode), "Hospitality", true) == 0
				};
			}
		}

		private static string GetTraitValue(LocaleType store, string traitCode)
        {
			var trait = store.traits.FirstOrDefault(t => String.Compare(t.code,traitCode,true) == 0 ? t.code == traitCode : t.code == null);
			return trait != null ? (trait.type.value[0].value != null ? trait.type.value[0].value : null): null;
        }
    }
}
