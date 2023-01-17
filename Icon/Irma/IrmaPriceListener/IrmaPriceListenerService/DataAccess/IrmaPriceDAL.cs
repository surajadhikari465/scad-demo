using Icon.Esb.Schemas.Mammoth;
using System;
using System.Collections.Generic;

namespace IrmaPriceListenerService.DataAccess
{
    public class IrmaPriceDAL : IIrmaPriceDAL
    {
        public void DeleteStagedMammothPrices(string transactionId)
        {
            throw new NotImplementedException();
        }

        public void LoadMammothPricesBatch(IList<MammothPriceType> mammothPrices, string transactionId)
        {
            throw new NotImplementedException();
        }

        public void LoadMammothPricesSingle(MammothPriceType mammothPrice, string transactionId)
        {
            throw new NotImplementedException();
        }

        public void UpdateIrmaPrice(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
