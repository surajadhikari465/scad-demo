using Icon.Common.DataAccess;
using Infor.Services.NewItem.Queries;
using Infor.Services.NewItem.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infor.Services.NewItem.Models;

namespace Infor.Services.NewItem.Tests.Validators
{
    [TestClass]
    public class PluCollectionValidatorTests
    {
        private PluCollectionValidator validator;
        private Mock<IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>> mockGetItemIdsQueryHandler;
        private Dictionary<string, int> itemIdDictionary;
        private List<NewItemModel> testItems;

        [TestInitialize]
        public void Initialize()
        {
            itemIdDictionary = new Dictionary<string, int>();
            mockGetItemIdsQueryHandler = new Mock<IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>>();
            mockGetItemIdsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemIdsQuery>())).Returns(itemIdDictionary);

            validator = new PluCollectionValidator(mockGetItemIdsQueryHandler.Object);

            testItems = new List<NewItemModel>();
        }

        [TestMethod]
        public void Validate_NoPlus_ShouldHaveOnlyValidItems()
        {
            //Given
            testItems.Add(new NewItemModel { ScanCode = "1234567", IsRetailSale = true });

            //When
            var result = validator.Validate(testItems);

            //Then
            Assert.AreEqual(0, result.InvalidEntities.Count());
            Assert.AreEqual(1, result.ValidEntities.Count());
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void Validate_AllPlus_ShouldHaveOnlyInvalidItems()
        {
            //Given
            testItems.Add(new NewItemModel { ScanCode = "123456", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "1", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "20000000000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "22879000000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "27894000000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "21234500000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "46123456787" });
            testItems.Add(new NewItemModel { ScanCode = "46000000000" });
            testItems.Add(new NewItemModel { ScanCode = "48452879654" });
            testItems.Add(new NewItemModel { ScanCode = "48000000000" });

            //When
            var result = validator.Validate(testItems);

            //Then
            Assert.AreEqual(testItems.Count(), result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            Assert.IsNotNull(result.Error);
        }

        [TestMethod]
        public void Validate_NonPlusAndPlus_ShouldHaveInvalidAndValidItems()
        {
            //Given
            testItems.Add(new NewItemModel { ScanCode = "12345678", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "123456789" });
            testItems.Add(new NewItemModel { ScanCode = "123456", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "1", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "20000000000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "22879000000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "27894000000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "21234500000", IsRetailSale = true });
            testItems.Add(new NewItemModel { ScanCode = "46123456787" });
            testItems.Add(new NewItemModel { ScanCode = "46000000000" });
            testItems.Add(new NewItemModel { ScanCode = "48452879654" });
            testItems.Add(new NewItemModel { ScanCode = "48000000000" });

            //When
            var result = validator.Validate(testItems);

            //Then
            Assert.AreEqual(10, result.InvalidEntities.Count());
            Assert.AreEqual(2, result.ValidEntities.Count());
            Assert.IsNotNull(result.Error);
        }
    }
}
