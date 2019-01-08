using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class PurgeTableInfo
    {
        public int Id { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ReferenceColumn { get; set; }
        public short DaysToKeep { get; set; }
        public byte TimeToStart { get; set; }
        public byte TimeToEnd { get; set; }
        public bool IsDailyPurge { get; set; }
        public bool IsDailyPurgeCompleted { get; set; }
        public string PurgeJobName { get; set; }
        public DateTime? LastPurgedDate { get; set; }
    }
}
