using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Service.Extensions;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Service.Services
{
    public class DeletePriceService : IService<DeletePrice>
    {
        private ICommandHandler<StagingPriceCommand> stagePricesCommandHandler;
        private ICommandHandler<DeletePricesCommand> deletePricesCommandHandler;
        private ICommandHandler<DeleteStagingCommand> deleteStagingCommandHandler;
        private ICommandHandler<AddEsbMessageQueuePriceCommand> addEsbPriceQueueCommandHandler;

        public DeletePriceService(ICommandHandler<StagingPriceCommand> stagePricesCommandHandler,
            ICommandHandler<DeletePricesCommand> deletePricesCommandHandler,
            ICommandHandler<DeleteStagingCommand> deleteStagingCommandHandler,
            ICommandHandler<AddEsbMessageQueuePriceCommand> addEsbPriceQueueCommandHandler)
        {
            this.stagePricesCommandHandler = stagePricesCommandHandler;
            this.deletePricesCommandHandler = deletePricesCommandHandler;
            this.deleteStagingCommandHandler = deleteStagingCommandHandler;
            this.addEsbPriceQueueCommandHandler = addEsbPriceQueueCommandHandler;
        }

        public void Handle(DeletePrice data)
        {
            // Setup timestamp and guid for staging table
            DateTime timestamp = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            // Make each record unique before inserting into staging table
            var distinctData = data.Prices
                .DistinctBy(p => new { p.ScanCode, p.BusinessUnitId, p.StartDate, p.EndDate, p.PriceType }).ToList();

            // Add Prices to staging
            StagingPriceCommand addStagingDataParameters = new StagingPriceCommand();
            addStagingDataParameters.Prices = distinctData.ToStagingPriceModel(timestamp: timestamp, guid: transactionId);
            this.stagePricesCommandHandler.Execute(addStagingDataParameters);

            try
            {
                // Delete Reg and Sale prices
                List<string> regions = data.Prices.Select(p => p.Region).Distinct().ToList();
                foreach (var region in regions)
                {
                    // Delete in Mammoth Db
                    var deletePriceParameters = new DeletePricesCommand();
                    deletePriceParameters.Region = region;
                    deletePriceParameters.Timestamp = timestamp;
                    deletePriceParameters.TransactionId = transactionId;
                    this.deletePricesCommandHandler.Execute(deletePriceParameters);

                    // Delete row in MessageQueuePrice
                    var esbCommandParameters = new AddEsbMessageQueuePriceCommand();
                    esbCommandParameters.Region = region;
                    esbCommandParameters.Timestamp = timestamp;
                    esbCommandParameters.TransactionId = transactionId;
                    esbCommandParameters.MessageActionId = MessageActions.Delete;
                    this.addEsbPriceQueueCommandHandler.Execute(esbCommandParameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DeleteFromStaging(transactionId);
            }            
        }

        private void DeleteFromStaging(Guid transactionId)
        {
            // Delete Prices from staging table
            DeleteStagingCommand deleteStagingParameters = new DeleteStagingCommand
            {
                StagingTableName = StagingTableNames.Price,
                TransactionId = transactionId
            };

            this.deleteStagingCommandHandler.Execute(deleteStagingParameters);
        }
    }
}
