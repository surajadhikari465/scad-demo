using Icon.Logging;
using Icon.Services.ItemPublisher.Services;
using System;
using System.Threading;

namespace Icon.Services.ItemPublisher.Application
{
    public class ItemPublisherApplication : IItemPublisherApplication
    {
        private readonly System.Timers.Timer timer;
        private readonly IItemPublisherService itmePublisherService;
        private readonly ILogger<ItemPublisherApplication> logger;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private readonly ServiceSettings serviceSettings;

        public ItemPublisherApplication(IItemPublisherService itmePublisherService, ILogger<ItemPublisherApplication> logger, ServiceSettings serviceSettings)
        {
            this.logger = logger;
            this.itmePublisherService = itmePublisherService;
            this.serviceSettings = serviceSettings;
            this.timer = new System.Timers.Timer(this.serviceSettings.TimerIntervalInMilliseconds);
            this.timer.Elapsed += Timer_Elapsed;
        }

        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.semaphore.CurrentCount > 0)
            {
                try
                {
                    this.logger.Debug("Semaphore waiting");
                    await this.semaphore.WaitAsync();
                    await this.itmePublisherService.Process(this.serviceSettings.BatchSize);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex.ToString());
                }
                finally
                {
                    this.semaphore.Release();
                    this.logger.Debug("Semaphore released");
                }
            }
            else
            {
                this.logger.Debug("Semaphore Available Count < 1. Exiting.");
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