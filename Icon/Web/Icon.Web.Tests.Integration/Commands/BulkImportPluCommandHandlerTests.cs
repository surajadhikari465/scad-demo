using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class BulkImportPluCommandHandlerTests
    {
        private IconContext context;
        private BulkImportPluCommandHandler importHandler;
        private BulkImportCommand<BulkImportPluModel> importData;
        private Mock<ILogger> mockLogger;
        private List<PLUMap> originalMap;

        [TestInitialize]
        public void SetupTestData()
        {
            context = new IconContext();
            importData = new BulkImportCommand<BulkImportPluModel>();
            mockLogger = new Mock<ILogger>();
            importHandler = new BulkImportPluCommandHandler(this.mockLogger.Object, this.context);

            originalMap = context.PLUMap
                .Where(m => m.Item.ScanCode
                    .Any(s => s.ScanCodeType.scanCodeTypeDesc.Contains("PLU")
                        && (s.scanCode.Contains("6864") || s.scanCode.Contains("6865")))).ToList();

            foreach (var map in this.originalMap)
            {
                context.Entry(map).State = EntityState.Detached;
            }    
        }

        [TestCleanup]
        public void CleanupTestData()
        {
            foreach (var map in this.originalMap)
            {
                context.PLUMap.Attach(map);

                var entry = context.Entry(map);
                entry.Property(p => p.flPLU).IsModified = true;
                entry.Property(p => p.maPLU).IsModified = true;
                entry.Property(p => p.mwPLU).IsModified = true;
                entry.Property(p => p.naPLU).IsModified = true;
                entry.Property(p => p.ncPLU).IsModified = true;
                entry.Property(p => p.nePLU).IsModified = true;
                entry.Property(p => p.pnPLU).IsModified = true;
                entry.Property(p => p.rmPLU).IsModified = true;
                entry.Property(p => p.soPLU).IsModified = true;
                entry.Property(p => p.spPLU).IsModified = true;
                entry.Property(p => p.swPLU).IsModified = true;
                entry.Property(p => p.ukPLU).IsModified = true;
            }

            context.SaveChanges();
            context.Dispose();
        }

        [TestCategory("Integration"), TestCategory("BulkImport-Plu")]
        [TestMethod]
        public void PluBulkImportExecute_BulkImportPluObject_PluMappingUpdated()
        {
            // Given.
            importData = new BulkImportCommand<BulkImportPluModel>
            {
                UserName = "TestIntegrationUser",
                BulkImportData = new List<BulkImportPluModel>
                {
                    new BulkImportPluModel
                    {
                        NationalPlu = "6864",
                        flPLU = "34561",
                        maPLU = "23452",
                        mwPLU = "76543",
                        naPLU = "123456",
                        nePLU = "98765",
                        ncPLU = "44444",
                        pnPLU = "76543",
                        rmPLU = "55555",
                        spPLU = "15648",
                        soPLU = "77777",
                        swPLU = "50012",
                        ukPLU = "50012"
                    },

                    new BulkImportPluModel
                    {
                        NationalPlu = "6865",
                        flPLU = "23453",
                        maPLU = "23453",
                        mwPLU = "77777",
                        naPLU = "897845",
                        nePLU = "99999",
                        ncPLU = "23453",
                        pnPLU = "12345",
                        rmPLU = "12346",
                        spPLU = "789416",
                        soPLU = "45678",
                        swPLU = "50013",
                        ukPLU = "50013"
                    }
                }
            };

            // When.
            importHandler.Execute(this.importData);

            // Then.
            // Query PLUMap table for above National PLUs to get actual mapped values after Bulk Import
            List<PLUMap> updatedMap = context.PLUMap
                .Where(m => m.Item.ScanCode
                    .Any(s => s.ScanCodeType.scanCodeTypeDesc.Contains("PLU")
                        && (s.scanCode.Contains("6864") || s.scanCode.Contains("6865")))).ToList();
            
            string actualFlPlu = updatedMap.Where(m => m.Item.ScanCode.Any(s => s.scanCode == "6864")).FirstOrDefault().flPLU;
            string actualSwPlu = updatedMap.Where(m => m.Item.ScanCode.Any(s => s.scanCode == "6865")).FirstOrDefault().swPLU;

            // Detach updateMap from Context so we can rever the values back to the original state
            foreach (var map in updatedMap)
            {
                context.Entry(map).State = EntityState.Detached;
            }

            Assert.AreEqual("34561", actualFlPlu);
            Assert.AreEqual("50013", actualSwPlu);
        }

        [TestCategory("Integration"), TestCategory("BulkImport-Plu")]
        [TestMethod]
        public void PluBulkImportExecute_NullValuesInMapping_PluMappingUpdated()
        {
            // Given.
            importData = new BulkImportCommand<BulkImportPluModel>
            {
                UserName = "TestIntegrationUser",
                BulkImportData = new List<BulkImportPluModel>
                {
                    new BulkImportPluModel
                    {
                        NationalPlu = "6864",
                        flPLU = "34561",
                        maPLU = "23452",
                        mwPLU = "76543",
                        naPLU = "123456",
                        nePLU = "98765",
                        ncPLU = "44444",
                        pnPLU = "",
                        rmPLU = "55555",
                        spPLU = "15648",
                        soPLU = "",
                        swPLU = "50012",
                        ukPLU = "50012"
                    },

                    new BulkImportPluModel
                    {
                        NationalPlu = "6865",
                        flPLU = "23453",
                        maPLU = "23453",
                        mwPLU = "77777",
                        naPLU = "",
                        nePLU = "99999",
                        ncPLU = "23453",
                        pnPLU = "12345",
                        rmPLU = "12346",
                        spPLU = "",
                        soPLU = "45678",
                        swPLU = "50013",
                        ukPLU = "50013"
                    }
                }
            };

            // When.
            importHandler.Execute(this.importData);

            // Then.
            // Query PLUMap table for above National PLUs to get actual mapped values after Bulk Import
            List<PLUMap> updatedMap = context.PLUMap
                .Where(m => m.Item.ScanCode
                    .Any(s => s.ScanCodeType.scanCodeTypeDesc.Contains("PLU")
                        && (s.scanCode.Contains("6864") || s.scanCode.Contains("6865")))).ToList();

            string actualFlPlu = updatedMap.Where(m => m.Item.ScanCode.Any(s => s.scanCode == "6864")).FirstOrDefault().flPLU;
            string actualSwPlu = updatedMap.Where(m => m.Item.ScanCode.Any(s => s.scanCode == "6865")).FirstOrDefault().swPLU;

            // Detach updateMap from Context so we can rever the values back to the original state
            foreach (var map in updatedMap)
            {
                context.Entry(map).State = EntityState.Detached;
            }

            Assert.AreEqual("34561", actualFlPlu);
            Assert.AreEqual("50013", actualSwPlu);
        }


        [TestCategory("BulkImport-Plu")]
        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void PluBulkImportExecute_ErrorWithUpload_ThrowsException()
        {
            // Given.
            importData = new BulkImportCommand<BulkImportPluModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportPluModel>
                {
                    new BulkImportPluModel
                    {
                        NationalPlu = "6867",
                        flPLU = "345616546546132132465468",
                        maPLU = "23452",
                        mwPLU = "76543",
                        naPLU = "76543",
                        nePLU = "",
                        ncPLU = "",
                        pnPLU = "",
                        rmPLU = "564651321325165",
                        spPLU = "",
                        soPLU = "",
                        swPLU = "50012",
                        ukPLU = "50012"
                    }
                }
            };

            // When & Then - SqlException Expected
            importHandler.Execute(this.importData);
        }

        [TestCategory("Integration"), TestCategory("BulkImport-Plu")]
        [TestMethod]
        public void PluBulkImportExecute_Logging_VerifyLoggingIsCalled()
        {
            // Given.
            importData = new BulkImportCommand<BulkImportPluModel>
            {
                UserName = "TestIntegrationUser",
                BulkImportData = new List<BulkImportPluModel>
                {
                    new BulkImportPluModel
                    {
                        NationalPlu = "6864",
                        flPLU = "34561",
                        maPLU = "23452",
                        mwPLU = "76543",
                        naPLU = "123456",
                        nePLU = "98765",
                        ncPLU = "44444",
                        pnPLU = "76543",
                        rmPLU = "55555",
                        spPLU = "15648",
                        soPLU = "77777",
                        swPLU = "50012",
                        ukPLU = "50012"
                    },

                    new BulkImportPluModel
                    {
                        NationalPlu = "6865",
                        flPLU = "23453",
                        maPLU = "23453",
                        mwPLU = "77777",
                        naPLU = "897845",
                        nePLU = "99999",
                        ncPLU = "23453",
                        pnPLU = "12345",
                        rmPLU = "12346",
                        spPLU = "789416",
                        soPLU = "45678",
                        swPLU = "50013",
                        ukPLU = "50013"
                    }
                }
            };

            // When.
            importHandler.Execute(this.importData);

            // Then.
            mockLogger.Verify(log => log.Info(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}
