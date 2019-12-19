using Icon.Services.ItemPublisher.Application;
using NLog;
using System;

namespace Icon.Services.ItemPublisher
{
    internal static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            try
            {
                var applicationRunner = new ApplicationRunner();
                applicationRunner.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}