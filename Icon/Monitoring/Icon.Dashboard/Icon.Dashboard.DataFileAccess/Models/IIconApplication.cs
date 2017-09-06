namespace Icon.Dashboard.DataFileAccess.Models
{
    using System;
    using System.Collections.Generic;

    public interface IIconApplication
    {
        /// <summary>
        /// String Dictionary representing key/value pairs from the appSettings element in the config file
        /// </summary>
        Dictionary<string, string> AppSettings { get; }

        /// <summary>
        /// String Dictionary representing a subset of key/value pairs from the appSettings element which 
        ///   relate to ESB Connection settings (if present)
        /// </summary>
        Dictionary<string, string> EsbConnectionSettings { get; }

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
        void Start(TimeSpan timeout, params string[] args);

        /// <summary>
        /// Stops the application.
        /// </summary>
        void Stop(TimeSpan timeout);
        
        /// <summary>
        /// Calls the process instance and command it to execute some behavior identified by the provided string
        /// </summary>
        /// <param name="command"></param>
        void Execute(TimeSpan timeout, string command, string[] args = null);

        /// <summary>
        /// List of valid commands (e.g. start, stop, run, enable) allowed for the process
        /// </summary>
        List<string> ValidCommands { get; }

        /// <summary>
        /// Name of the application as used in database logging (e.g. ICON app.App table), used
        ///  to look up associated logging data for the applicatoin
        /// </summary>
        string LoggingName { get; set; }

        /// <summary>
        /// True/false flag indicating whether the application is configured for
        ///   communicating with ESB Queues or not (whether it has app settings 
        ///   needed for configuring ESB connections)
        /// </summary>
        bool HasEsbConfiguration { get;  }

        string NameAndServer { get; }
    }
}
