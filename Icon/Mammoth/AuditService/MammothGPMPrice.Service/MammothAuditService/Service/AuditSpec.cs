using System;
using System.Collections.Generic;
using System.Linq;

namespace Audit
{
    public class SpecInfo
    {
        public AuditConfigItem Config { get; private set; }
        public UploadConfigItem Profile { get; private set; }
        public Region[] Regions { get; private set; }
        public string DirPath { get; private set; }
        public string ConnectionString { get; private set; }
        public int CommandTimeOut { get; private set; }

        public SpecInfo(AuditConfigItem configItem, UploadConfigItem profileItem, Region[] sourceRegions, string tempDirPath, string sqlConnection, int commandTimeout = 600)
        {
            this.Config = configItem;
            this.Profile = profileItem;
            this.Regions = sourceRegions ?? new Region[0];
            this.DirPath = tempDirPath;
            this.ConnectionString = sqlConnection;
            this.CommandTimeOut = commandTimeout;

            var hsExcluded = string.IsNullOrEmpty(this.Config.ExcludeRegions) ? new HashSet<string>()
                                                                                                                                                : new HashSet<string>(this.Config.ExcludeRegions.Split(',').Select(x => x.Trim()), StringComparer.InvariantCultureIgnoreCase);
            this.Regions = sourceRegions.Where(x => !hsExcluded.Contains(x.Code)).ToArray();
        }

        public bool IsValid
        {
            get
            {
                return this.Config != null &&
                             this.Profile != null &&
                             this.Regions != null &&
                             this.Regions.Length > 0 &&
                             !String.IsNullOrWhiteSpace(this.DirPath) &&
                             !String.IsNullOrWhiteSpace(this.ConnectionString);
            }
        }
    }
}
