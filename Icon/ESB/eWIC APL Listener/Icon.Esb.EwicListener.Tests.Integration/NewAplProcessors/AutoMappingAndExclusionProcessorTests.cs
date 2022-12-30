using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Esb.EwicAplListener.NewAplProcessors;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.NewAplProcessorsbr
{
    [TestClass]
    public class AutoMappingAndExclusionProcessorTests
    {
        private AutoMappingAndExclusionProcessor processor;
        private IRenewableContext<IconContext> globalContext;
        private Mock<ILogger<AutoMappingAndExclusionProcessor>> mockLogger;
        private ExclusionGenerator exclusionGenerator;
        private MappingGenerator mappingGenerator;
        private Mock<IEsbConnectionFactory> mockConnectionFactory;
        private Mock<IEsbProducer> mockProducer;
        private IconContext context;
        private DbContextTransaction transaction;
        private AuthorizedProductListModel testModel;
        private List<string> testAgenciesId;
        private List<Agency> testAgencies;
        private List<AuthorizedProductList> testApl;
        private List<Item> testItems;
        private string testAplScanCode;
        private List<string> testWfmScanCodes;
        private List<Mapping> testMappings;
        private string newAgencyId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            newAgencyId = "QQ";

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX"
            };

            testWfmScanCodes = new List<string>
            {
                "22222222",
                "22222223",
                "22222224"
            };

            testAplScanCode = "2222222229";

            mockLogger = new Mock<ILogger<AutoMappingAndExclusionProcessor>>();

            mockConnectionFactory = new Mock<IEsbConnectionFactory>();
            mockProducer = new Mock<IEsbProducer>();

            mockConnectionFactory.Setup(f => f.CreateProducer(It.IsAny<EsbConnectionSettings>())).Returns(mockProducer.Object);

            exclusionGenerator = new ExclusionGenerator(
                new Mock<ILogger<ExclusionGenerator>>().Object,
                new EwicExclusionSerializer(),
                new GetExclusionQuery(globalContext),
                new AddExclusionCommand(globalContext),
                new SaveToMessageHistoryCommand(globalContext),
                new UpdateMessageHistoryMessageCommand(globalContext));

            mappingGenerator = new MappingGenerator(
                new Mock<ILogger<MappingGenerator>>().Object,
                new EwicMappingSerializer(),
                new GetExistingMappingsQuery(globalContext),
                new AddMappingsCommand(globalContext),
                new SaveToMessageHistoryCommand(globalContext),
                new UpdateMessageHistoryMessageCommand(globalContext));

            processor = new AutoMappingAndExclusionProcessor(
                mockLogger.Object,
                exclusionGenerator,
                mappingGenerator);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestAgency()
        {
            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]));
            context.SaveChanges();

            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(newAgencyId));
            context.SaveChanges();
        }

        private void StageTestAgencies()
        {
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();

            context.Agency.Add(new TestAgencyBuilder().WithAgencyId(newAgencyId));
            context.SaveChanges();
        }

        private void StageTestAplForOneAgency()
        {
            context.AuthorizedProductList.Add(new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode));
            context.SaveChanges();

            context.AuthorizedProductList.Add(new TestAplBuilder().WithAgencyId(newAgencyId).WithScanCode(testAplScanCode));
            context.SaveChanges();
        }

        private void StageTestAplForMultipleAgencies()
        {
            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testAplScanCode),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testAplScanCode)
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            context.AuthorizedProductList.Add(new TestAplBuilder().WithAgencyId(newAgencyId).WithScanCode(testAplScanCode));
            context.SaveChanges();
        }

        private void StageTestItems()
        {
            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0]),
                new TestItemBuilder().WithScanCode(testWfmScanCodes[1]),
                new TestItemBuilder().WithScanCode(testWfmScanCodes[2])
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();
        }

        private void StageTestMappingsForOneAgency()
        {
            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[2].ScanCode.Single())
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();
        }

        private void StageTestMappingsForTwoAgencies()
        {
            testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[2].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCode).WithWfmScanCode(testItems[1].ScanCode.Single()),
            };

            context.Mapping.AddRange(testMappings);
            context.SaveChanges();
        }

        private void StageTestExclusionForOneAgency()
        {
            Agency testAgency = testAgencies[0];

            testAgency.ScanCode.Add(testItems[0].ScanCode.Single());
            context.SaveChanges();
        }

        [TestMethod]
        public void ApplyMappings_NoMappingsExist_NoMappingsShouldBeAdded()
        {
            // Given.
            int currentMappingCount = context.Mapping.Count();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyMappings(testModel);

            // Then.
            int newMappingCount = context.Mapping.Count();

            Assert.AreEqual(currentMappingCount, newMappingCount);
        }

        [TestMethod]
        public void ApplyMappings_NoMappingsExist_NoMappingsShouldBeTransmitted()
        {
            // Given.
            int currentMappingCount = context.Mapping.Count();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyMappings(testModel);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void ApplyMappings_MappingsExistForOneAgency_AllMappingsShouldBeAddedForNewAgency()
        {
            // Given.
            StageTestAgency();
            StageTestAplForOneAgency();
            StageTestItems();
            StageTestMappingsForOneAgency();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = newAgencyId,
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };
            
            // When.
            processor.ApplyMappings(testModel);

            // Then.
            var newMappings = context.Mapping.Where(m => m.AgencyId == newAgencyId && m.AplScanCode == testAplScanCode).ToList();

            Assert.AreEqual(testMappings.Count, newMappings.Count);
        }

        [TestMethod]
        public void ApplyMappings_MappingAlreadyExistsForAgency_DuplicateMappingShouldNotBeCreated()
        {
            // Given.
            StageTestAgency();
            StageTestAplForOneAgency();
            StageTestItems();
            StageTestMappingsForOneAgency();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = testAgenciesId[0],
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyMappings(testModel);

            // Then.
            var newMappings = context.Mapping.Where(m => m.AgencyId == newAgencyId && m.AplScanCode == testAplScanCode).ToList();

            Assert.AreEqual(0, newMappings.Count);
        }

        [TestMethod]
        public void ApplyMappings_MappingsExistForOneAgency_AllMappingsShouldBeTransmitted()
        {
            // Given.
            StageTestAgency();
            StageTestAplForOneAgency();
            StageTestItems();
            StageTestMappingsForOneAgency();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = newAgencyId,
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyMappings(testModel);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(testItems.Count));
        }

        [TestMethod]
        public void ApplyMappings_MappingsExistForTwoAgencies_AllMappingsShouldBeAddedForNewAgencyWithNoDuplicates()
        {
            // Given.
            StageTestAgencies();
            StageTestAplForMultipleAgencies();
            StageTestItems();
            StageTestMappingsForTwoAgencies();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = newAgencyId,
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyMappings(testModel);

            // Then.
            var newMappings = context.Mapping.Where(m => m.AgencyId == newAgencyId && m.AplScanCode == testAplScanCode).ToList();

            Assert.AreEqual(testMappings.Select(m => m.ScanCodeId).Distinct().ToList().Count, newMappings.Count);
        }

        [TestMethod]
        public void ApplyMappings_MappingsExistForTwoAgencies_AllMappingsShouldBeTransmitted()
        {
            // Given.
            StageTestAgencies();
            StageTestAplForMultipleAgencies();
            StageTestItems();
            StageTestMappingsForTwoAgencies();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = newAgencyId,
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyMappings(testModel);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(testItems.Count));
        }

        [TestMethod]
        public void ApplyExclusions_NoExclusionsExist_NoExclusionsShouldBeAdded()
        {
            // Given.
            int currentExclusionCount = context.Agency
                .Where(a => a.ScanCode.Count > 0)
                .SelectMany(a => a.ScanCode.Select(sc => sc.scanCode))
                .ToList()
                .Count;

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyExclusions(testModel);

            // Then.
            int newExclusionCount = context.Agency
                .Where(a => a.ScanCode.Count > 0)
                .SelectMany(a => a.ScanCode.Select(sc => sc.scanCode))
                .ToList()
                .Count;

            Assert.AreEqual(currentExclusionCount, newExclusionCount);
        }

        [TestMethod]
        public void ApplyExclusions_NoExclusionsExist_NoExclusionsShouldBeTransmitted()
        {
            // Given.
            int currentExclusionCount = context.Agency
                .Where(a => a.ScanCode.Count > 0)
                .SelectMany(a => a.ScanCode.Select(sc => sc.scanCode))
                .ToList()
                .Count;

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = testAplScanCode,
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyExclusions(testModel);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void ApplyExclusions_ExclusionExistsForOneAgency_ExclusionShouldBeCreatedforTheNewAgency()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestExclusionForOneAgency();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = newAgencyId,
                    ScanCode = testWfmScanCodes[0],
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyExclusions(testModel);

            // Then.
            var newExclusion = context.Agency.Single(a => a.AgencyId == newAgencyId).ScanCode.Single();

            Assert.AreEqual(this.testItems[0].ScanCode.Single().scanCodeID, newExclusion.scanCodeID);
        }

        [TestMethod]
        public void ApplyExclusions_ExclusionAlreadyExists_DuplicateExlusionShouldNotBeCreated()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestExclusionForOneAgency();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = testAgenciesId[0],
                    ScanCode = testWfmScanCodes[0],
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyExclusions(testModel);

            // Then.
            string testAgencyId = testAgenciesId[0];
            var newExclusion = context.Agency.Single(a => a.AgencyId == testAgencyId).ScanCode.Single();

            Assert.AreEqual(this.testItems[0].ScanCode.Single().scanCodeID, newExclusion.scanCodeID);
        }

        [TestMethod]
        public void ApplyExclusions_ExclusionExistsForOneAgency_ExclusionShouldBeTransmitted()
        {
            // Given.
            StageTestAgencies();
            StageTestItems();
            StageTestExclusionForOneAgency();

            var ewicItemModels = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = newAgencyId,
                    ScanCode = testWfmScanCodes[0],
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            testModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = ewicItemModels
            };

            // When.
            processor.ApplyExclusions(testModel);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }
    }
}
