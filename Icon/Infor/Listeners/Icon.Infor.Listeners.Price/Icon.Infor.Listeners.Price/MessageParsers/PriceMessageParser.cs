using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Extensions;
using Icon.Infor.Listeners.Price.Models;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.ContractTypes;


namespace Icon.Infor.Listeners.Price.MessageParsers
{
    public class PriceMessageParser : MessageParserBase<Contracts.items, IEnumerable<PriceModel>>
    {
        private ILogger<PriceMessageParser> logger;
        private IQueryHandler<GetCurrenciesParameters, IEnumerable<Currency>> getCurrenciesQuery;
        private IQueryHandler<GetLocalesByBusinessUnitsParameters, IEnumerable<Locale>> getLocalesQuery;

        private Dictionary<int, string> businessUnitToRegionDictionary;
        private Dictionary<string, int> currencyCodeToId;
        private DateTime now;

        public PriceMessageParser(ILogger<PriceMessageParser> logger,
            IQueryHandler<GetCurrenciesParameters, IEnumerable<Currency>> getCurrenciesQuery,
            IQueryHandler<GetLocalesByBusinessUnitsParameters, IEnumerable<Locale>> getLocalesQuery)
        {
            this.logger = logger;
            this.getCurrenciesQuery = getCurrenciesQuery;
            this.getLocalesQuery = getLocalesQuery;

            this.businessUnitToRegionDictionary = new Dictionary<int, string>();
            this.currencyCodeToId = new Dictionary<string, int>();
            this.now = DateTime.Now;
        }

        public override IEnumerable<PriceModel> ParseMessage(IEsbMessage message)
        {
            this.logger.Info($"Deserializing message: {message}");
            var contract = base.DeserializeMessage(message);

            PopulateDictionaryHelpers(contract.item);

            List<PriceModel> prices = contract.item
                .Select(i => CreatePriceModel(i))
                .ToList();

            return prices;
        }

        private PriceModel CreatePriceModel(Contracts.ItemType item)
        {
            var locale = item.locale.First();
            var storeItemAttributes = locale.Item as Contracts.StoreItemAttributesType;
            var esbPrice = storeItemAttributes.prices.First(p => p.Action == Contracts.ActionEnum.Add || p.Action == Contracts.ActionEnum.AddOrUpdate);

            var replacePrice = locale.Action == Contracts.ActionEnum.Replace 
                ? storeItemAttributes.prices.FirstOrDefault(p => p.Action == Contracts.ActionEnum.Delete)
                : null;

            PriceModel price = new PriceModel();
            price.Action = locale.Action;
            price.BusinessUnitId = int.Parse(locale.id);
            price.CurrencyCode = esbPrice.currencyTypeCode.ToString();
            price.CurrencyId = currencyCodeToId[esbPrice.currencyTypeCode.ToString()];
            price.EndDate = esbPrice.priceEndDate == default(DateTime) ? default(DateTime?) : esbPrice.priceEndDate;
            price.GpmId = Guid.Parse(esbPrice.gpmId);
            price.ItemId = item.id;
            price.Multiple = esbPrice.priceMultiple;
            price.NewTagExpiration = GetTraitValue("NTE", esbPrice.traits).ParseToNullableDateTime(); //TODO:  update when we know the trait code.
            price.Price = esbPrice.priceAmount.amount;
            price.PriceType = esbPrice.type.id;
            price.PriceTypeAttribute = esbPrice.type.type.id;
            price.ReplacedGpmId = replacePrice == null ? default(Guid?) : Guid.Parse(replacePrice.gpmId);
            price.SellableUom = esbPrice.uom.code.ToString();
            price.StartDate = esbPrice.priceStartDate;
            price.Region = businessUnitToRegionDictionary[price.BusinessUnitId];
            price.AddedDate = this.now;

            return price;
        }

        private void PopulateDictionaryHelpers(Contracts.ItemType[] items)
        {
            IEnumerable<int> businessUnits = items.Select(i => int.Parse(i.locale.First().id));

            IEnumerable<Currency> currencies = getCurrenciesQuery.Search(new GetCurrenciesParameters());
            IEnumerable<Locale> locales = getLocalesQuery.Search(new GetLocalesByBusinessUnitsParameters { BusinessUnitIDs = businessUnits });

            this.businessUnitToRegionDictionary = locales.ToDictionary(l => l.BusinessUnitID, r => r.Region);
            this.currencyCodeToId = currencies.ToDictionary(c => c.CurrencyCode, i => i.CurrencyID);
        }

        private string GetTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            var trait = traits?.FirstOrDefault(t => t.code == traitCode);

            if (trait != null)
            {
                return trait.type.value.First().value;
            }

            return null;
        }

        private Contracts.PriceTypeIdType GetPriceTypeAttribute(Contracts.PriceTypeType priceType)
        {
            if (priceType.type == null)
            {

            }
            var priceTypeAttribute = priceType.type.id;

            return Contracts.PriceTypeIdType.EDV;
        }
    }
}
