using Icon.Logging;
using System;
using TlogController.Common;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TlogController.Controller
{
    class Program
    {
        private static ILogger<Program> logger = new NLogLogger<Program>();
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                logger.Error("No command-line arguments provided.  The program cannot continue.");
                return;
            }

            int controllerInstanceId;
            if (!Int32.TryParse(args[0], out controllerInstanceId) || controllerInstanceId < 1)
            {
                logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                return;
            }

            StartupOptions.Instance = controllerInstanceId;
            logger = new NLogLoggerInstance<Program>(StartupOptions.Instance.ToString());

            int maxTransactionsToProcess;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MaxTlogTransactionsToProcess"], out maxTransactionsToProcess))
            {
                maxTransactionsToProcess = 500;
            }
            StartupOptions.MaxTransactionsToProcess = maxTransactionsToProcess;

            int maxTlogTransactionsWhenSplit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MaxTlogTransactionsWhenSplit"], out maxTlogTransactionsWhenSplit))
            {
                maxTlogTransactionsWhenSplit = 50;
            }
            StartupOptions.MaxTlogTransactionsWhenSplit = maxTlogTransactionsWhenSplit;

            var tlogController = ControllerProvider.ComposeController();

            logger.Info("Starting Tlog Controller...");

            tlogController.Start();

            logger.Info("Shutting down Tlog Controller...");
        }
    }
}
