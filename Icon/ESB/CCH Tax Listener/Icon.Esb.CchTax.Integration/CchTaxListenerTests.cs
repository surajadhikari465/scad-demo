using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dvs;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Esb.Subscriber;
using Icon.Esb.CchTax.Models;
using System.Collections.Generic;
using Icon.Esb.CchTax.MessageParsers;
using Icon.Common.Email;
using Icon.Logging;
using Icon.Common.DataAccess;
using Icon.Esb.CchTax.Commands;
using Moq;
using System.IO;
using Icon.Framework;
using Icon.Testing.Builders;
using System.Linq;
using Icon.Esb.CchTax.Infrastructure;
using System.Configuration;

namespace Icon.Esb.CchTax.Tests.Integration
{
    [TestClass]
    public class CchTaxListenerTests
    {
        private CchTaxListener listener;
        private DataConnectionManager dataConnectionManager;
        private string connectionString;
        private CchTaxListenerApplicationSettings applicationSettings;
        private DvsListenerSettings listenerSettings;
        private Mock<IDvsSubscriber> mockSubscriber;
        private CchTaxMessageParser messageParser;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<CchTaxListener>> mockLogger;
        private SaveTaxHierarchyClassesCommandHandler saveTaxHierarchyClassCommandHandler;
        private SaveTaxToMammothCommandHandler saveTaxToMammothCommandHandler;
        private List<RegionModel> regions;
        private DvsSqsMessage sqsMessage;
        private IconContext context;
        private List<string> taxHierarchyClassNamesBeforeUpdate = new List<string>
            {
                "1111111 TEST TAX1 BEFORE UPDATE",
                "1111112 TEST TAX2 BEFORE UPDATE",
                "1111113 TEST TAX3 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST BEFORE UPDATE",
                "1111114 TEST TAX4",
                "1111115 TEST TAX5",
                "1111116 TEST TAX6 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST"
            };
        private List<string> taxHierarchyClassNamesAfterUpdate = new List<string>
            {
                "1111111 TEST TAX1",
                "1111112 TEST TAX2",
                "1111113 TEST TAX3 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST",
                "1111114 TEST TAX4",
                "1111115 TEST TAX5",
                "1111116 TEST TAX6 WITH MORE THAN 50 CHARACTERS FOR TAX ABBREVIATION TEST"
            };

        [TestInitialize]
        public void Initialize()
        {
            dataConnectionManager = new DataConnectionManager();
            connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            dataConnectionManager.InitializeConnection(connectionString);
            applicationSettings = new CchTaxListenerApplicationSettings { UpdateMammoth = true, GenerateGlobalEvents = true };
            listenerSettings = DvsListenerSettings.CreateSettingsFromConfig();
            mockSubscriber = new Mock<IDvsSubscriber>();
            messageParser = new CchTaxMessageParser();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<CchTaxListener>>();
            saveTaxHierarchyClassCommandHandler = new SaveTaxHierarchyClassesCommandHandler(applicationSettings);
            saveTaxToMammothCommandHandler = new SaveTaxToMammothCommandHandler(dataConnectionManager, applicationSettings);
            regions = new List<RegionModel>();
            sqsMessage = new DvsSqsMessage()
            {
                MessageAttributes = new Dictionary<string, string>() {
                    { "IconMessageID", "1" },
                    { "toBeReceivedBy", "ALL" }
                },
                S3BucketName = "SampleBucket",
                S3Key = "SampleS3Key"
            };

            listener = new CchTaxListener(
                listenerSettings,
                mockSubscriber.Object,
                messageParser,
                mockEmailClient.Object,
                mockLogger.Object,
                saveTaxHierarchyClassCommandHandler,
                saveTaxToMammothCommandHandler,
                regions);
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context = new IconContext();

            var taxHierarchyClasses = context.HierarchyClass
                .Where(hc => taxHierarchyClassNamesBeforeUpdate.Contains(hc.hierarchyClassName)
                                || taxHierarchyClassNamesAfterUpdate.Contains(hc.hierarchyClassName));
            var taxHierarchyClassTraits = taxHierarchyClasses.SelectMany(hc => hc.HierarchyClassTrait);

            var taxIds = taxHierarchyClassNamesAfterUpdate
                .Select(s => s.Split(' ')[0])
                .ToList();
            var taxEvents = context.EventQueue
                .Where(e => taxIds.Contains(e.EventMessage))
                .ToList();
            var taxHierarchyClassIds = taxHierarchyClasses
                .Select(t => t.hierarchyClassID)
                .ToList();

            List<int> hierarchyClassIds = taxHierarchyClasses.Select(thc => thc.hierarchyClassID).ToList();
            var ids = String.Join(",", hierarchyClassIds);

            context.EventQueue.RemoveRange(taxEvents);
            context.HierarchyClassTrait.RemoveRange(taxHierarchyClassTraits);

            context.HierarchyClass.RemoveRange(taxHierarchyClasses);
            context.SaveChanges();

            dataConnectionManager.Connection
                .Execute(@"DELETE FROM HierarchyClass WHERE HierarchyID = 3 AND HierarchyClassName IN @TestTaxClasses",
                    new { TestTaxClasses = this.taxHierarchyClassNamesAfterUpdate });

            dataConnectionManager.Connection
                .Execute(@"DELETE FROM Tax_Attributes WHERE TaxCode IN @TaxCodes",
                    new { TaxCodes = this.taxHierarchyClassNamesAfterUpdate.Select(taxClassName => taxClassName.Substring(0,7)) });
        }

        [TestMethod]
        public void HandleMessage_3RegionsForEventsAnd3AddAnd3UpdateMessagesAndGenerateEventSettingIsTrue_ShouldAdd3AndUpdat3TaxHierarchyClassesAndGenerateEvents()
        {
            //Given
            List<HierarchyClass> taxHierarchyClasses = new List<HierarchyClass>
                {
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[0])
                        .WithHierarchyId(Hierarchies.Tax),
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[1])
                        .WithHierarchyId(Hierarchies.Tax),
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[2])
                        .WithHierarchyId(Hierarchies.Tax)
                };
            context.HierarchyClass.AddRange(taxHierarchyClasses);
            context.SaveChanges();

            regions.AddRange(new List<RegionModel>
                {
                    new RegionModel { RegionAbbr = "FL"},
                    new RegionModel { RegionAbbr = "SP"},
                    new RegionModel { RegionAbbr = "NE"}
                });

            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));

            //When
            listener.HandleMessage(message);

            //Then
            if (context != null)
            {
                context.Dispose();
                context = new IconContext();
            }

            //Assert that no Tax Hierarchy Classes exist with previous names
            var taxHierarchyClassesThatShouldntExist = context.HierarchyClass
                .Where(hc => taxHierarchyClassNamesBeforeUpdate.Take(3).Contains(hc.hierarchyClassName))
                .ToList();
            Assert.IsFalse(taxHierarchyClassesThatShouldntExist.Any());

            //Assert that Updated/Added Tax Hierarchy Classes exist
            var taxHierarchyClassesAfterUpdate = context.HierarchyClass
                .Where(hc => taxHierarchyClassNamesAfterUpdate.Contains(hc.hierarchyClassName))
                .OrderBy(hc => hc.hierarchyClassName)
                .ToList();
            Assert.AreEqual(6, taxHierarchyClassesAfterUpdate.Count);
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(taxHierarchyClassNamesAfterUpdate[i], taxHierarchyClassesAfterUpdate[i].hierarchyClassName);
            }

            //Assert that tax abbreviations are set for added tax hierarchy classes
            var taxAbbreviations = taxHierarchyClassesAfterUpdate
                .SelectMany(hc => hc.HierarchyClassTrait)
                .Where(hct => hct.traitID == Traits.TaxAbbreviation)
                .OrderBy(hct => hct.traitValue)
                .ToList();
            Assert.AreEqual(2, taxAbbreviations.Count);
            Assert.AreEqual(taxAbbreviations[0].traitValue, taxHierarchyClassesAfterUpdate[3].hierarchyClassName);
            Assert.AreEqual(taxAbbreviations[1].traitValue, taxHierarchyClassesAfterUpdate[4].hierarchyClassName);

            //Assert that events are generated
            var taxIds = taxHierarchyClassNamesAfterUpdate
                .Select(s => s.Split(' ')[0]);
            var taxEvents = context.EventQueue
                .Where(e => taxIds.Contains(e.EventMessage))
                .OrderBy(e => e.EventMessage)
                .ToList();
            Assert.AreEqual(6, taxEvents.Count);
            foreach (var taxEventGroup in taxEvents.GroupBy(e => e.EventMessage))
            {
                Assert.AreEqual(3, taxEventGroup.Count());
                foreach (var region in regions)
                {
                    Assert.IsTrue(taxEventGroup.Any(e => e.RegionCode == region.RegionAbbr));
                }
            }

            // Assert that tax classes are added/update properly to Mammoth database
            int taxId = dataConnectionManager.Connection.Query<int>("SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax'").First();
            var actualTaxesInMammoth = dataConnectionManager.Connection
                .Query(@"SELECT *
                    FROM dbo.HierarchyClass hc
                    JOIN dbo.Tax_Attributes ta on hc.HierarchyClassID = ta.TaxHCID
                    WHERE ta.TaxCode in @TaxCodes AND hc.HierarchyID = @TaxId",
                    new { TaxId = taxId, TaxCodes = this.taxHierarchyClassNamesAfterUpdate.Select(t => t.Substring(0, 7)) })
                .ToList();
            Assert.AreEqual(6, actualTaxesInMammoth.Count);
            for (int i = 0; i < actualTaxesInMammoth.Count; i++)
            {
                Assert.AreEqual(taxHierarchyClassesAfterUpdate[i].hierarchyClassID, actualTaxesInMammoth[i].HierarchyClassID);
                Assert.AreEqual(this.taxHierarchyClassNamesAfterUpdate[i], actualTaxesInMammoth[i].HierarchyClassName);
                Assert.AreEqual(this.taxHierarchyClassNamesAfterUpdate[i].Substring(0, 7), actualTaxesInMammoth[i].TaxCode);
            }            
        }

        [TestMethod]
        public void HandleMessage_0RegionsForEventsAnd3AddAnd3UpdateMessagesAndGlobalEventGenerationSettingIsTrue_ShouldAdd3AndUpdate3TaxHierarchyClassesAndGenerate2EventsWithEmptyRegions()
        {
            //Given
            this.applicationSettings.GenerateGlobalEvents = true;
            List<HierarchyClass> taxHierarchyClasses = new List<HierarchyClass>
                {
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[0])
                        .WithHierarchyId(Hierarchies.Tax),
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[1])
                        .WithHierarchyId(Hierarchies.Tax),
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[2])
                        .WithHierarchyId(Hierarchies.Tax)
                };
            context.HierarchyClass.AddRange(taxHierarchyClasses);
            context.SaveChanges();

            regions.Clear();
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));

            //When
            listener.HandleMessage(message);

            //Then
            if (context != null)
            {
                context.Dispose();
                context = new IconContext();
            }

            //Assert that no Tax Hierarchy Classes exist with previous names
            var taxHierarchyClassesThatShouldntExist = context.HierarchyClass
                .Where(hc => taxHierarchyClassNamesBeforeUpdate.Take(3).Contains(hc.hierarchyClassName))
                .ToList();
            Assert.IsFalse(taxHierarchyClassesThatShouldntExist.Any());

            //Assert that Updated/Added Tax Hierarchy Classes exist
            var taxHierarchyClassesAfterUpdate = context.HierarchyClass
                .Where(hc => taxHierarchyClassNamesAfterUpdate.Contains(hc.hierarchyClassName))
                .OrderBy(hc => hc.hierarchyClassName)
                .ToList();
            Assert.AreEqual(6, taxHierarchyClassesAfterUpdate.Count);
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(taxHierarchyClassNamesAfterUpdate[i], taxHierarchyClassesAfterUpdate[i].hierarchyClassName);
            }

            //Assert that tax abbreviations are set for added tax hierarchy classes
            var taxAbbreviations = taxHierarchyClassesAfterUpdate
                .SelectMany(hc => hc.HierarchyClassTrait)
                .Where(hct => hct.traitID == Traits.TaxAbbreviation)
                .OrderBy(hct => hct.traitValue)
                .ToList();
            Assert.AreEqual(2, taxAbbreviations.Count);
            Assert.AreEqual(taxAbbreviations[0].traitValue, taxHierarchyClassesAfterUpdate[3].hierarchyClassName);
            Assert.AreEqual(taxAbbreviations[1].traitValue, taxHierarchyClassesAfterUpdate[4].hierarchyClassName);

            //Assert that events are generated
            var taxIds = taxHierarchyClassNamesAfterUpdate
                .Select(s => s.Split(' ')[0])
                .ToList();
            var taxEvents = context.EventQueue
                .Where(e => taxIds.Contains(e.EventMessage))
                .OrderBy(e => e.EventMessage)
                .ToList();
            Assert.AreEqual(2, taxEvents.Count);
            foreach (var taxEvent in taxEvents)
            {
                Assert.IsNull(taxEvent.RegionCode);
            }
            Assert.AreEqual("1111114", taxEvents[0].EventMessage);
            Assert.AreEqual("1111115", taxEvents[1].EventMessage);

            // Assert that tax classes are added/update properly to Mammoth database
            int taxId = dataConnectionManager.Connection.Query<int>("SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Tax'").First();
            var actualTaxesInMammoth = dataConnectionManager.Connection
                .Query(@"SELECT *
                    FROM dbo.HierarchyClass hc
                    JOIN dbo.Tax_Attributes ta on hc.HierarchyClassID = ta.TaxHCID
                    WHERE ta.TaxCode in @TaxCodes AND hc.HierarchyID = @TaxId",
                    new { TaxId = taxId, TaxCodes = this.taxHierarchyClassNamesAfterUpdate.Select(t => t.Substring(0, 7)) })
                .ToList();
            Assert.AreEqual(6, actualTaxesInMammoth.Count);
            for (int i = 0; i < actualTaxesInMammoth.Count; i++)
            {
                Assert.AreEqual(taxHierarchyClassesAfterUpdate[i].hierarchyClassID, actualTaxesInMammoth[i].HierarchyClassID);
                Assert.AreEqual(this.taxHierarchyClassNamesAfterUpdate[i], actualTaxesInMammoth[i].HierarchyClassName);
                Assert.AreEqual(this.taxHierarchyClassNamesAfterUpdate[i].Substring(0, 7), actualTaxesInMammoth[i].TaxCode);
            }

        }

        [TestMethod]
        public void HandleMessage_AddUpdateMessageWithGlobalEventsSetToFalse_ShouldNotGenerateGlobalEvents()
        {
            //Given
            this.applicationSettings.GenerateGlobalEvents = false;
            List<HierarchyClass> taxHierarchyClasses = new List<HierarchyClass>
                {
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[0])
                        .WithHierarchyId(Hierarchies.Tax),
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[1])
                        .WithHierarchyId(Hierarchies.Tax),
                    new TestHierarchyClassBuilder()
                        .WithHierarchyClassName(taxHierarchyClassNamesBeforeUpdate[2])
                        .WithHierarchyId(Hierarchies.Tax)
                };
            context.HierarchyClass.AddRange(taxHierarchyClasses);
            context.SaveChanges();

            regions.AddRange(new List<RegionModel>
                {
                    new RegionModel { RegionAbbr = "FL"},
                    new RegionModel { RegionAbbr = "SP"},
                    new RegionModel { RegionAbbr = "NE"}
                });

            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));

            //When
            listener.HandleMessage(message);

            //Then
            if (context != null)
            {
                context.Dispose();
                context = new IconContext();
            }

            //Assert that Updated/Added Tax Hierarchy Classes exist
            var taxHierarchyClassesAfterUpdate = context.HierarchyClass
                .Where(hc => taxHierarchyClassNamesAfterUpdate.Contains(hc.hierarchyClassName))
                .OrderBy(hc => hc.hierarchyClassName)
                .ToList();

            //Assert that events are generated
            var taxIds = taxHierarchyClassNamesAfterUpdate
                .Select(s => s.Split(' ')[0]);
            var taxEvents = context.EventQueue
                .Where(e => taxIds.Contains(e.EventMessage))
                .OrderBy(e => e.EventMessage)
                .ToList();
            Assert.AreEqual(0, taxEvents.Count);
        }

        [TestMethod]
        public void HandleMessage_FullTaxLoad_ShouldNotFail()
        {
            //Given
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages/full_tax_load_from_QA_message.xml"));

            //When
            listener.HandleMessage(message);

            //Then
            Assert.IsTrue(true);
        }
    }
}
