using Esb.Core.MessageBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.MessageBuilders
{
    public class PriceRefreshMessageBuilder : IMessageBuilder<List<GpmPrice>>
    {
        private const string AddOrUpdateAction = "AddOrUpdate";

        public string BuildMessage(List<GpmPrice> request)
        {
            XDocument mammothPrices = new XDocument(
                new XElement(
                    XName.Get("MammothPrices", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"),
                    request
                        .Where(p => !p.EndDate.HasValue)
                        .Select(p => new XElement(
                            XName.Get("MammothPrice", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"),
                            new XElement(XName.Get("Region", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.Region),
                            new XElement(XName.Get("BusinessUnit", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.BusinessUnitId),
                            new XElement(XName.Get("ItemId", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.ItemId),
                            new XElement(XName.Get("GpmId", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.GpmId),
                            new XElement(XName.Get("Multiple", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.Multiple),
                            new XElement(XName.Get("Price", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.Price),
                            new XElement(XName.Get("StartDate", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.StartDate),
                            new XElement(XName.Get("PriceType", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.PriceType),
                            new XElement(XName.Get("PriceTypeAttribute", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.PriceTypeAttribute),
                            new XElement(XName.Get("SellableUom", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.SellableUOM),
                            new XElement(XName.Get("CurrencyCode", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.CurrencyCode),
                            new XElement(XName.Get("Action", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), AddOrUpdateAction),
                            new XElement(XName.Get("ItemTypeCode", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.ItemTypeCode),
                            new XElement(XName.Get("StoreName", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.StoreName),
                            new XElement(XName.Get("ScanCode", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.ScanCode)))
                        .Concat(request
                                    .Where(p => p.EndDate.HasValue)
                                    .Select(p => new XElement(
                                        XName.Get("MammothPrice", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"),
                                        new XElement(XName.Get("Region", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.Region),
                                        new XElement(XName.Get("BusinessUnit", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.BusinessUnitId),
                                        new XElement(XName.Get("ItemId", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.ItemId),
                                        new XElement(XName.Get("GpmId", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.GpmId),
                                        new XElement(XName.Get("Multiple", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.Multiple),
                                        new XElement(XName.Get("Price", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.Price),
                                        new XElement(XName.Get("StartDate", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.StartDate),
                                        new XElement(XName.Get("EndDate", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.EndDate),
                                        new XElement(XName.Get("PriceType", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.PriceType),
                                        new XElement(XName.Get("PriceTypeAttribute", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.PriceTypeAttribute),
                                        new XElement(XName.Get("SellableUom", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.SellableUOM),
                                        new XElement(XName.Get("CurrencyCode", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.CurrencyCode),
                                        new XElement(XName.Get("Action", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), AddOrUpdateAction),
                                        new XElement(XName.Get("ItemTypeCode", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.ItemTypeCode),
                                        new XElement(XName.Get("StoreName", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.StoreName),
                                        new XElement(XName.Get("ScanCode", "http://schemas.wfm.com/Mammoth/MammothPrices.xsd"), p.ScanCode))))));
            return mammothPrices.ToString();
        }
    }
}