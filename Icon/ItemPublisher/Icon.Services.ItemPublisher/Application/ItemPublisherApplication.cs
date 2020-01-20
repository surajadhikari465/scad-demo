using Icon.Logging;
using Icon.Services.ItemPublisher.Services;
using System;
using System.Threading;

namespace Icon.Services.ItemPublisher.Application
{
    public class ItemPublisherApplication : IItemPublisherApplication
    {
        private readonly System.Timers.Timer timer;
        private readonly IItemPublisherService itemPublisherService;
        private readonly ILogger<ItemPublisherApplication> logger;
        private readonly ServiceSettings serviceSettings;

        public ItemPublisherApplication(IItemPublisherService itmePublisherService, ILogger<ItemPublisherApplication> logger, ServiceSettings serviceSettings)
        {
            this.logger = logger;
            this.itemPublisherService = itmePublisherService;
            this.serviceSettings = serviceSettings;
            this.timer = new System.Timers.Timer(this.serviceSettings.TimerIntervalInMilliseconds);
            this.timer.Elapsed += Timer_Elapsed;
        }

        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.timer.Stop();
            try
            {
                await this.itemPublisherService.Process(this.serviceSettings.BatchSize);
            }
            catch (Exception ex)
            {
                logger.Error("Unexpected error occurred in Item Publisher: " + ex.ToString());
            }
            finally
            {
                this.timer.Start();
            }
        }

        public void Start()
        {
            this.logger.Info("Starting application");
            this.timer.Start();
        }

        public void Stop()
        {
            this.logger.Info("Stopping application");
            this.timer.Stop();
        }
    }
}