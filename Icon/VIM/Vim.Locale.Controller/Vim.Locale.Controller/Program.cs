using System;
using Vim.Common.ControllerApplication;
using Vim.Logging;

namespace Vim.Locale.Controller
{
    class Program
    {
        private static int instanceId;
        private static ILogger logger = new NLogLogger(typeof(Program));

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                logger.Error("No command-line arguments provided.  The program cannot continue.");
                return;
            }

            if (!Int32.TryParse(args[0], out instanceId) || instanceId < 1)
            {
                logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                return;
            }

            var container = SimpleInjectorInitializer.InitializeContainer(instanceId);

            container.Verify();

            container.GetInstance<IControllerApplication>().Run();
        }
    }
}
