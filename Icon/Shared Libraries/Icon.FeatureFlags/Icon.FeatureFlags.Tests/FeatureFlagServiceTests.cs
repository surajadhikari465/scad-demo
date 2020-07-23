using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.FeatureFlags.Tests
{
    [TestClass]
    public class FeatureFlagServiceTests
    {
        private FeatureFlagService featureFlagService;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection("Data Source=icon-db01-dev;Initial Catalog=Icon;Integrated Security=True;");
            featureFlagService = new FeatureFlagService(sqlConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            sqlConnection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void IsEnabled_FeatureFlagIsEnabled_ReturnsTrue()
        {
            //Given
            string featureFlagName = "AutomatedTestFeatureFlag";
            InsertFeatureFlag(true, featureFlagName);

            //When
            var result = featureFlagService.IsEnabled(featureFlagName);

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsEnabled_FeatureFlagIsDisabled_ReturnsFalse()
        {
            //Given
            string featureFlagName = "AutomatedTestFeatureFlag";
            InsertFeatureFlag(false, featureFlagName);

            //When
            var result = featureFlagService.IsEnabled(featureFlagName);

            //Then
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsEnabled_FeatureFlagDoesNotExist_ReturnsFalse()
        {
            //Given
            string featureFlagName = "AutomatedTestFeatureFlag";

            //When
            var result = featureFlagService.IsEnabled(featureFlagName);

            //Then
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Feature Flag Name cannot be null, empty, or whitespace.")]
        public void IsEnabled_FeatureFlagNameIsNull_ThrowsException()
        {
            //When
            var result = featureFlagService.IsEnabled(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Feature Flag Name cannot be null, empty, or whitespace.")]
        public void IsEnabled_FeatureFlagNameIsEmpty_ThrowsException()
        {
            //When
            var result = featureFlagService.IsEnabled(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Feature Flag Name cannot be null, empty, or whitespace.")]
        public void IsEnabled_FeatureFlagNameIsWhitespace_ThrowsException()
        {
            //When
            var result = featureFlagService.IsEnabled("   ");
        }

        private void InsertFeatureFlag(bool enabled, string featureFlagName)
        {
            sqlConnection.Execute("INSERT dbo.FeatureFlag VALUES (@FlagName, @Enabled, '', GETUTCDATE(), GETUTCDATE(), '')", new { FlagName = featureFlagName, Enabled = enabled });
        }
    }
}
