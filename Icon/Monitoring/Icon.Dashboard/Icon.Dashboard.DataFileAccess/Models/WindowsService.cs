namespace Icon.Dashboard.DataFileAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;

    /// <summary>
    /// This class represents a process that is run via Windows Service.
    /// </summary>
    public class WindowsService : IApplication, IDisposable
    {
        public WindowsService()
        {
            this.TypeOfApplication = ApplicationTypeEnum.WindowsService;
        }

        public void FindAndCreateInstance()
        {
            this.Instance = new ServiceController(this.Name, this.Server);
            SetValidCommands(GetStatus());
        }

        private Lazy<Dictionary<string, string>> appSettings = new Lazy<Dictionary<string, string>>(
            () => new Dictionary<string, string>());
        private Lazy<List<Dictionary<string, string>>> esbConnectionSettings = new Lazy<List<Dictionary<string, string>>>(
            () => new List<Dictionary<string, string>>());

        public Dictionary<string, string> AppSettings
        {
            get { return this.appSettings.Value; }
        }

        public string ConfigFilePath { get; set; }

        public string DataFlowFrom { get; set; }

        public string DataFlowTo { get; set; }

        public List<Dictionary<string, string>> EsbConnectionSettings
        {
            get { return this.esbConnectionSettings.Value; }
        }

        public string DisplayName { get; set; }

        public ServiceController Instance { get; private set; }

        public string Name { get; set; }

        public string Server { get; set; }

        public virtual ApplicationTypeEnum TypeOfApplication { get; private set; }

        /// <summary>
        /// Name of the application as used in database logging (e.g. ICON app.App table), used
        ///  to look up associated logging data for the applicatoin
        /// </summary>
        public string LoggingName { get; set; }

        /// <summary>
        /// AppID used in the database when logging
        /// </summary>
        public int? LoggingID { get; set; }

        public List<string> ValidCommands { get; private set; }

        /// <summary>
        /// Refreshes the information about the instance of the windows service and return the status.
        /// </summary>
        /// <returns>
        /// A <see cref="System.ServiceProcess.ServiceControllerStatus"/> as string
        /// or Undefined if the applicaiton is not configured correctly.
        /// </returns>
        public string GetStatus()
        {
            if (this.Instance == null) this.FindAndCreateInstance();
            if (this.Instance == null)
            {
                return "Unknown";
            }
            try
            {
                this.Instance?.Refresh();
                return this.Instance?.Status.ToString() ?? "Undefined";
            }
            catch (InvalidOperationException)
            {
                return "Undefined";
            }
        }

        public void Start(string[] args)
        {
            if (this.Instance == null) this.FindAndCreateInstance();
            this.Instance?.Start(args ?? Enumerable.Empty<string>().ToArray());
            this.Instance?.WaitForStatus(ServiceControllerStatus.Running);
        }

        public void Stop()
        {
            if (this.Instance == null) this.FindAndCreateInstance();

            this.Instance?.Stop();
            this.Instance?.WaitForStatus(ServiceControllerStatus.Stopped);
        }

        public bool StatusIsGreen
        {
            get
            {
                var status = GetStatus();
                switch (status)
                {
                    case "Running":
                        return true;
                    case "Stopped":
                    case "StartPending":
                    case "StopPending":
                    case "ContinuePending":
                    case "PausePending":
                    case "Paused":
                    default:
                        return false;
                }
            }
        }

        private void SetValidCommands(string status)
        {
            this.ValidCommands = new List<string>();
            switch (status)
            {
                case "Running":
                    this.ValidCommands.Add("Stop");
                    break;
                case "Stopped":
                    this.ValidCommands.Add("Start");
                    break;
                case "StartPending":
                case "StopPending":
                case "ContinuePending":
                case "PausePending":
                case "Paused":
                    this.ValidCommands.Add("Start");
                    this.ValidCommands.Add("Stop");
                    break;
                case "Undefined":
                default:
                    break;
            }
        }

        public void Execute(string command, string[] args = null)
        {
            if (!String.IsNullOrWhiteSpace(command))
            {
                switch (command.ToLower())
                {
                    case "start":
                        Start(args);
                        break;
                    case "stop":
                        Stop();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Dispose()
        {
            this.Instance?.Dispose();
        }
    }
}