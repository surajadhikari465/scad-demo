using System;

namespace Icon.FeatureFlags
{
    internal class FeatureFlag
    {
        public int FeatureFlagId { get; set; }
        public string FlagName { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
