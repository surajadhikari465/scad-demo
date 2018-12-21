namespace Icon.Dashboard.DataFileAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;

    /// <summary>
    /// This class represents an Icon application or process that runs as a Windows Service.
    /// </summary>
    public class IconService : IIconApplication, IDisposable
    {
        #region constructors
        public IconService()
        {
            this.TypeOfApplication = ApplicationTypeEnum.WindowsService;
        }

        public IconService(string name, string server, string configFilePath, string displayName = null)
            : this()
        {
            this.Name = name;
            this.Server = server;
            this.ConfigFilePath = configFilePath;
            this.DisplayName = String.IsNullOrWhiteSpace(displayName) ? name : displayName;
        }
        #endregion

        #region private fields

        private Lazy<Dictionary<string, string>> appSettings =
            new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>());
        private Lazy<Dictionary<string, string>> esbConnectionSettings =
            new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>());
        #endregion

        #region public properties

        public Dictionary<string, string> AppSettings
        {
            get { return this.appSettings.Value; }
        }

        public string ConfigFilePath { get; set; }

        public string DataFlowFrom { get; set; }

        public string DataFlowTo { get; set; }

        public Dictionary<string, string> EsbConnectionSettings
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
        /// True/false flag indicating whether the application is configured for
        ///   communicating with ESB Queues or not
        /// </summary>
        public bool HasEsbConfiguration
        {
            get
            {
                return this.EsbConnectionSettings.Any();
            }
        }

        public string NameAndServer
        {
            get
            {
                return $"{this.Name} | {this.Server}";
            }
        }
        #endregion

        #region public methods
        public void FindAndCreateInstance()
        {
            this.Instance = new ServiceController(this.Name, this.Server);
            SetValidCommands(GetStatus());
        }

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
                if (this.Instance != null)
                {
                    this.Instance.Refresh();
                    return this.Instance.Status.ToString();
                }
                return "Undefined";
            }
            catch (InvalidOperationException)
            {
                return "Undefined";
            }
        }

        public void Start(TimeSpan timeout, string[] args)
        {
            if (this.Instance == null) this.FindAndCreateInstance();
            if (args == null) args = Enumerable.Empty<string>().ToArray();

            switch (this.Instance.Status)
            {
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.StopPending:
                    this.Instance.Start(args);
                    this.Instance.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    break;
                case ServiceControllerStatus.ContinuePending:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.Running:
                default:
                    break;
            }
        }        

        public void Stop(TimeSpan timeout)
        {
            if (this.Instance == null) this.FindAndCreateInstance();
            
            switch (this.Instance.Status)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.ContinuePending:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.StartPending:
                    this.Instance.Stop();
                    this.Instance.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    break;
                case ServiceControllerStatus.Stopped:
                default:
                    break;
            }
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

        public void Execute(TimeSpan timeout, string command, string[] args = null)
        {
            if (!String.IsNullOrWhiteSpace(command))
            {
                switch (command.ToLower())
                {
                    case "start":
                        Start(timeout, args);
                        break;
                    case "stop":
                        Stop(timeout);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Dispose()
        {
            if (this.Instance != null)
            {
                this.Instance.Dispose();
            }
        }
        #endregion
    }
}