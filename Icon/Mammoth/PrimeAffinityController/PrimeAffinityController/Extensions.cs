using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Newtonsoft.Json;
using PrimeAffinityController.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeAffinityController
{
    public static class Extensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static IEnumerable<PrimeAffinityMessageModel> ToPrimeAffinityModels(this IEnumerable<PrimeAffinityPsgPriceModel> prices)
        {
            return prices
                .Select(p => new PrimeAffinityMessageModel
                {
                    BusinessUnitID = p.BusinessUnitID,
                    InternalPriceObject = p,
                    ItemID = p.ItemID,
                    ItemTypeCode = p.ItemTypeCode,
                    MessageAction = (ActionEnum) Enum.Parse(typeof(ActionEnum), p.MessageAction, true),
                    Region = p.Region,
                    ScanCode = p.ScanCode,
                    StoreName = p.StoreName,
                });
        }
    }
}
