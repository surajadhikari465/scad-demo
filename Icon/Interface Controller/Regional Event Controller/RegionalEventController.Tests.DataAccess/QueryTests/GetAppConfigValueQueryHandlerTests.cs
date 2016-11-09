using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using RegionalEventController.DataAccess.Queries;
using System.Data.Entity;
using System.Linq;
using System.Collections;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetAppConfigValueQueryHandlerTests
    {
        private IrmaContext context;
        private GetAppConfigValueQuery query;
        private GetAppConfigValueQueryHandler handler;
        private DbContextTransaction transaction;
        private AppConfigKey key;
        private AppConfigEnv environment;
        private AppConfigApp application;
        private AppConfigValue value;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.query = new GetAppConfigValueQuery();
            this.handler = new GetAppConfigValueQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();

            this.environment = this.context.AppConfigEnv.First(e => e.Name == "TEST");
            this.application = this.context.AppConfigApp.First(e => e.Name.ToLower() == "irma client");

            // Add new key and value to test with
            this.key = new AppConfigKey { Name = "ReconTestKey", Deleted = false, LastUpdate = DateTime.Now, LastUpdateUserID = 0 };
            this.value = new AppConfigValue
            {
                ApplicationID = this.application.ApplicationID,
                EnvironmentID = this.environment.EnvironmentID,
                Deleted = false,
                KeyID = this.key.KeyID,
                LastUpdate = DateTime.Now,
                LastUpdateUserID = 0,
                Value = "ReconTestValue"
            };

            this.context.AppConfigValue.Add(this.value);
            this.context.AppConfigKey.Add(this.key);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetAppConfigValue_ApplicationAndKey_QueryReturnExpectedValue()
        {
            // Given
            string expected = this.value.Value;
            this.query.applicationName = this.application.Name;
            this.query.configurationKey = "ReconTestKey";

            // When
            string actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(actual, expected, "The return configuration value did not match the expected value.");
        }
    }
}
