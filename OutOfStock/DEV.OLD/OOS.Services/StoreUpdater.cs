using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace OOS.Services
{
    public class StoreUpdater
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            _logger.Info("Store Updater Started");
        }

        public void Stop()
        {
            _logger.Info("Store Updater Stopped");
        }
    }
}
