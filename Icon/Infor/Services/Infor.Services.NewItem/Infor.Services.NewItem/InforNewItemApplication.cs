using Icon.Common;
using Icon.Logging;
using Infor.Services.NewItem.Processor;
using System;
using System.Timers;

namespace Infor.Services.NewItem
{
    public class InforNewItemApplication : IInforNewItemApplication
    {
        private Timer timer;
        private ILogger<InforNewItemApplication> logger;
        private INewItemProcessor processor;

        public InforNewItemApplication(INewItemProcessor processor, ILogger<InforNewItemApplication> logger)
        {
            this.processor = processor;
            this.logger = logger;
            int runInterval = AppSettingsAccessor.GetIntSetting("RunInterval");
            this.timer = new Timer(runInterval);
        }

        public void Start()
        {
            logger.Info("Starting Infor New Item Application");
            this.timer.Elapsed += ProcessNewItemEvents;
            this.timer.Start();
        }

        public void ProcessNewItemEvents(object sender, ElapsedEventArgs eventArgs)
        {
            this.timer.Stop();
            try
            {
                int instanceId = AppSettingsAccessor.GetIntSetting("InstanceId");

                processor.ProcessNewItemEvents(instanceId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Unexpected error occurred. Error = {0}", ex));
            }
            finally
            {
                this.timer.Start();
            }
        }

        public void Stop()
        {
            logger.Info("Shutting down Infor New Item Application.");
        }
    }
}
