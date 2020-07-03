using System;

namespace Icon.Web.DataAccess.Models
{
    public class FeatureFlagModel
    {
        public int FeatureFlagId { get; set; }

        public string FlagName { get; set; }

        public Boolean Enabled { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDateUtc { get; set; }

        public DateTime? LastModifiedDateUtc { get; set; }

        public string LastModifiedBy { get; set; }
    }
}