using System;
using System.Collections.Generic;
using System.Linq;

namespace Audit
{
    public class SpecInfo
    {
        public AuditConfigItem Config { get; private set; }
        public UploadConfigItem Profile { get; private set; }
        public HashSet<string> Regions { get; private set; }
        public string DirPath { get; private set; }
        public Dictionary<string, string> ConnectionStrings { get; private set; }
        public int CommandTimeOut { get; private set; }

        public SpecInfo(AuditConfigItem configItem, UploadConfigItem profileItem, HashSet<string> sourceRegions, string tempDirPath, Dictionary<string, string> sqlConnections, int commandTimeout = 600)
        {
            this.Config = configItem;
            this.Profile = profileItem;
            this.Regions = sourceRegions ?? new HashSet<string>();
            this.DirPath = tempDirPath;
            this.ConnectionStrings = sqlConnections;
            this.CommandTimeOut = commandTimeout;

            var hsExcluded = string.IsNullOrEmpty(this.Config.ExcludeRegions)
              ? new HashSet<string>()
              : new HashSet<string>(this.Config.ExcludeRegions.Split(',').Select(x => x.Trim()), StringComparer.InvariantCultureIgnoreCase);
            this.Regions = new HashSet<string>(sourceRegions.Except(hsExcluded, StringComparer.InvariantCultureIgnoreCase), StringComparer.InvariantCultureIgnoreCase);
        }

        public bool IsValid
        {
            get
            {
                return this.Config != null &&
                             this.Profile != null &&
                             (this.Regions != null && this.Regions.Count > 0) &&
                             !String.IsNullOrWhiteSpace(this.DirPath) &&
                             (this.ConnectionStrings != null && this.ConnectionStrings.Count > 0);
            }
        }
    }
}
