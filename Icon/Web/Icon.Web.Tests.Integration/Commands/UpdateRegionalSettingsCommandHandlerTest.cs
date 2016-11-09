using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateRegionalSettingsCommandHandlerTest
    {
        private UpdateRegionalSettingsCommandHandler commandHandler;
        private IconContext context;
        private RegionalSettings regionalSettings;
        private int posDeptSubTeamSettingsId;
        int regionId;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.commandHandler = new UpdateRegionalSettingsCommandHandler(context);
            int itemSectionID = context.SettingSection.Where(ss => ss.SectionName.Equals("Item")).Select(ss => ss.SettingSectionId).First();
            int financialSectionID = context.SettingSection.Where(ss => ss.SectionName.Equals("Financial")).Select(ss => ss.SettingSectionId).First();
            posDeptSubTeamSettingsId = context.Settings.Where(s => s.KeyName.Equals(ConfigurationConstants.SendSubTeamUpdatesToIRMASettingsKey) && s.SettingSectionId == financialSectionID).Select(ss => ss.SettingsId).First();
            regionId = this.context.Regions.Where(r => r.RegionName == "RM").Select(s => s.RegionId).First();
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
            context.Dispose();
        }
            
        [TestMethod]
        public void UpdateRegionalSettings_SetItemSubscriptionToTrue()
        {
            // Given
            regionalSettings = this.context.RegionalSettings.Where(rs => rs.RegionId == regionId).First();
            regionalSettings.Value = false;
            this.context.SaveChanges();
            
            // When
            commandHandler.Execute(new UpdateRegionalSettingsCommand(){RegionalSettingId = regionalSettings.RegionalSettingsId, SettingValue = true});

            // Then
            var result = context.RegionalSettings.SingleOrDefault(rs => rs.RegionalSettingsId == regionalSettings.RegionalSettingsId);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value, true);
        }

        [TestMethod]
        public void UpdateRegionalSettings_SetPosDeptSubscriptionToTrue()
        {
            // Given
            regionalSettings = this.context.RegionalSettings.Where(rs => rs.RegionId == regionId && rs.SettingsId == posDeptSubTeamSettingsId).First();
            regionalSettings.Value = false;
            this.context.SaveChanges();            

            // When
            commandHandler.Execute(new UpdateRegionalSettingsCommand() { RegionalSettingId = regionalSettings.RegionalSettingsId, SettingValue = true });

            // Then
            var result = context.RegionalSettings.SingleOrDefault(rs => rs.RegionalSettingsId == regionalSettings.RegionalSettingsId);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value, true);
        }
    }
}
