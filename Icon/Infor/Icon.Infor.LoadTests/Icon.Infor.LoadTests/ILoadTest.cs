namespace Icon.Infor.LoadTests
{
    public interface ILoadTest
    {
        /// <summary>
        /// Gets or Sets the Name of the test.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Configuration used to run the test.
        /// </summary>
        ILoadTestConfiguration Configuration { get; set; }
      
        /// <summary>
        /// Run the test.
        /// </summary>
        void Run();

        /// <summary>
        /// Send email notifications.
        /// </summary>
        void Notify();

        /// <summary>
        /// Stops the current execution of the test.
        /// </summary>
        void Stop();

        /// <summary>
        /// Is the test ready to be run.
        /// </summary>
        bool IsAbleToRun();

        /// <summary>
        /// Gets whether the test is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the current status of the test that is running.
        /// </summary>
        /// <returns></returns>
        ILoadTestStatus GetStatus();
    }
}
