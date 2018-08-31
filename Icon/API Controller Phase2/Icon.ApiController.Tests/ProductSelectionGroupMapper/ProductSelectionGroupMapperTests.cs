using Icon.ApiController.Controller.Mappers;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.PreGpm.Contracts;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Icon.ApiController.Tests.ProductSelectionGroups
{
    [TestClass]
    public class ProductSelectionGroupMapperTests
    {
        private ProductSelectionGroupsMapper productSelectionGroupMapper;
        private Mock<IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>> mockGetProductSelectionGroupsQueryHandler;
        private List<ProductSelectionGroup> testProductSelectionGroups;

        [TestInitialize]
        public void Initialize()
        {
            mockGetProductSelectionGroupsQueryHandler = new Mock<IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>>>();
            productSelectionGroupMapper = new ProductSelectionGroupsMapper(mockGetProductSelectionGroupsQueryHandler.Object);
        }

        [TestMethod]
        public void LoadProductSelectionGroups_ShouldCallQueryHandler()
        {
            //Given 
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>())
                .Verifiable();

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();

            //Then
            mockGetProductSelectionGroupsQueryHandler.Verify();
        }

        [TestMethod]
        public void GetProductSelectionGroups_ProductWithAllPsgAssociations_ShouldReturnPsgMessages()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Food_Stamp")
                        .WithTraitId(Traits.FoodStampEligible)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("ProhibitDiscount")
                        .WithTraitId(Traits.ProhibitDiscount)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("DonationsGroup")
                        .WithMerchandiseHierarchyClassId(5)
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueProduct mqp = new MessageQueueProduct
            {
                FoodStampEligible = "1",
                ProhibitDiscount = true,
                MerchandiseClassId = 5
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqp);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[2], result.group.First(sg => sg.id == testProductSelectionGroups[2].ProductSelectionGroupName), true);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ProductWithoutPsgAssociations_ShouldReturnNull()
        {
            //Given
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(new MessageQueueProduct());

            //Then
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ProductWithSomePsgAssociations_ShouldReturnPsgMessages()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Food_Stamp")
                        .WithTraitId(Traits.FoodStampEligible)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("ProhibitDiscount")
                        .WithTraitId(Traits.ProhibitDiscount)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueProduct mqp = new MessageQueueProduct
            {
                ProhibitDiscount = true
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqp);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), false);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), true);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ProductWithMerchandiseHierarchyClassPsgAssociations_ShouldReturnPsgMessages()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("035_GC_CASHOUT_All")
                        .WithMerchandiseHierarchyClassId(5)
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueProduct mqp = new MessageQueueProduct
            {
                FoodStampEligible = "1",
                ProhibitDiscount = true,
                MerchandiseClassId = 5
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqp);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.group.Length); 
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), true);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ProductMessageWithMerchandiseHierarchyClassPsgAssociationsAndPsgsExistAssociatedToMerchandiseHierarchies_ShouldReturnPsgMessagesWithDeletePsgsForMerchandisePsgs()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupId(1)
                        .WithProductSelectionGroupName("035_GC_CASHOUT_All")
                        .WithMerchandiseHierarchyClassId(5)
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupId(2)
                        .WithProductSelectionGroupName("040_GC_BLACKHAWK")
                        .WithMerchandiseHierarchyClassId(6)
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupId(3)
                        .WithProductSelectionGroupName("005_GC_ACTIVATION_VANTIV_BARCODE")
                        .WithMerchandiseHierarchyClassId(7)
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueProduct mqp = new MessageQueueProduct
            {
                FoodStampEligible = "1",
                ProhibitDiscount = true,
                MerchandiseClassId = 5
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqp);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), false);
            ValidateProductSelectionGroup(testProductSelectionGroups[2], result.group.First(sg => sg.id == testProductSelectionGroups[2].ProductSelectionGroupName), false);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ItemLocaleWithAllPsgAssociations_ShouldReturnPsgMessages()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Restrict 18")
                        .WithTraitId(Traits.AgeRestrict)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("CaseDiscount")
                        .WithTraitId(Traits.CaseDiscountEligible)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("RestrictHours")
                        .WithTraitId(Traits.RestrictedHours)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("TMDiscount")
                        .WithTraitId(Traits.TmDiscountEligible)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueItemLocale mqil = new MessageQueueItemLocale
            {
                AgeCode = 1,
                Case_Discount = true,
                Restricted_Hours = true,
                TMDiscountEligible = true
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqil);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[2], result.group.First(sg => sg.id == testProductSelectionGroups[2].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[3], result.group.First(sg => sg.id == testProductSelectionGroups[3].ProductSelectionGroupName), true);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ItemLocaleWithoutPsgAssociations_ShouldReturnNull()
        {
            //Given
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(new List<ProductSelectionGroup>());

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(new MessageQueueItemLocale());

            //Then
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ItemLocaleWithSomePsgAssociations_ShouldReturnPsgMessages()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Restrict 18")
                        .WithTraitId(Traits.AgeRestrict)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("CaseDiscount")
                        .WithTraitId(Traits.CaseDiscountEligible)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("RestrictHours")
                        .WithTraitId(Traits.RestrictedHours)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("TMDiscount")
                        .WithTraitId(Traits.TmDiscountEligible)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueItemLocale mqil = new MessageQueueItemLocale
            {
                AgeCode = 2,
                Case_Discount = true
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqil);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), false);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[2], result.group.First(sg => sg.id == testProductSelectionGroups[2].ProductSelectionGroupName), false);
            ValidateProductSelectionGroup(testProductSelectionGroups[3], result.group.First(sg => sg.id == testProductSelectionGroups[3].ProductSelectionGroupName), false);
        }

        [TestMethod]
        public void GetProductSelectionGroups_ItemLocaleAndMultiplePsgsWithTheSameTraitIdExistWithNoMatchForMessagesTraitValue_ShouldReturnPsgsWithDeleteActionsForNonMatchingTraitValues()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Restrict 18")
                        .WithTraitId(Traits.AgeRestrict)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Restrict 21")
                        .WithTraitId(Traits.AgeRestrict)
                        .WithTraitValue("2")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueItemLocale mqil = new MessageQueueItemLocale
            {
                AgeCode = 0
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqil);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), false);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), false);            
        }

        [TestMethod]
        public void GetProductSelectionGroups_ItemLocaleAndMultiplePsgsWithTheSameTraitIdExistAndAPsgMatcheMessagesTraitValue_ShouldReturnPsgsWithDeleteActionsForNonMatchingTraitValues()
        {
            //Given
            testProductSelectionGroups = new List<ProductSelectionGroup>
                {
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Restrict 18")
                        .WithTraitId(Traits.AgeRestrict)
                        .WithTraitValue("1")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder()),
                    new TestProductSelectionGroupBuilder()
                        .WithProductSelectionGroupName("Restrict 21")
                        .WithTraitId(Traits.AgeRestrict)
                        .WithTraitValue("2")
                        .WithProductSelectionGroupType(new TestProductSelectionGroupTypeBuilder())
                };
            mockGetProductSelectionGroupsQueryHandler.Setup(m => m.Search(It.IsAny<GetProductSelectionGroupsParameters>()))
                .Returns(testProductSelectionGroups);
            MessageQueueItemLocale mqil = new MessageQueueItemLocale
            {
                AgeCode = 1
            };

            //When
            productSelectionGroupMapper.LoadProductSelectionGroups();
            var result = productSelectionGroupMapper.GetProductSelectionGroups(mqil);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.group.Length);
            ValidateProductSelectionGroup(testProductSelectionGroups[0], result.group.First(sg => sg.id == testProductSelectionGroups[0].ProductSelectionGroupName), true);
            ValidateProductSelectionGroup(testProductSelectionGroups[1], result.group.First(sg => sg.id == testProductSelectionGroups[1].ProductSelectionGroupName), false);
        }

        private void ValidateProductSelectionGroup(ProductSelectionGroup expected, GroupTypeType actual, bool addOrUpdate)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ProductSelectionGroupName, actual.id);
            Assert.AreEqual(expected.ProductSelectionGroupName, actual.name);
            Assert.AreEqual(expected.ProductSelectionGroupType.ProductSelectionGroupTypeName, actual.type);
            Assert.IsNull(actual.description);
            if (addOrUpdate)
            {
                Assert.AreEqual(ActionEnum.AddOrUpdate, actual.Action);
            }
            else
            {
                Assert.AreEqual(ActionEnum.Delete, actual.Action);
            }
            Assert.IsTrue(actual.ActionSpecified);
        }
    }
}
