using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Mammoth.Esb.LocaleListener.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Mammoth.Esb.LocaleListener.MessageParsers
{
    public class LocaleMessageParser : IMessageParser<List<LocaleModel>>
    {
        private const string PhoneNumberTraitCode = "PHN";
        private const string StoreAbbreviationTraitCode = "SAB";
        private Dictionary<string, string> regionNameToCodeDictionary;
        private XmlSerializer serializer;
        private TextReader textReader;

        public LocaleMessageParser()
        {
            regionNameToCodeDictionary = new Dictionary<string, string>
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
            var regions = chain.locales;
            foreach (var region in regions)
            {
                var metros = region.locales;
                foreach (var metro in metros)
                {
                    var stores = metro.locales;
                    foreach (var store in stores)
                    {
                        locales.Add(ParseStore(region, store));
                    }
                }
            }

            return locales;
        }

        private LocaleModel ParseStore(LocaleType region, LocaleType store)
        {
            var address = store.addresses[0].type.Item as PhysicalAddressType;

            return new LocaleModel
            {
                Region = regionNameToCodeDictionary[region.name],
                StoreName = store.name,
                BusinessUnitID = int.Parse(store.id),
                StoreAbbrev = GetTraitValue(store, StoreAbbreviationTraitCode),
                PhoneNumber = GetTraitValue(store, PhoneNumberTraitCode),
                Address1 = address.addressLine1,
                Address2 = address.addressLine2,
                Address3 = address.addressLine3,
                City = address.cityType.name,
                Country = address.country.name,
                CountryAbbrev = address.country.code,
                PostalCode = address.postalCode,
                Territory = address.territoryType.name,
                TerritoryAbbrev = address.territoryType.code,
                Timezone = address.timezone.code
            };
        }

        private static string GetTraitValue(LocaleType store, string traitCode)
        {
            var trait = store.traits.FirstOrDefault(t => t.code == traitCode);
            if (trait != null)
                return trait.type.value[0].value;
            else
                return null;
        }
    }
}
