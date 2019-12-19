using Icon.Common;
using Icon.Logging;
using Services.NewItem.Processor;
using System;
using System.Timers;

namespace Services.NewItem
{
    public class NewItemApplication : INewItemApplication
    {
        private Timer timer;
        private ILogger<NewItemApplication> logger;
        private INewItemProcessor processor;

        public NewItemApplication(INewItemProcessor processor, ILogger<NewItemApplication> logger)
        {
            this.processor = processor;
            this.logger = logger;
            int runInterval = AppSettingsAccessor.GetIntSetting("RunInterval");
            this.timer = new Timer(runInterval);
        }

        public void Start()
        {
            logger.Info("Starting New Item Application");
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
            logger.Info("Shutting down New Item Application.");
        }
    }
}
