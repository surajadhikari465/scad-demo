using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Extensions;
using Icon.Infor.Listeners.Price.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Icon.Infor.Listeners.Price.Constants;

namespace Icon.Infor.Listeners.Price.Services
{
    public class DeletePricesService : IService<PriceModel>
    {
        //private IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> checkPricesQuery;
        private ICommandHandler<DeletePricesCommand> deletePricesCommandHandler;

        public DeletePricesService(
            //IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> checkPricesQuery,
            ICommandHandler<DeletePricesCommand> deletePricesCommandHandler)
        {
            //this.checkPricesQuery = checkPricesQuery;
            this.deletePricesCommandHandler = deletePricesCommandHandler;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            var deletePrices = data
                .Where(price => price.Action == Esb.Schemas.Wfm.ContractTypes.ActionEnum.Delete
                    && price.ErrorCode == null);

            if (deletePrices.Any())
            {
                IEnumerable<DbPriceModel> prices = data.ToDbPriceModel();
                //var existingPrices = this.checkPricesQuery.Search(new GetPricesByGpmIdsParameters { Prices = prices });

                //if (prices.Count() != existingPrices.Count())
                //{
                //    // get prices that were not returned in the price query
                //    var nonExistentPrices = existingPrices.ExceptBy(prices, cp => cp.GpmID);

                //    // mark error details for prices that can't be deleted because it does not exist
                //    data.Join(nonExistentPrices, d => d.GpmId, nep => nep.GpmID, (d, nep) => d)
                //        .ForEach(p =>
                //        {
                //            p.ErrorCode = Errors.Codes.DeletePricesError;
                //            p.ErrorDetails = "Price cannot be deleted because it does not exist.";
                //        });

                //    // reset price collection to only include existing prices.
                //    prices = prices.Join(existingPrices, p => p.GpmID, cp => cp.GpmID, (p, id) => p);
                //}

                // delete prices that exist in db
                deletePricesCommandHandler.Execute(new DeletePricesCommand { Prices = prices });
            }
        }
    }
}
