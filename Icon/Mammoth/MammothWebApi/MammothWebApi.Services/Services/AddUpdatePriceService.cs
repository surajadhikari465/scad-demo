using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Service.Extensions;
using MammothWebApi.Service.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Service.Services
{
    public class AddUpdatePriceService : IService<AddUpdatePrice>
    {
        private ILogger logger;
        private ICommandHandler<StagingPriceCommand> stagePricesCommandHandler;
        private ICommandHandler<AddOrUpdatePricesCommand> pricesCommandHandler;
        private ICommandHandler<DeleteStagingCommand> deleteStagingCommandHandler;
        private ICommandHandler<AddEsbMessageQueuePriceCommand> addEsbPriceCommandHandler;

        public AddUpdatePriceService(
            ILogger logger,
            ICommandHandler<StagingPriceCommand> stagePricesCommandHandler,
            ICommandHandler<AddOrUpdatePricesCommand> pricesCommandHandler,
            ICommandHandler<DeleteStagingCommand> deleteStagingCommandHandler,
            ICommandHandler<AddEsbMessageQueuePriceCommand> addEsbPriceCommandHandler)
        {
            this.logger = logger;
            this.stagePricesCommandHandler = stagePricesCommandHandler;
            this.pricesCommandHandler = pricesCommandHandler;
            this.deleteStagingCommandHandler = deleteStagingCommandHandler;
            this.addEsbPriceCommandHandler = addEsbPriceCommandHandler;
        }

        public void Handle(AddUpdatePrice data)
        {
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            // Make each record unique before inserting into staging table
            var distinctData = data.Prices.DistinctBy(p => new { p.ScanCode, p.BusinessUnitId, p.StartDate, p.PriceType }).ToList();

            logger.Info(string.Format("Beginning AddUpdatePrice for {0} distinct prices. Prices : {1}", distinctData.Count(), string.Join("|", distinctData.Select(p => p.ToLogString()))));

            // Add prices to staging table
            var stagingParameters = new StagingPriceCommand();
            stagingParameters.Prices = distinctData.ToStagingPriceModel(timestamp: now, guid: transactionId);
            this.stagePricesCommandHandler.Execute(stagingParameters);

            try
            {
                // Add/Update Reg and Sale Prices
                List<string> regions = data.Prices.Select(p => p.Region).Distinct().ToList();
                foreach (var region in regions)
                {
                    var priceCommandParameters = new AddOrUpdatePricesCommand();
                    priceCommandParameters.Region = region;
                    priceCommandParameters.Timestamp = now;
                    priceCommandParameters.TransactionId = transactionId;
                    this.pricesCommandHandler.Execute(priceCommandParameters);

                    // Queue Esb Price Message
                    var esbCommandParameters = new AddEsbMessageQueuePriceCommand();
                    esbCommandParameters.Region = region;
                    esbCommandParameters.Timestamp = now;
                    esbCommandParameters.TransactionId = transactionId;
                    esbCommandParameters.MessageActionId = MessageActions.AddOrUpdate;
                    this.addEsbPriceCommandHandler.Execute(esbCommandParameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DeleteFromStaging(transactionId, distinctData);
            }

            logger.Info(string.Format("Finished AddUpdatePrice for {0} distinct prices. Prices : {1}", distinctData.Count(), string.Join("|", distinctData.Select(p => p.ToLogString()))));
        }

        private void DeleteFromStaging(Guid guid, List<PriceServiceModel> prices)
        {
            // Delete from staging table
            DeleteStagingCommand deleteStagingData = new DeleteStagingCommand
            {
                TransactionId = guid,
                StagingTableName = StagingTableNames.Price
            };
            this.deleteStagingCommandHandler.Execute(deleteStagingData);
        }
    }
}
