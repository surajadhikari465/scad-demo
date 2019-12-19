using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AttributePublisher.Infrastructure
{
    public abstract class RecurringServiceBase : IService
    {
        private Timer timer;
        private RecurringServiceSettings settings;

        public RecurringServiceBase(RecurringServiceSettings settings)
        {
            this.settings = settings;

            timer = new Timer();
            timer.Interval = this.settings.RunInterval;
            timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            HandleServiceStart();
            Task.Run(() => Timer_Elapsed(null, null));
        }

        public void Stop()
        {
            timer.Stop();
            timer.Elapsed -= Timer_Elapsed;
            HandleServiceStop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                Run();
            }
            catch(Exception ex)
            {
                HandleRunException(ex);
            }
            finally
            {
                timer.Start();
            }
        }

        public abstract void Run();

        public abstract void HandleRunException(Exception ex);

        public abstract void HandleServiceStart();

        public abstract void HandleServiceStop();
    }
}
