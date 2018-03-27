using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.Commands;
using System.Transactions;
using Newtonsoft.Json;
using System.Data;
using Mammoth.Framework;
using System.Data.SqlClient;
using WebSupport.DataAccess.Test.TestData;

namespace WebSupport.DataAccess.Test.Commands
{
    [TestClass]
    public class RegionGpmStatusCommandTests
    {
        private MammothContext mammothDbContext;
        private SqlConnection rawSqlConnection;
        private TransactionScope transaction;
        private GetGpmStatusQuery query;
        private GetGpmStatusParameters queryParams;
        private UpsertGpmStatusCommandHandler updateSingleCmd;
        private UpsertGpmStatusCommandParameters updateSingleCmdParams;
        private UpdateGpmStatusTableCommandHandler updateManyCmd;
        private UpdateGpmStatusTableCommandParameters updateManyCmdParams;
        private DeleteGpmStatusCommandHandler deleteCmd;
        private DeleteGpmStatusCommandParameters deleteCmdParams;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            mammothDbContext = new MammothContext();

            query = new GetGpmStatusQuery(mammothDbContext);
            queryParams = new GetGpmStatusParameters();
            updateSingleCmd = new UpsertGpmStatusCommandHandler(mammothDbContext);
            updateSingleCmdParams = new UpsertGpmStatusCommandParameters();
            deleteCmd = new DeleteGpmStatusCommandHandler(mammothDbContext);
            deleteCmdParams = new DeleteGpmStatusCommandParameters();
            updateManyCmd = new UpdateGpmStatusTableCommandHandler(mammothDbContext, query, updateSingleCmd, deleteCmd);
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters();

            var connectionString = ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString;
            rawSqlConnection = new SqlConnection(connectionString);
            rawSqlConnection.Execute(RegionGpmStatusTestData.SqlForInsertingTestRegions);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            mammothDbContext.Dispose();
        }

        [TestMethod]
        public void UpdateGpmStatus_WhenRegionNotPreviouslySetUp_SavesNewEntryWithExpectedData()
        {
            //Given
            updateSingleCmdParams.Region = "XY";
            updateSingleCmdParams.IsGpmEnabled = true;

            //When
            updateSingleCmd.Execute(updateSingleCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses
                .First(s => s.Region == updateSingleCmdParams.Region);
            Assert.AreEqual(updateSingleCmdParams.Region, currentData.Region);
            Assert.AreEqual(updateSingleCmdParams.IsGpmEnabled, currentData.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatus_WhenRegionHasGpmDisabled_ommandCanEnableIt()
        {
            //Given
            updateSingleCmdParams.Region = RegionGpmStatusTestData.ExistingRegionNotOnGpm;
            updateSingleCmdParams.IsGpmEnabled = RegionGpmStatusTestData.GpmOn;

            //When
            updateSingleCmd.Execute(updateSingleCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses
                .First(s => s.Region == updateSingleCmdParams.Region);
            Assert.AreEqual(updateSingleCmdParams.Region, currentData.Region);
            Assert.AreEqual(updateSingleCmdParams.IsGpmEnabled, currentData.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatus_WhenRegionHasGpmEnabled_CommandCanDisableIt()
        {
            //Given
            updateSingleCmdParams.Region = RegionGpmStatusTestData.ExistingRegionNotOnGpm;
            updateSingleCmdParams.IsGpmEnabled = RegionGpmStatusTestData.GpmOff;

            //When
            updateSingleCmd.Execute(updateSingleCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses
                .First(s => s.Region == updateSingleCmdParams.Region);
            Assert.AreEqual(updateSingleCmdParams.Region, currentData.Region);
            Assert.AreEqual(updateSingleCmdParams.IsGpmEnabled, currentData.IsGpmEnabled);
        }

        [TestMethod]
        public void DeleteGpmStatus_WhenTheCommandParameterIsBad_ThenTheCommandDoesNothing()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            deleteCmdParams.Region = null;

            //When
            deleteCmd.Execute(deleteCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(existingCount, currentData.Count);
        }

        [TestMethod]
        public void DeleteGpmStatus_WhenRegionIsUnknown_CommandDoesNothing()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();

            //When
            deleteCmdParams.Region = RegionGpmStatusTestData.NonExistentRegion;
            deleteCmd.Execute(deleteCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(existingCount, currentData.Count);
        }

        [TestMethod]
        public void DeleteGpmStatus_WhenRegionHasGpmEnabled_CanBeDeleted()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount - 1;

            //When
            deleteCmdParams.Region = RegionGpmStatusTestData.ExistingRegionOnGpm;
            deleteCmd.Execute(deleteCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);
            var deletedRegion = currentData
                .FirstOrDefault(rgs => rgs.Region == deleteCmdParams.Region);
            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public void DeleteGpmStatus_WhenRegionHasGpmDisabled_CanBeDeleted()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount - 1;

            //When
            deleteCmdParams.Region = RegionGpmStatusTestData.ExistingRegionNotOnGpm;
            deleteCmd.Execute(deleteCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);
            var deletedRegion = currentData
                .FirstOrDefault(rgs => rgs.Region == deleteCmdParams.Region);
            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenAddingSingleRegion_RegionIsAddedWithExpectedData()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount + 1;
            var regionToAddWithGpmOn = new RegionGpmStatus
            {
                Region = "AB",
                IsGpmEnabled = RegionGpmStatusTestData.GpmOn
            };
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Add(regionToAddWithGpmOn);

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var addedRegionAB = currentData.FirstOrDefault(rgs => rgs.Region == regionToAddWithGpmOn.Region);
            Assert.IsNotNull(addedRegionAB);
            Assert.AreEqual(RegionGpmStatusTestData.GpmOn, addedRegionAB.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenAddingMultipleRegions_RegionsAreUpdatedWithExpectedData()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount + 2;
            var regionToAddWithGpmOn = new RegionGpmStatus
            {
                Region = "AB",
                IsGpmEnabled = RegionGpmStatusTestData.GpmOn
            };
            var regionToAddWithGpmOff = new RegionGpmStatus
            {
                Region = "CD",
                IsGpmEnabled = RegionGpmStatusTestData.GpmOff
            };
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.AddRange(new RegionGpmStatus[] { regionToAddWithGpmOn, regionToAddWithGpmOff });
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);

            //When
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var addedRegionWithGpmOn = currentData.FirstOrDefault(rgs => rgs.Region == regionToAddWithGpmOn.Region);
            Assert.IsNotNull(addedRegionWithGpmOn);
            Assert.AreEqual(RegionGpmStatusTestData.GpmOn, addedRegionWithGpmOn.IsGpmEnabled);

            var addedRegionWithGpmOff = currentData.FirstOrDefault(rgs => rgs.Region == regionToAddWithGpmOff.Region);
            Assert.IsNotNull(addedRegionWithGpmOff);
            Assert.AreEqual(RegionGpmStatusTestData.GpmOff, addedRegionWithGpmOff.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenUpdatingSingleRegion_RegionCanHaveGpmTurnedOn()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount;
            var updatedStatus = RegionGpmStatusTestData.GpmOn;
            var regionToUpdate = new RegionGpmStatus
            {
                Region = RegionGpmStatusTestData.ExistingRegionOnGpm,
                IsGpmEnabled = updatedStatus
            };
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Single(r => r.Region == regionToUpdate.Region).IsGpmEnabled = updatedStatus;

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var updatedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToUpdate.Region);
            Assert.IsNotNull(updatedRegion);
            Assert.AreEqual(updatedStatus, updatedRegion.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenUpdatingSingleRegion_RegionCanHaveGpmTurnedOff()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount;
            var updatedStatus = RegionGpmStatusTestData.GpmOff;
            var regionToUpdate = new RegionGpmStatus
            {
                Region = RegionGpmStatusTestData.ExistingRegionOnGpm,
                IsGpmEnabled = updatedStatus
            };
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Single(r => r.Region == regionToUpdate.Region).IsGpmEnabled = updatedStatus;

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var updatedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToUpdate.Region);
            Assert.IsNotNull(updatedRegion);
            Assert.AreEqual(updatedStatus, updatedRegion.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenRemovingSingleRegionOnGpm_RegionIsRemoved()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount - 1;
            var regionToRemove = new RegionGpmStatus
            {
                Region = RegionGpmStatusTestData.ExistingRegionOnGpm,
                IsGpmEnabled = RegionGpmStatusTestData.GpmOn
            };
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Remove(regions.Single(r=>r.Region== regionToRemove.Region));

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var deletedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToRemove.Region);
            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenRemovingSingleRegionNotOnGpm_RegionIsRemoved()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount - 1;
            var regionToRemove = RegionGpmStatusTestData.ExistingRegionNotOnGpm;
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Remove(regions.Single(r => r.Region == regionToRemove));

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var deletedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToRemove);
            Assert.IsNull(deletedRegion);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenAddingOneRegionAndUpdatingAnother_RegionsAreAddedAndUpdated()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount + 1;
            var regionToAdd = new RegionGpmStatus
            {
                Region = "AB",
                IsGpmEnabled = RegionGpmStatusTestData.GpmOn
            };
            var updatedStatus = RegionGpmStatusTestData.GpmOn;
            var regionToUpdate = new RegionGpmStatus
            {
                Region = RegionGpmStatusTestData.ExistingRegionNotOnGpm,
                IsGpmEnabled = updatedStatus
            };
            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Add(regionToAdd);
            regions.Single(r => r.Region == regionToUpdate.Region).IsGpmEnabled = regionToUpdate.IsGpmEnabled;

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var addedRegionWithGpmOn = currentData.FirstOrDefault(rgs => rgs.Region == regionToAdd.Region);
            Assert.AreEqual(RegionGpmStatusTestData.GpmOn, addedRegionWithGpmOn.IsGpmEnabled);

            var updatedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToUpdate.Region);
            Assert.AreEqual(updatedStatus, updatedRegion.IsGpmEnabled);
        }

        [TestMethod]
        public void UpdateGpmStatusTable_WhenAddingOneRegionAndUpdatingAnotherAndDeleteingAnother_RegionsAreAddedAndUpdatedAndDeleted()
        {
            //Given
            var existingCount = rawSqlConnection.Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount).Single();
            var expectedCount = existingCount + 1 - 1;
            var regionToAdd = new RegionGpmStatus
            {
                Region = "AB",
                IsGpmEnabled = RegionGpmStatusTestData.GpmOn
            };
            var updatedStatus = RegionGpmStatusTestData.GpmOn;
            var regionToUpdate = new RegionGpmStatus
            {
                Region = RegionGpmStatusTestData.ExistingRegionNotOnGpm,
                IsGpmEnabled = updatedStatus
            };
            var regionToDelete = new RegionGpmStatus
            {
                Region = RegionGpmStatusTestData.ExistingRegionOnGpm,
                IsGpmEnabled = RegionGpmStatusTestData.GpmOn
            };

            var regions = RegionGpmStatusTestData.TestRegions;
            regions.Single(r => r.Region == regionToUpdate.Region).IsGpmEnabled = regionToUpdate.IsGpmEnabled;
            regions.Add(regionToAdd);
            regions.Remove(regions.Single(r => r.Region == regionToDelete.Region));

            //When
            updateManyCmdParams = new UpdateGpmStatusTableCommandParameters(regions);
            updateManyCmd.Execute(updateManyCmdParams);

            //Then
            var currentData = mammothDbContext.RegionGpmStatuses.ToList();
            Assert.AreEqual(expectedCount, currentData.Count);

            var addedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToAdd.Region);
            Assert.AreEqual(RegionGpmStatusTestData.GpmOn, addedRegion.IsGpmEnabled);

            var updatedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToUpdate.Region);
            Assert.AreEqual(updatedStatus, updatedRegion.IsGpmEnabled);

            var deletedRegion = currentData.FirstOrDefault(rgs => rgs.Region == regionToDelete.Region);
            Assert.IsNull(deletedRegion);
        }
    }
}
