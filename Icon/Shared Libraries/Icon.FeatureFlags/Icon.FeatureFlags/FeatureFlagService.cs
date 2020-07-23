using Dapper;
using System;
using System.Data;

namespace Icon.FeatureFlags
{
    public class FeatureFlagService : IFeatureFlagService
    {
        private readonly IDbConnection dbConnection;

        public FeatureFlagService(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public bool IsEnabled(string featureFlagName)
        {
            if (string.IsNullOrWhiteSpace(featureFlagName))
            {
                throw new ArgumentException("Feature Flag Name cannot be null, empty, or whitespace.", nameof(featureFlagName));
            }
            var featureFlag = dbConnection.QueryFirstOrDefault<FeatureFlag>("SELECT Enabled FROM dbo.FeatureFlag WHERE FlagName = @FlagName", new { FlagName = featureFlagName });
            if (featureFlag == null)
            {
                return false;
            }
            else
            {
                return featureFlag.Enabled;
            }
        }
    }
}
