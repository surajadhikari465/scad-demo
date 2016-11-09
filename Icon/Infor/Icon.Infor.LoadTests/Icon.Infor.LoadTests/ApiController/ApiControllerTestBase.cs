using Icon.Infor.LoadTests.LoadTestSteps;
using System;
using System.Linq;

namespace Icon.Infor.LoadTests.ApiController
{
    public abstract class ApiControllerTestBase : LoadTestBase
    {
        protected DateTime initialPopulateDataTime;

        protected abstract string ApiControllerName { get; }

        #region Ctors

        public ApiControllerTestBase() : base() { }

        public ApiControllerTestBase(ILoadTestConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        public override void Run()
        {
            initialPopulateDataTime = DateTime.Now;

            base.Run();
        }
        
        protected override void Setup()
        {
            foreach (var apiController in Configuration.ApplicationInstances)
            {
                new StartScheduledTaskStep
                {
                    Name = ApiControllerName,
                    Server = apiController.Server
                }.Execute();
            }
        }

        public override void Stop()
        {
            foreach (var apiController in Configuration.ApplicationInstances)
            {
                new StopScheduledTaskStep
                {
                    Name = ApiControllerName,
                    Server = apiController.Server
                }.Execute();
            }

            this.populateDataTimer.Stop();
            this.testStopwatch.Stop();
        }

        public override bool IsAbleToRun()
        {
            var canRun = this.Configuration.ApplicationInstances.All(
                a => (new ConfirmTaskIsEnabledStep { Name = ApiControllerName, Server = a.Server }.Execute()));

            return canRun;
        }
    }
}
