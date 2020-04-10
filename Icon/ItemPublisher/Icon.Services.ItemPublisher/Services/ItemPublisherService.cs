using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Polly;
using Polly.Retry;
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
        private AsyncRetryPolicy policy = null;

        public ItemPublisherService(IItemPublisherRepository repository,
            ILogger<ItemPublisherService> logger,
            ServiceSettings serviceSettings,
            IItemProcessor itemProcessor)
        {
            this.repository = repository;
            this.logger = logger;
            this.serviceSettings = serviceSettings;
            this.itemProcessor = itemProcessor;
            this.policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(5,
                retryAttempt =>
                    TimeSpan.FromSeconds(2),
                  (exception, timeSpan, context) =>
                  {
                      this.logger.Error(exception.ToString());
                  }
                );
        }

        /// <summary>
        /// Calls ProcesInternal. If an exception occurs retry the operation with incremental backoff up to 60 seconds then 60 seconds after that.
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task Process(int batchSize)
        {
            
            await this.ProcessInternal(batchSize);
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
            List<MessageQueueItemModel> records = new List<MessageQueueItemModel>();
            List<MessageQueueItem> messageQueueItems = new List<MessageQueueItem>();

            do
            {
                try
                {
                    this.logger.Info($"Processing records. {DateTime.UtcNow}.");

                    messageQueueItems.Clear();
                    messageQueueItems = await this.repository.DequeueMessageQueueItems(batchSize);

                    try
                    {
                        if (messageQueueItems.Any())
                        {
                            await this.policy.ExecuteAsync(async token =>
                            {
                                records = await this.GetMessageQueueItemModels(messageQueueItems);
                                recordCount = records.Count;

                                bool success = await this.ProcessRecordsAndReturnSuccess(records);
                                if (!success)
                                {
                                    // an error occurred somewhere in the process. We don't know exactly what it is so just throw up an exception so the process will retry.
                                    throw new Exception("Unable to process records");
                                }
                            }, CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            this.logger.Error(ex.ToString());
                            // if we had an exception and we retried and still get an exception dump the items and exception to the dead letter queue
                            if (messageQueueItems.Any())
                            {
                                var deadLetterMessage = new MessageDeadLetterQueue(ex.ToString(), messageQueueItems.Select(x => x.ItemId).ToList());
                                await this.repository.AddDeadLetterMessageQueueRecord(deadLetterMessage);
                            }
                        }
                        catch (Exception dex)
                        {
                            this.logger.Error(dex.ToString());
                        }
                    }
                    finally
                    {
                        this.logger.Info($"Processing complete. {stopwatch.Elapsed.TotalSeconds} seconds elapsed. {recordCount} records processed.");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error($"Failed to dequeue pending messages {DateTime.UtcNow}. Error: {ex.ToString()}");                    
                }
            } while (messageQueueItems.Any());            
        }

        // Calls ths Item Processor and if errors are returned tell the process to not continue.
        // Returns true if succeeded otherwise false.
        private async Task<bool> ProcessRecordsAndReturnSuccess(List<MessageQueueItemModel> records)
        {
            List<EsbSendResult> response = new List<EsbSendResult>();

            response.AddRange(await this.itemProcessor.ProcessRetailRecords(records));
            response.AddRange(await this.itemProcessor.ProcessNonRetailRecords(records));

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
        }

        // dequeue records and tell the process to not contiue if no records are found. Otherwise return the records.
        private async Task<List<MessageQueueItemModel>> GetMessageQueueItemModels(List<MessageQueueItem> messageQueueItems)
        {
            List<MessageQueueItemModel> records = await this.repository.GetMessageItemModels(messageQueueItems);
            this.logger.Debug($"{records.Count} queue records found");
            return records;
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