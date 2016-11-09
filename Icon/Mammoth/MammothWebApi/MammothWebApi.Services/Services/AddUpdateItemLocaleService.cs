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
    public class AddUpdateItemLocaleService : IService<AddUpdateItemLocale>
    {
        private ILogger logger;
        private ICommandHandler<StagingItemLocaleCommand> itemLocaleStagingHandler;
        private ICommandHandler<StagingItemLocaleExtendedCommand> itemLocaleExtendedStagingHandler;
        private ICommandHandler<AddOrUpdateItemLocaleCommand> addUpdateItemLocaleHandler;
        private ICommandHandler<AddOrUpdateItemLocaleExtendedCommand> addUpdateItemLocaleExtendedHandler;
        private ICommandHandler<AddEsbMessageQueueItemLocaleCommand> addToMessageQueueItemLocaleHandler;
        private ICommandHandler<DeleteStagingCommand> deleteStagingHandler;

        public AddUpdateItemLocaleService(
            ILogger logger,
            ICommandHandler<StagingItemLocaleCommand> itemLocaleStagingHandler,
            ICommandHandler<StagingItemLocaleExtendedCommand> itemLocaleExtendedStagingHandler,
            ICommandHandler<AddOrUpdateItemLocaleCommand> addUpdateItemLocaleHandler,
            ICommandHandler<AddOrUpdateItemLocaleExtendedCommand> addUpdateItemLocaleExtendedHandler,
            ICommandHandler<AddEsbMessageQueueItemLocaleCommand> addToMessageQueueItemLocaleHandler,
            ICommandHandler<DeleteStagingCommand> deleteStagingHandler)
        {
            this.logger = logger;
            this.itemLocaleStagingHandler = itemLocaleStagingHandler;
            this.itemLocaleExtendedStagingHandler = itemLocaleExtendedStagingHandler;
            this.addUpdateItemLocaleHandler = addUpdateItemLocaleHandler;
            this.addUpdateItemLocaleExtendedHandler = addUpdateItemLocaleExtendedHandler;
            this.addToMessageQueueItemLocaleHandler = addToMessageQueueItemLocaleHandler;
            this.deleteStagingHandler = deleteStagingHandler;
        }

        public void Handle(AddUpdateItemLocale data)
        {
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            // Make each record unique before inserting into staging table
            var distinctData = data.ItemLocales.DistinctBy(il => new { il.Region, il.ScanCode, il.BusinessUnitId })
                .OrderBy(d => d.BusinessUnitId)
                .ThenBy(d => d.ScanCode)
                .ToList();

            logger.Info(string.Format("Beginning AddUpdateItemLocale for {0} distinct items. Items : {1}", distinctData.Count(), string.Join("|", distinctData.Select(p => p.ToLogString()))));

            // Add to ItemLocale staging table
            var insertStagingParameter = new StagingItemLocaleCommand();
            insertStagingParameter.ItemLocales = distinctData.ToStagingItemLocaleModel(timestamp: now, transactionId: transactionId);
            this.itemLocaleStagingHandler.Execute(insertStagingParameter);

            // Add to ItemLocaleExtended staging table
            var insertExtendedStagingParameter = new StagingItemLocaleExtendedCommand();
            insertExtendedStagingParameter.ItemLocalesExtended = distinctData.ToStagingItemLocaleExtendedModel(timestamp: now, transactionId: transactionId);
            this.itemLocaleExtendedStagingHandler.Execute(insertExtendedStagingParameter);

            try
            {
                // Run Add/Update for core attributes for each region
                List<string> regions = data.ItemLocales.Select(il => il.Region).Distinct().ToList();
                foreach (string region in regions)
                {
                    // Add/Update core attributes on ItemAttributes_Locale_XX
                    var addUpdateParameters = new AddOrUpdateItemLocaleCommand
                    {
                        Region = region,
                        Timestamp = now,
                        TransactionId = transactionId
                    };

                    this.addUpdateItemLocaleHandler.Execute(addUpdateParameters);

                    // Add/Update extended attributes on ItemAttributes_Locale_XX_Ext
                    var addUpdateExtendedParameters = new AddOrUpdateItemLocaleExtendedCommand
                    {
                        Region = region,
                        Timestamp = now,
                        TransactionId = transactionId
                    };

                    this.addUpdateItemLocaleExtendedHandler.Execute(addUpdateExtendedParameters);

                    // Write messages to the MessageQueue table.
                    var addToMessageQueueItemLocaleParameters = new AddEsbMessageQueueItemLocaleCommand
                    {
                        Region = region,
                        Timestamp = now,
                        TransactionId = transactionId
                    };

                    this.addToMessageQueueItemLocaleHandler.Execute(addToMessageQueueItemLocaleParameters);
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                DeleteFromStaging(transactionId, distinctData);
            }
        }

        private void DeleteFromStaging(Guid transactionId, List<ItemLocaleServiceModel> distinctData)
        {
            // Delete from Staging
            var deleteStagingParameter = new DeleteStagingCommand
            {
                TransactionId = transactionId,
                StagingTableName = StagingTableNames.ItemLocale
            };

            deleteStagingHandler.Execute(deleteStagingParameter);

            deleteStagingParameter = new DeleteStagingCommand
            {
                TransactionId = transactionId,
                StagingTableName = StagingTableNames.ItemLocaleExtended
            };

            deleteStagingHandler.Execute(deleteStagingParameter);

            logger.Info(string.Format("Finished AddUpdateItemLocale for {0} distinct items. Items : {1}", distinctData.Count(), string.Join("|", distinctData.Select(p => p.ToLogString()))));
        }
    }
}
