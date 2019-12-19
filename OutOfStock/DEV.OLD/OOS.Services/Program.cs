using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOS.Services.DAL;
using NLog;

namespace OOS.Services
{
    class Program
    {
        static void Main(string[] args)
        {

             var logger = LogManager.GetCurrentClassLogger();

            logger.Info("Executions");

            var repo = new OosRepository();
            var sage = new SageApi();

            var updater = new StoreUpdaterSage(repo, sage);
            updater.Compare();
            updater.UpdateDatabase();

            logger.Info("job done");

        }
    }
}
