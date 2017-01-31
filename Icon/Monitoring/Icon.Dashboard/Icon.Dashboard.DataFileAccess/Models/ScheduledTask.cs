namespace Icon.Dashboard.DataFileAccess.Models
{
    using Microsoft.Win32.TaskScheduler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ScheduledTask : IApplication, IDisposable
    {
        public ScheduledTask()
        {
            this.TypeOfApplication = ApplicationTypeEnum.ScheduledTask;
        }

        public void FindAndCreateInstance()
        {
            //this.TypeOfApplication = ApplicationTypeEnum.ScheduledTask;
            try
            {
                this.Instance = new TaskService(this.Server).FindTask(this.Name);
                SetValidCommands(GetStatus());
            }
            catch (IOException)
            {
                this.Instance = null;
            }
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

        public EnvironmentEnum Environment { get; set; }

        public Task Instance { get; private set; }

        public string Name { get; set; }

        public string Server { get; set; }

        public virtual ApplicationTypeEnum TypeOfApplication { get; private set; }

        public List<string> ValidCommands { get; private set; }

        /// <summary>
        /// Name of the application as used in database logging (e.g. ICON app.App table), used
        ///  to look up associated logging data for the applicatoin
        /// </summary>
        public string LoggingName { get; set; }

        /// <summary>
        /// AppID used in the database when logging
        /// </summary>
        public int? LoggingID { get; set; }

        public string GetStatus()
        {
            try
            {
                if (this.Instance == null)
                {
                    FindAndCreateInstance();
                }
                if (this.Instance == null)
                {
                    return "Unknown";
                }

                //Unknown = 0,
                //Disabled = 1,
                //Queued = 2,.
                //Ready = 3,
                //Running = 4
                return this.Instance.State.ToString();
            }
            catch
            {
                //TODO how to handle?
                return "Unknown";
            }
        }

        public void Start(params string[] args)
        {
            if (this.Instance == null) this.FindAndCreateInstance();

            this.Instance.Run(args ?? Enumerable.Empty<string>().ToArray());
        }

        public void Stop()
        {
            this.Instance?.Stop();
        }

        public void Enable()
        {
            if (this.Instance == null) this.FindAndCreateInstance();
            if (this.Instance != null)
            {
                this.Instance.Enabled = true;
            }
        }

        public void Disable()
        {
            if (this.Instance == null) this.FindAndCreateInstance();
            if (this.Instance != null)
            {
                this.Instance.Enabled = false;
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
                    case "enable":
                        Enable();
                        break;
                    case "disable":
                        Disable();
                        break;
                    default:
                        break;
                }
            }
        }


        public DateTime? LastRun { get
            {
                if (this.Instance != null)
                {
                    return this.Instance.LastRunTime;
                }
                return null;
            }
        }

        public DateTime? NextRun
        {
            get
            {
                if (this.Instance != null)
                {
                    return this.Instance.NextRunTime;
                }
                return null;
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
                    case "Queued":
                    case "Ready":
                        return true;
                    case "Unknown":
                    case "Disabled":
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
                case "Disabled":
                    this.ValidCommands.Add("Enable");
                    break;
                case "Queued":
                case "Ready":
                    this.ValidCommands.Add("Run");
                    break;
                case "Unknown":
                default:
                    break;
            }
        }

        public void Dispose()
        {
            this.Instance?.Dispose();
        }
    }
}
