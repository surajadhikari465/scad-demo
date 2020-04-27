using System;
using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Service.Interfaces;
using System.Timers;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using Icon.Common.DataAccess;
using Icon.Logging;
using OfficeOpenXml.DataValidation;

namespace BrandUploadProcessor.Service
{
    public class BrandUploadProcessorService : IService
    {

        private readonly Timer timer;
        private readonly IQueryHandler<EmptyQueryParameters<IEnumerable<BrandUploadInformation>>, IEnumerable<BrandUploadInformation>> getBulkUploadsQueryHandler;
        private readonly ILogger<BrandUploadProcessorService> logger;
        private readonly IBrandUploadManager brandUploadManager;
        private readonly IServiceConfiguration configuration;
        private readonly IBrandsCache brandsCache;

        public BrandUploadProcessorService(ILogger<BrandUploadProcessorService> logger, 
            IQueryHandler<EmptyQueryParameters<IEnumerable<BrandUploadInformation>>,
            IEnumerable<BrandUploadInformation>> getBulkUploadsQueryHandler,
            IBrandUploadManager brandUploadManager, 
            IServiceConfiguration configuration,
            IBrandsCache brandsCache)
        {
            this.logger = logger;
            this.getBulkUploadsQueryHandler = getBulkUploadsQueryHandler;
            this.brandUploadManager = brandUploadManager;
            this.configuration = configuration;
            this.brandsCache = brandsCache;

            timer = new Timer(this.configuration.TimerInterval) {AutoReset = true};
            timer.Elapsed += ServiceTimerOnElapsed;
        }

        private void ServiceTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();

                var uploadsToProcess = GetUploadsToProcess().ToList();
                if (uploadsToProcess.Any())
                {
                    brandUploadManager.GetAttributeData();
                    brandsCache.Refresh();
                }
                
                foreach (var uploadInforamtion in uploadsToProcess)
                {
                    brandUploadManager.SetActiveUpload(uploadInforamtion);
                    brandUploadManager.SetStatus(Enums.FileStatusEnum.Processing, string.Empty, 5);
                    brandUploadManager.Validate();
                    brandUploadManager.Process();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                brandUploadManager.SetStatus(Enums.FileStatusEnum.Error, ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }

        private IEnumerable<BrandUploadInformation> GetUploadsToProcess()
        {
            return getBulkUploadsQueryHandler.Search(new EmptyQueryParameters<IEnumerable<BrandUploadInformation>>());
        }

        public void Start()
        {
            timer.Start();
            logger.Info($"Started Brand Upload Processor.");
        }

        public void Stop()
        {
            timer.Stop();
            logger.Info($"Stopped Brand Upload Processor.");
        }
    }
}