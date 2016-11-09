namespace Icon.Dashboard.DataFileAccess.Models
{
    using System.Collections.Generic;

    public interface IApplication
    {
        Dictionary<string, string> AppSettings { get; }

        /// <summary>
        /// Gets or Sets the file path to the app.config
        /// </summary>
        string ConfigFilePath { get; set; }

        /// <summary>
        /// Indicates the system originating data used in the application (e.g. Icon, Irma, ESB, etc.)
        /// </summary>
        DataFlowSystemEnum DataFlowFrom { get; set; }

        /// <summary>
        /// Indicates the system receiving data used in the application (e.g. Icon, Irma, ESB, etc.)
        /// </summary>
        DataFlowSystemEnum DataFlowTo { get; set; }

        /// <summary>
        /// Gets or Sets the DisplayName to be user-friendly.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets the Environment the server belongs i.e. Dev, Test, QA
        /// </summary>
        EnvironmentEnum Environment { get; set; }

        /// <summary>
        /// Gets or Sets the Name of the e.g. windows service or task.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Server name where the component is running.
        /// </summary>
        string Server { get; set; }

        ApplicationTypeEnum TypeOfApplication { get; }

        /// <summary>
        /// Gets the current status of the application i.e. is it Running?
        /// </summary>
        /// <returns>Message about the status.</returns>
        string GetStatus();

        /// <summary>
        /// Reports whether the current status for the application is considered "good" (green) or bad
        /// </summary>
        bool StatusIsGreen { get; }

        /// <summary>
        /// Starts the application.
        /// </summary>
        void Start(params string[] args);

        /// <summary>
        /// Stops the application.
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Calls the process instance and command it to execute some behavior identified by the provided string
        /// </summary>
        /// <param name="command"></param>
        void Execute(string command, string[] args = null);

        /// <summary>
        /// List of valid commands (e.g. start, stop, run, enable) allowed for the process
        /// </summary>
        List<string> ValidCommands { get; }

        /// <summary>
        /// Name of the application as used in database logging (e.g. ICON app.App table), used
        ///  to look up associated logging data for the applicatoin
        /// </summary>
        string LoggingName { get; set; }
    }
}
