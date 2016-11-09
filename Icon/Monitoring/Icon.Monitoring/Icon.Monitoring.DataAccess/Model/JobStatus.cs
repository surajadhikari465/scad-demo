namespace Icon.Monitoring.DataAccess.Model
{
    using System;
    using Common.Enums;

    public class JobStatus
    {
        public IrmaRegions Region { get; set; }

        public string Classname { get; set; }

        public string Status { get; set; }

        public DateTime? LastRun { get; set; }
    }
}
