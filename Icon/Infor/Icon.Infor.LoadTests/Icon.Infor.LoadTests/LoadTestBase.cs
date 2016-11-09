using Icon.Infor.LoadTests.LoadTestSteps;
using System;
using System.Diagnostics;
using System.Timers;

namespace Icon.Infor.LoadTests
{
    public abstract class LoadTestBase : ILoadTest, IDisposable
    {
        private const double DefaultMonitorInterval = 1000;

        #region Fields

        protected Stopwatch testStopwatch;
        protected Timer populateDataTimer;
        protected Timer updateStatusTimer;
        protected ILoadTestStatus currentStatus;
        protected ILoadTestStatus lastRunStatus;

        #endregion

        #region Properties

        protected abstract string TestEmailSubject { get; }
        public ILoadTestConfiguration Configuration { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get { return this.testStopwatch.IsRunning; } }

        #endregion

        #region Ctors

        public LoadTestBase()
        {
            this.testStopwatch = new Stopwatch();
            this.populateDataTimer = new Timer();
            this.currentStatus = LoadTestStatus.Default;
            this.lastRunStatus = LoadTestStatus.Default;
            this.updateStatusTimer = new Timer(DefaultMonitorInterval);
            this.updateStatusTimer.Elapsed += this.UpdateStatus;
        }

        public LoadTestBase(ILoadTestConfiguration configuration)
            : this()
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Public Methods

        public virtual void Run()
        {
            if (Configuration.TestRunTime.TotalSeconds > 0 && Configuration.PopulateTestDataInterval.TotalSeconds > 0)
            {
                this.populateDataTimer.Interval = Configuration.PopulateTestDataInterval.TotalMilliseconds;
                this.populateDataTimer.Elapsed += PopulateTestDataElapsedCallback;
            }

            this.PopulateTestData();
            this.Setup();

            this.populateDataTimer.Start();
            this.testStopwatch.Start();

            this.updateStatusTimer.Start();
        }

        public void Notify()
        {
            var results = lastRunStatus;

            new EmailNotificationStep(
                results,
                Configuration.EmailRecipients.ToArray(),
                TestEmailSubject)
                .Execute();
        }

        public ILoadTestStatus GetStatus()
        {
            if (IsRunning)
                return currentStatus;
            else
                return LoadTestStatus.Default;
        }

        #endregion

        #region Protected Methods 

        protected void UpdateStatusBasedOnElapsedTime(ILoadTestStatus status)
        {
            status.ElapsedTime = testStopwatch.Elapsed.TotalSeconds;

            if(testStopwatch.Elapsed.TotalMinutes > Configuration.TestRunTime.TotalMinutes
                && status.UnprocessedEntities == 0)
            {
                testStopwatch.Stop();
            }
        }

        protected void PopulateTestDataElapsedCallback(object sender, ElapsedEventArgs e)
        {
            populateDataTimer.Stop();

            if (testStopwatch.Elapsed.TotalMinutes > Configuration.TestRunTime.TotalMinutes)
            {
                return;
            }

            this.PopulateTestData();

            populateDataTimer.Start();
        }

        #endregion 

        #region Abstract Methods

        protected abstract void PopulateTestData();
        protected abstract void Setup();
        protected abstract void UpdateCurrentStatus();
        public abstract void Stop();
        public abstract bool IsAbleToRun();

        #endregion

        #region Private Methods

        private void UpdateStatus(object sender, ElapsedEventArgs e)
        {
            this.updateStatusTimer.Stop();

            this.UpdateCurrentStatus();

            if (this.IsRunning)
            {
                this.updateStatusTimer.Start();
            }
            else
            {
                this.lastRunStatus = currentStatus;
                this.Notify();
            }
        }

        public void Dispose()
        {
            if (this.updateStatusTimer != null)
            {
                this.updateStatusTimer.Elapsed -= this.UpdateStatus;
                this.updateStatusTimer.Dispose();
            }
                
        }

        #endregion
    }
}
