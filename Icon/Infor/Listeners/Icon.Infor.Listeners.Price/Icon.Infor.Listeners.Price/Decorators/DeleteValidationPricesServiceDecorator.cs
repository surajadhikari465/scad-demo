using Icon.Esb.Schemas.Wfm.ContractTypes;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
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

namespace Icon.Infor.Listeners.Price.Decorators
{
    public class DeleteValidationPricesServiceDecorator : IService<PriceModel>
    {
        private IService<PriceModel> service;
        private IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> getPricesQuery;
        private IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>> getDeletedPricesQuery;

        public DeleteValidationPricesServiceDecorator(
            IService<PriceModel> service,
            IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> getPricesQuery,
            IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>> getDeletedPricesQuery)
        {
            this.service = service;
            this.getPricesQuery = getPricesQuery;
            this.getDeletedPricesQuery = getDeletedPricesQuery;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            var prices = data.Where(price => price.Action == ActionEnum.Delete);

            if (prices.Any())
            {
                // Check Price table if prices are available to delete
                List<DbPriceModel> deletePrices = data.ToDbPriceModel().ToList();
                var existingPrices = this.getPricesQuery.Search(new GetPricesByGpmIdsParameters { Prices = deletePrices }).ToList();

                if (deletePrices.Count != existingPrices.Count)
                {
                    // Check DeletedPrices table to see if prices were already deleted
                    var priceIdsNotFound = prices
                        .Select(dp => dp.GpmId)
                        .Except(existingPrices.Select(pdb => pdb.GpmID))
                        .ToList();
                    List<DeletedPriceModel> alreadyDeletedPrices = this.getDeletedPricesQuery
                        .Search(new GetDeletedPricesByGpmIdsParameters { PriceIds = priceIdsNotFound })
                        .ToList();

                    if (priceIdsNotFound.Count != alreadyDeletedPrices.Count)
                    {
                        IEnumerable<Guid> gpmIdsNotInDb = priceIdsNotFound
                            .Except(alreadyDeletedPrices.Select(adp => adp.GpmID));

                        data.Join(gpmIdsNotInDb, d => d.GpmId, gpmId => gpmId, (d, gpmId) => d)
                            .ToList()
                            .ForEach(p =>
                            {
                                p.ErrorCode = Errors.Codes.DeletePricesError;
                                p.ErrorDetails = Errors.Details.DeletePriceDoesNotExist;
                            });
                    }
                }
            }

            this.service.Process(data, message);
        }
    }
}
