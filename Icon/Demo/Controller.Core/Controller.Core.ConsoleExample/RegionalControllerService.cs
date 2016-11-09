using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Controller.Core.ConsoleExample
{
    public class RegionalControllerService
    {
        private RegionalControllerFactory factory;
        private ILogger<RegionalControllerService> logger;
        private List<string> regions = new List<string> { "FL", "MA", "MW" };
        private Timer timer;

        public void Start()
        {
            Timer t = new Timer();
            t.Elapsed += (o, e) => RunController();
            t.Interval = 5000;
            RunController();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void RunController()
        {
            timer.Stop();

            try
            {
                Parallel.ForEach(regions, (region) =>
                {
                    factory.Create(region).Run();
                });
            }
            catch (Exception ex)
            {
                logger.Error(
                    new
                    {
                        Message = "Unexpected error occurred in the Regional Controller Service",
                        Error = ex.ToString()
                    }.ToJson());
            }
            finally
            {
                timer.Start();
            }
        }
    }
}
