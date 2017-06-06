using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.LocaleListener.Models;
using Icon.Logging;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.LocaleListener.MessageParsers
{
    public class LocaleMessageParser : MessageParserBase<Contracts.LocaleType, LocaleModel>
    {
        private ILogger<LocaleMessageParser> logger;

        public LocaleMessageParser(ILogger<LocaleMessageParser> logger)
        {
            this.logger = logger;
        }

        public override LocaleModel ParseMessage(IEsbMessage message)
        {
            Contracts.LocaleType localeMessage = base.DeserializeMessage(message);

            LocaleModel model = new LocaleModel();
            model.Name = localeMessage.name;
            model.TypeCode = localeMessage.type.code.ToString();
            model.Locales = localeMessage.locales.Select(l => BuildChain(l, int.Parse(localeMessage.id)));
            model.Action = localeMessage.Action;

            return model;
        }

        private LocaleModel BuildChain(Contracts.LocaleType localeMessage, int parentLocaleId)
        {
            LocaleModel model = new LocaleModel();
            model.Name = localeMessage.name;
            model.TypeCode = localeMessage.type.code.ToString();
            model.LocaleId = int.Parse(localeMessage.id);
            model.Locales = localeMessage.locales.Select(l => BuildRegion(l, int.Parse(localeMessage.id)));
            model.Action = localeMessage.Action;
            return model;
        }

        private LocaleModel BuildRegion(Contracts.LocaleType localeMessage, int parentLocaleId)
        {
            LocaleModel model = new LocaleModel();
            model.Name = localeMessage.name;
            model.TypeCode = localeMessage.type.code.ToString();
            model.LocaleId = int.Parse(localeMessage.id);
            model.ParentLocaleId = parentLocaleId;
            model.Locales = localeMessage.locales.Select(l => BuildMetro(l, int.Parse(localeMessage.id)));
            model.Action = localeMessage.Action;
            return model;
        }

        private LocaleModel BuildMetro(Contracts.LocaleType localeMessage, int parentLocaleId)
        {
            LocaleModel model = new LocaleModel();
            model.Name = localeMessage.name;
            model.TypeCode = localeMessage.type.code.ToString();
            model.LocaleId = int.Parse(localeMessage.id);
            model.ParentLocaleId = parentLocaleId;
            model.Locales = localeMessage.locales.Select(l => BuildStore(l, int.Parse(localeMessage.id)));
            model.Action = localeMessage.Action;
            return model;
        }

        private LocaleModel BuildStore(Contracts.LocaleType localeMessage, int parentLocaleId)
        {
            LocaleModel model = new LocaleModel();
            var address = localeMessage.addresses[0].type.Item as Contracts.PhysicalAddressType;

            model.Name = localeMessage.name;
            model.TypeCode = localeMessage.type.code.ToString();
            model.LocaleId = 0;
            model.BusinessUnitId = int.Parse(localeMessage.id);
            model.Address.BusinessUnitId = model.BusinessUnitId;
            model.ParentLocaleId = parentLocaleId;
            model.OpenDate = localeMessage.openDate;
            model.CloseDate = localeMessage.closeDate;
            model.Address.AddressId = localeMessage.addresses[0].id;
            model.Address.AddressLine1 = address.addressLine1;
            model.Address.AddressLine2 = address.addressLine2;
            model.Address.AddressLine3 = address.addressLine3;
            model.Address.CityName = address.cityType.name;
            model.Address.PostalCode = address.postalCode;
            model.Address.Country = address.country.name;
            model.Address.TerritoryCode = address.territoryType.code;
            model.Address.TimeZoneName = address.timezone.code; //The name of the timezone is actually in the timezone code of the message
            model.Address.Latitude = address.latitude;
            model.Address.Longitude = address.longitude;
            model.EwicAgency = GetTraitValue(localeMessage.traits, Traits.Codes.EwicAgency);
            model.Action = localeMessage.Action;

            model.LocaleTraits = localeMessage.traits.SelectMany(lm => new[] {
                new LocaleTraitModel(Traits.PhoneNumber,GetTraitValue(localeMessage.traits, Traits.Codes.PhoneNumber), null, model.BusinessUnitId),
                new LocaleTraitModel(Traits.Fax,GetTraitValue(localeMessage.traits, Traits.Codes.Fax),null,model.BusinessUnitId),
                new LocaleTraitModel(Traits.ContactPerson,GetTraitValue(localeMessage.traits, Traits.Codes.ContactPerson), null,model.BusinessUnitId),
                new LocaleTraitModel(Traits.PsBusinessUnitId, GetTraitValue(localeMessage.traits, Traits.Codes.PsBusinessUnitId),  null,model.BusinessUnitId),
                new LocaleTraitModel(Traits.IrmaStoreId, GetTraitValue(localeMessage.traits, Traits.Codes.IrmaStoreId),  null,model.BusinessUnitId),
                new LocaleTraitModel(Traits.StorePosType,GetTraitValue(localeMessage.traits, Traits.Codes.StorePosType),  null,model.BusinessUnitId),
                new LocaleTraitModel(Traits.StoreAbbreviation,GetTraitValue(localeMessage.traits, Traits.Codes.StoreAbbreviation), null,model.BusinessUnitId)
            });

            return model;
        }

        private static string GetTraitValue(Contracts.TraitType[] traits, string traitCode, string defaultValue = "")
        {
            var traitValue = traits.FirstOrDefault(i => i.code == traitCode);

            if (traitValue == null || string.IsNullOrWhiteSpace(traitValue.type.value.First().value))
                return defaultValue;
            else
                return traitValue.type.value.First().value;
        }
    }
}
