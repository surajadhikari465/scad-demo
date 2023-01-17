using Icon.Esb.Schemas.Mammoth;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IrmaPriceListenerService.DataAccess
{
    public interface IIrmaPriceDAL
    {
        void LoadMammothPricesBatch(IList<MammothPriceType> mammothPrices, string transactionId);
        void LoadMammothPricesSingle(MammothPriceType mammothPrice, string transactionId);
        void UpdateIrmaPrice(string transactionId);
        void DeleteStagedMammothPrices(string transactionId);
    }
}
