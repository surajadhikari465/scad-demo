using Icon.Logging;
using System;
using GlobalEventController.Common;
using Icon.Framework;

namespace SubteamEventController
{
    class Program
    {
        private static NLogLogger<Program> logger = new NLogLogger<Program>();

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
            GlobalControllerBase globalController = ControllerProvider.ComposeController();

            logger.Info("Starting Subteam Event Controller...");

            globalController.Start();
            logger.Info("Shutting down Subteam Event Controller...");
        }
    }
}
