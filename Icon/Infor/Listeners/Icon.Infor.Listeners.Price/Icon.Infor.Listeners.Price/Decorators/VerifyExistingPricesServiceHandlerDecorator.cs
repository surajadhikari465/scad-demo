using Icon.Esb.Schemas.Wfm.ContractTypes;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Extensions;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Decorators
{
    public class VerifyExistingPricesServiceHandlerDecorator : IPriceService<PriceModel>
    {
        private IPriceService<PriceModel> priceService;
        private IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> checkPricesQuery;

        public VerifyExistingPricesServiceHandlerDecorator(
            IPriceService<PriceModel> priceService,
            IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> checkPricesQuery)
        {
            this.priceService = priceService;
            this.checkPricesQuery = checkPricesQuery;
        }

        public void Add(IEnumerale<PriceModel> prices)
        {
            if(HasErrors(prices))
                ValidateExistingPrices(ActionEnum.Add, prices, prices.ToDbPriceModel());
        }

        public void Delete(IEnumerable<PriceModel> prices)
        {
            throw new NotImplementedException();
        }

        public void Replace(IEnumerable<PriceModel> prices)
        {
            throw new NotImplementedException();
        }

        private bool HasErrors(IEnumerable<PriceModel> prices)
        {
            return !string.IsNullOrEmpty(prices.First().ErrorCode);
        }

        private void ValidateExistingPrices(ActionEnum action, IEnumerable<PriceModel> messageData, IEnumerable<DbPriceModel> dbPricesToCheck)
        {
            IEnumerable<DbPriceModel> pricesInDb = null;
            IEnumerable<DbPriceModel> errantPrices = null;
            string errorDetails = null;

            GetPricesByGpmIdsParameters queryParameters = new GetPricesByGpmIdsParameters { Prices = dbPricesToCheck };
            pricesInDb = this.checkPricesQuery.Search(queryParameters);

            if (dbPricesToCheck.Count() != pricesInDb.Count() &&
                (action == ActionEnum.Delete || action == ActionEnum.Replace))
            {
                errantPrices = dbPricesToCheck.ExceptBy(pricesInDb, ptp => ptp.GpmID);
                errorDetails = "Price cannot be deleted because it does not exist.";
            }

            if (dbPricesToCheck.Count() == pricesInDb.Count() && action == ActionEnum.Add)
            {
                errantPrices = pricesInDb;
                errorDetails = "Price cannot be added because it already exists.";
            }

            messageData.Join(errantPrices, d => d.GpmId, nep => nep.GpmID, (d, nep) => d)
                .ForEach(p =>
                {
                    p.ErrorCode = $"{action.ToString()}PriceError";
                    p.ErrorDetails = errorDetails;
                });
        }
    }
}
