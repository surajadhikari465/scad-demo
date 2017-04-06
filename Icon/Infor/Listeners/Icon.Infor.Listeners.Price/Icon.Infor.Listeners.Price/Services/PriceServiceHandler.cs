using Icon.Esb.Schemas.Wfm.ContractTypes;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.Extensions;
using Icon.Infor.Listeners.Price.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.Services
{
    public class PriceServiceHandler : IService<PriceModel>
    {
        private ICommandHandler<AddPricesCommand> addPricesCommandHandler;
        private ICommandHandler<DeletePricesCommand> deletePricesCommandHandler;
        private ICommandHandler<ReplacePricesCommand> replacePricesCommandHandler;

        public PriceServiceHandler(
            ICommandHandler<AddPricesCommand> addPricesCommandHandler,
            ICommandHandler<DeletePricesCommand> deletePricesCommandHandler,
            ICommandHandler<ReplacePricesCommand> replacePricesCommandHandler,
            ICommandHandler<ArchivePricesCommand> archivePricesCommandHandler)
        {
            this.addPricesCommandHandler = addPricesCommandHandler;
            this.deletePricesCommandHandler = deletePricesCommandHandler;
            this.replacePricesCommandHandler = replacePricesCommandHandler;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            if (!data.Any())
            {
                return;
            }

            if (data.Any(price => string.IsNullOrEmpty(price.ErrorCode)))
            {
                foreach (var price in data)
                {
                    ActionEnum action = price.Action;

                    switch (action)
                    {
                        case ActionEnum.Add:

                            var addPricesCommand = new AddPricesCommand { Prices = data.ToDbPriceModel() };
                            this.addPricesCommandHandler.Execute(addPricesCommand);
                            break;

                        case ActionEnum.Delete:

                            var deletePricesCommand = new DeletePricesCommand { Prices = data.ToDbPriceModel() };
                            this.deletePricesCommandHandler.Execute(deletePricesCommand);
                            break;

                        case ActionEnum.Replace:

                            var replacePricesCommand = new ReplacePricesCommand { Prices = data.ToDbPriceModel() };
                            this.replacePricesCommandHandler.Execute(replacePricesCommand);
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
