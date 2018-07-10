using AmazonLoad.Common;
using Dapper;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Framework;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.Locale
{
    class Program
    {
        public static Serializer<Contracts.LocaleType> serializer = new Serializer<Contracts.LocaleType>();
        public static Dictionary<string, string> timeZoneDictionary = TimeZoneInfo.GetSystemTimeZones()
            .ToDictionary(tz => tz.DisplayName, tz => tz.StandardName);

        static void Main(string[] args)
        {
            if (!Directory.Exists("LocaleMessages"))
            {
                Directory.CreateDirectory("LocaleMessages");
            }
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);

            var models = sqlConnection.Query<LocaleModel>(SqlQueries.LocaleSql, buffered: false);

            var producer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            foreach (var model in models)
            {
                string message = BuildMessage(model);
                string messageId = Guid.NewGuid().ToString();

                producer.Send(
                    message,
                    messageId,
                    new Dictionary<string, string>
                    {
                        { "IconMessageID", messageId },
                        { "Source", "Icon"},
                        { "nonReceivingSysName", AppSettingsAccessor.GetStringSetting("NonReceivingSysName") }
                    });
                File.WriteAllText($"LocaleMessages/{messageId}.xml", message);
            }
        }

        private static string BuildMessage(LocaleModel model)
        {
            var localeMessage = new Contracts.LocaleType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = 0.ToString(),
                name = "Global",
                type = new Contracts.LocaleTypeType
                {
                    code = Contracts.LocaleCodeType.CMP,
                    description = Contracts.LocaleDescType.Company
                },
                locales = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = model.ChainId.ToString(),
                        name = model.ChainName,
                        type = new Contracts.LocaleTypeType
                        {
                            code = Contracts.LocaleCodeType.CHN,
                            description = Contracts.LocaleDescType.Chain
                        },
                        locales = new Contracts.LocaleType[]
                        {
                            new Contracts.LocaleType
                            {
                                Action = Contracts.ActionEnum.AddOrUpdate,
                                ActionSpecified = true,
                                id = model.RegionId.ToString(),
                                name = model.RegionName,
                                type = new Contracts.LocaleTypeType
                                {
                                    code = Contracts.LocaleCodeType.REG,
                                    description = Contracts.LocaleDescType.Region
                                },
                                locales = new Contracts.LocaleType[]
                                {
                                    new Contracts.LocaleType
                                    {
                                        Action = Contracts.ActionEnum.AddOrUpdate,
                                        ActionSpecified = true,
                                        id = model.MetroId.ToString(),
                                        name = model.MetroName,
                                        type = new Contracts.LocaleTypeType
                                        {
                                            code = Contracts.LocaleCodeType.MTR,
                                            description = Contracts.LocaleDescType.Metro
                                        },
                                        locales = model.StoreId == null ? null : new Contracts.LocaleType[]
                                        {
                                            new Contracts.LocaleType
                                            {
                                                Action = Contracts.ActionEnum.AddOrUpdate,
                                                ActionSpecified = true,
                                                id = model.BusinessUnit,
                                                name = model.StoreName,
                                                type = new Contracts.LocaleTypeType
                                                {
                                                    code = Contracts.LocaleCodeType.STR,
                                                    description = Contracts.LocaleDescType.Store
                                                },
                                                addresses = new Contracts.AddressType[]
                                                {
                                                    CreateLocaleAddress(model)
                                                },
                                                traits = CreateLocaleTraits(model)
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return serializer.Serialize(localeMessage);
        }

        private static Contracts.TraitType[] CreateLocaleTraits(LocaleModel model)
        {
            return new Contracts.TraitType[]
            {
                new Contracts.TraitType
                {
                    code = TraitCodes.StoreAbbreviation,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.StoreAbbreviation,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = model.StoreAbbreviation,
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PhoneNumber,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PhoneNumber,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = model.PhoneNumber,
                            }
                        }
                    }
                }
            };
        }

        private static Contracts.AddressType CreateLocaleAddress(LocaleModel model)
        {
            return new Contracts.AddressType
            {
                id = model.AddressId,
                idSpecified = true,
                type = new Contracts.AddressTypeType
                {
                    code = AddressTypeCodes.PhysicalAddress,
                    description = Contracts.AddressDescType.physical,
                    ItemElementName = Contracts.ItemChoiceType.physical,
                    Item = new Contracts.PhysicalAddressType
                    {
                        addressLine1 = (model.AddressLine1 + " " + model.AddressLine2 + " " + model.AddressLine3).Trim(),
                        cityType = new Contracts.CityType
                        {
                            name = model.City
                        },
                        territoryType = new Contracts.TerritoryType
                        {
                            code = model.TerritoryAbbrev,
                            name = model.Territory
                        },
                        country = new Contracts.CountryType
                        {
                            code = model.CountryAbbrev,
                            name = model.Country
                        },
                        postalCode = model.PostalCode,
                        timezone = new Contracts.TimezoneType
                        {
                            // ESB is expecting the Time Zone standard name to be in the timezoneCode element.
                            code = model.Timezone.Contains("(UTC) Dublin, Edinburgh, Lisbon, London") ? timeZoneDictionary["(UTC+00:00) Dublin, Edinburgh, Lisbon, London"] : timeZoneDictionary[model.Timezone],
                            name = GetTimezoneName(model.Timezone)
                        }
                    }
                },
                usage = new Contracts.AddressUsageType
                {
                    code = "SHP",
                    description = Contracts.AddressUsgDescType.street
                }
            };
        }

        private static Contracts.TimezoneNameType GetTimezoneName(string timezoneName)
        {
            switch (timezoneName)
            {
                case "(UTC-10:00) Hawaii":
                    return Contracts.TimezoneNameType.USHawaii;
                case "(UTC-08:00) Pacific Time (US & Canada)":
                    return Contracts.TimezoneNameType.USPacific;
                case "(UTC-07:00) Mountain Time (US & Canada)":
                    return Contracts.TimezoneNameType.USMountain;
                case "(UTC-06:00) Central Time (US & Canada)":
                    return Contracts.TimezoneNameType.USCentral;
                case "(UTC-05:00) Eastern Time (US & Canada)":
                    return Contracts.TimezoneNameType.USEastern;
                case "(UTC-04:00) Atlantic Time (Canada)":
                    return Contracts.TimezoneNameType.CanadaAtlantic;
                case "(UTC) Dublin, Edinburgh, Lisbon, London":
                    return Contracts.TimezoneNameType.GMT;
                default:
                    return Contracts.TimezoneNameType.USCentral;
            }
        }
    }
}
