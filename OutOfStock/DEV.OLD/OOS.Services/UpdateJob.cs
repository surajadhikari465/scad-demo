//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using NLog;
//using OOS.Services.DAL;
//using Quartz;

//namespace OOS.Services
//{
//    public class UpdateJob : IJob
//    {
//        private static Logger _logger = LogManager.GetCurrentClassLogger();

//        public void Execute(IJobExecutionContext context)
//        {
//            _logger.Info("Executions");


//            var repo = new OosRepository();
//            var sage = new SageApi();

//            var updater = new StoreUpdaterSage(repo, sage);
//            updater.Compare();
//            updater.UpdateDatabase();

//            _logger.Info("job done");

//        }
//    }
//}
