using Icon.Logging;
using System;
using GlobalEventController.Common;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Common.Email;

namespace SubteamEventController.Controller
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
            try
            {
                ControllerProvider.emailClient = EmailClient.CreateFromConfig();
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Subteam Controller Unable to create email client. {0}", ex));
                return;
            }

            try
            { 
                SubteamEventControllerBase subTeamEventControllerBase = ControllerProvider.ComposeController();                
                logger.Info("Starting Sub team Event Controller...");
                subTeamEventControllerBase.Start();
            }
            catch (Exception ex)
            {
                logger.Error(String.Format(SubTeamConstants.ErrorUnexpectedException, ex));
                ControllerProvider.emailClient.Send(String.Format(SubTeamConstants.ErrorUnexpectedException, ex), SubTeamConstants.EmailSubjectUnexpectedException);
            }

            logger.Info("Shutting down Sub team Event Controller...");
        }
    }
}
