namespace Icon.Dashboard.DataFileAccess.Models
{
    using System.Collections.Generic;

    public interface IApplication
    {
        /// <summary>
        /// String Dictionary representing key/value pairs from the appSettings element in the config file
        /// </summary>
        Dictionary<string, string> AppSettings { get; }

        /// <summary>
        /// String Dictionary representing key/value pairs from the esbConnections element in the config file, if it is present
        /// </summary>
        List<Dictionary<string, string>> EsbConnectionSettings { get; }

        /// <summary>
        /// Gets or Sets the file path to the app.config
        /// </summary>
        string ConfigFilePath { get; set; }

        /// <summary>
        /// Indicates the system providing or originating the data used by the application (e.g. Icon, Irma, ESB, etc.)
        /// </summary>
        string DataFlowFrom { get; set; }

        /// <summary>
        /// Indicates the system receiving data used in the application (e.g. Icon, Irma, ESB, etc.)
        /// </summary>
        string DataFlowTo { get; set; }
        
        /// <summary>
        /// Gets or Sets the DisplayName to be user-friendly.
        /// </summary>
        string DisplayName { get; set; }

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
        /// Gets or Sets the id for the app, as used in the app.App.AppID column
        /// </summary>
        int? LoggingID { get; set; }

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
