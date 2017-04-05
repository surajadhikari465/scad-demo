using Icon.Esb.Schemas.Wfm.ContractTypes;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Extensions;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.Decorators
{
    public class AddValidationPriceServiceDecorator : IService<PriceModel>
    {
        private IService<PriceModel> service;
        private IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> getPricesQuery;

        public AddValidationPriceServiceDecorator(
            IService<PriceModel> service,
            IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> getPricesQuery)
        {
            this.service = service;
            this.getPricesQuery = getPricesQuery;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            if (data.Any(d => d.Action == ActionEnum.Add))
            {
                var pricesInDb = this.getPricesQuery.Search(new GetPricesByGpmIdsParameters { Prices = data.ToDbPriceModel() });

                if (pricesInDb.Any())
                {
                    data.Join(pricesInDb, d => d.GpmId, pid => pid.GpmID, (d, pid) => d)
                        .ToList()
                        .ForEach(p =>
                        {
                            p.ErrorCode = Errors.Codes.AddPricesError;
                            p.ErrorDetails = Errors.Details.AddPriceAlreadyExists;
                        });
                }
            }

            this.service.Process(data, message);
        }
    }
}
