using Mammoth.Logging;
using Mammoth.PrimeAffinity.Library.Esb;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MammothWebApi.BackgroundJobs
{
    public class ReconnectToEsbJob : IJob
    {
        private ILogger logger;

        public ReconnectToEsbJob(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (EsbConnectionCache.Connection != null && EsbConnectionCache.Connection.IsClosed)
                    {
                        logger.Info("Reconnecting to ESB.");
                        EsbConnectionCache.InitializeConnectionFactoryAndConnection();
                        logger.Info("Reconnected to ESB.");
                    }
                }
                catch(Exception ex)
                {
                    logger.Error("Error occurred while trying to connect to the ESB", ex);
                }
            });
        }
    }
}