using Quartz;
using Quartz.Spi;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MammothWebApi.BackgroundJobs
{
    public class JobFactory : IJobFactory
    {
        private Container container;
       
        public JobFactory(Container container)
        {
            this.container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return container.GetInstance(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}