using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.Interfaces;
using Icon.Common.DataAccess;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Timers;
using static BulkItemUploadProcessor.Common.Enums;

namespace BulkItemUploadProcessor.Service
{
    public class BulkUploadProcessorService : IService
    {
        private readonly Timer timer;
        private readonly IServiceConfiguration configuration;
        private readonly ILogger<BulkUploadProcessorService> logger;
        private readonly IBulkUploadManager bulkUploadManager;
        private readonly IQueryHandler<GetBulkUploadsParameters, IEnumerable<BulkItemUploadInformation>> getBulkUploadsQueryHandler;

        public BulkUploadProcessorService(
            IServiceConfiguration configuration,
            ILogger<BulkUploadProcessorService> logger,
            IQueryHandler<GetBulkUploadsParameters, IEnumerable<BulkItemUploadInformation>> getBulkUploadsQueryHandler,
            IBulkUploadManager bulkUploadManager)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.getBulkUploadsQueryHandler = getBulkUploadsQueryHandler;
            this.bulkUploadManager = bulkUploadManager; 

            timer = new Timer(this.configuration.TimerInterval) {AutoReset = true};
            timer.Elapsed += ServiceTimerOnElapsed;
        }

        private void ServiceTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();

                bulkUploadManager.GetAttributeData();

                foreach (var bulkItemUploadInformation in GetUploadsToProcess())
                {
                    bulkUploadManager.SetActiveUpload(bulkItemUploadInformation);
                    bulkUploadManager.SetStatus(FileStatusEnum.Processing);
                    bulkUploadManager.Validate();
                    bulkUploadManager.Process();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                bulkUploadManager.SetStatus(FileStatusEnum.Error, ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }

        private IEnumerable<BulkItemUploadInformation> GetUploadsToProcess()
        {
            return this.getBulkUploadsQueryHandler.Search(new GetBulkUploadsParameters());
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}