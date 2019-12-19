using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    public class ItemPublisherService : IItemPublisherService
    {
        private readonly IItemPublisherRepository repository;
        private readonly ILogger<ItemPublisherService> logger;
        private readonly ServiceSettings serviceSettings;
        private readonly IItemProcessor itemProcessor;

        public ItemPublisherService(IItemPublisherRepository repository,
            ILogger<ItemPublisherService> logger,
            ServiceSettings serviceSettings,
            IItemProcessor itemProcessor)
        {
            this.repository = repository;
            this.logger = logger;
            this.serviceSettings = serviceSettings;
            this.itemProcessor = itemProcessor;
        }

        /// <summary>
        /// Calls ProcesInternal. If an exception occurs retry the operation with incremental backoff up to 60 seconds then 60 seconds after that.
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task Process(int batchSize)
        {
            var policy = Policy
            .Handle<SqlException>()
            .WaitAndRetryForeverAsync(
                retryAttempt =>
                {
                    // Backoff by 2,4,8,16,32 then 60 seconds after that
                    double waitTimeInSeconds = Math.Pow(2, retryAttempt);

                    if (waitTimeInSeconds > 60)
                    {
                        waitTimeInSeconds = 60;
                    }
                    return TimeSpan.FromSeconds(waitTimeInSeconds);
                },
                (exception, timespan, context) =>
                {
                    this.logger.Error(exception.ToString());
                });

            await policy.ExecuteAsync(async token =>
            {
                await this.ProcessInternal(batchSize);
            }, CancellationToken.None);
        }

        /// <summary>
        /// ProcessInternal loads a batch of records from the esb.MessageQueueItem table and then
        /// processes those records one at a time generating an esb message and putting that on the esb.
        /// The queue records are then moved to the esb.MessageQueueItemArchive table.
        /// </summary>
        /// <returns></returns>
        private async Task ProcessInternal(int batchSize)
        {
            if (!(await this.itemProcessor.ReadyForProcessing))
            {
                this.logger.Info($"Esb service not ready yet");
                return;
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int recordCount = 0;

            try
            {
                // continueExecution determines if in this iteration of the timer we will check the queue table
                // for more records after processing this batch. If an error occurs we stop processing.
                bool continueExecution = true;

                this.logger.Info($"Processing records. {DateTime.UtcNow}.");

                // dequeue records and tell the process to not contiue if no records are found. Otherwise return the records.
                Func<Task<List<MessageQueueItemModel>>> dequeueRecords = async () =>
                {
                    List<MessageQueueItemModel> records = await this.repository.DequeueMessageQueueRecords(batchSize);
                    recordCount += records.Count;
                    this.logger.Debug($"{records.Count} queue records found");
                    return records;
                };

                // Calls ths Item Processor and if errors are returned tell the process to not continue.
                // Returns true if succeeded otherwise false.
                Func<List<MessageQueueItemModel>, Task<bool>> processRecordsAndReturnSuccess = async (records) =>
                {
                    List<EsbSendResult> response = new List<EsbSendResult>();

                    response.AddRange(await this.itemProcessor.ProcessRetailRecords(records));
                    response.AddRange(await this.itemProcessor.ProcessNonRetailRecords(records));
                    response.AddRange(await this.itemProcessor.ProcessDepartmentSaleRecords(records));

                    if (!response.Any(r => r != null))
                    {
                        return true;
                    }

                    await this.HandleProcessResult(records, response);

                    if (response.All(x => x.Success))
                    {
                        return true;
                    }
                    else
                    {
                        this.logger.Warn($"An error occurred. No more records will be processed. {string.Join(",", response)}");
                        return false;
                    }
                };

                while (continueExecution)
                {
                    this.repository.BeginTransaction();

                    List<MessageQueueItemModel> records = await dequeueRecords();

                    if (records.Count > 0)
                    {
                        bool success = await processRecordsAndReturnSuccess(records);

                        this.repository.CommitTransaction();
                        continueExecution = true;
                    }
                    else
                    {
                        this.repository.CommitTransaction();
                        continueExecution = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
                this.repository.RollbackTransaction();
                throw;
            }
            finally
            {
                this.logger.Info($"Processing complete. {stopwatch.Elapsed.TotalSeconds} seconds elapsed. {recordCount} records processed.");
            }
        }

        /// <summary>
        /// Takes a list of models and the results from the ESB and inserts a record into the history table
        /// </summary>
        /// <param name="records"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task HandleProcessResult(List<MessageQueueItemModel> records, List<EsbSendResult> results)
        {
            List<MessageQueueItemArchive> history = new List<MessageQueueItemArchive>();
            results.ForEach(x =>
            {
                history.Add(new MessageQueueItemArchive(records,
                    x.MessageId,
                    x.Request,
                    x.Headers,
                    x.ToString(),
                    x.Warnings,
                    DateTime.UtcNow,
                    Environment.MachineName,
                    x.Success));
            });
            await this.repository.AddMessageQueueHistoryRecords(history);
        }
    }
}