using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;

namespace IRMAUserAuditConsole
{
    public class Program
    {
        private const string version = "2.1";

        DateTime currentDateTime = DateTime.Now;

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private bool allLogsMoved = true;

        //static Mutex singleton;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Program p = new Program();

            // make sure we are only running one instance...
            //bool created;
            //singleton = new Mutex(true, "WHOLEFOODS_USER_AUDIT_PROCESS_THINGY", out created);
            //if (!created)
            //{
            //    p.logger.Error("App already running. Exiting...");
            //    return;
            //}

            var options = UserAudit.GetOptionsFromProgramArguments(args);
            if (options.IsError)
            {
                p.logger.Error(options.ErrorMessage);
                ShowUsage(p.logger);
            }
            else
            {
                if (options.IsWarning)
                {
                    p.logger.Warn(options.WarningMessage);
                }
                var userAudit = new UserAudit(options, p.logger);
                userAudit.RunAudit();
            }
        }

        private static void ShowUsage(ILog logger)
        {
            logger.Error("Usage:");
            logger.Error("\tIRMAUserAuditConsole <region> [environment]");
            logger.Error("\twhere <region> is the two letter region code (SP, CE, SO, etc)");
            logger.Error("\tand [environment] is one of: test, dev, qa, prod.  This is optional");
            logger.Error("\tand will default to TEST if not specified.");
            Console.WriteLine();
        }
    }
}
