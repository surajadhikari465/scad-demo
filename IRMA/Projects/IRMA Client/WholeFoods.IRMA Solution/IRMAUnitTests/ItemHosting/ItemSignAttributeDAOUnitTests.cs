using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeFoods.IRMA.ItemHosting.BusinessLogic;
using WholeFoods.IRMA.ItemHosting.DataAccess;

namespace IRMAUnitTests.ItemHosting
{
    [TestClass]
    public class ItemSignAttributeDAOUnitTests
    {
        [TestMethod]
        public void ItemSignAttributeDAO_ValidateData_WhenValid_ReturnsValidStatusCode()
        {
            // Given.
            var testSignAttributes = isaAllPropertyValues;
            var expected = ItemSignAttributeValidationStatus.Valid;

            // When.
            ArrayList result = ItemSignAttributeDAO.ValidateSignAttributes(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ItemSignAttributeValidationStatus)result[0]);
        }

        [TestMethod]
        public void ItemSignAttributeDAO_ValidateData_WhenInvalidCharInLocality_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = isaTypical;
            isaTypical.Locality = "No p|pes allowed";
            var expected = ItemSignAttributeValidationStatus.Error_LocalityInvalidCharacters;

            // When.
            ArrayList result = ItemSignAttributeDAO.ValidateSignAttributes(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ItemSignAttributeValidationStatus)result[0]);
        }

        [TestMethod]
        public void ItemSignAttributeDAO_ValidateData_WhenInvalidCharInSignRomanceLong_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = isaTypical;
            isaTypical.SignRomanceLong = "No p|pes allowed";
            var expected = ItemSignAttributeValidationStatus.Error_SignRomanceTextLongInvalidCharacters;

            // When.
            ArrayList result = ItemSignAttributeDAO.ValidateSignAttributes(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ItemSignAttributeValidationStatus)result[0]);
        }

        [TestMethod]
        public void ItemSignAttributeDAO_ValidateData_WhenInvalidCharInSignRomanceShort_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = isaTypical;
            isaTypical.SignRomanceShort = "No p|pes allowed";
            var expected = ItemSignAttributeValidationStatus.Error_SignRomanceTextShortInvalidCharacters;

            // When.
            ArrayList result = ItemSignAttributeDAO.ValidateSignAttributes(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ItemSignAttributeValidationStatus)result[0]);
        }

        [TestMethod]
        public void ItemSignAttributeDAO_ValidateData_WhenInvalidCharInChicagoBabyUOM_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = isaTypical;
            isaTypical.ChicagoBaby = "No p|pes allowed";
            var expected = ItemSignAttributeValidationStatus.Error_ChicagoBabyInvalidCharacters;

            // When.
            ArrayList result = ItemSignAttributeDAO.ValidateSignAttributes(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ItemSignAttributeValidationStatus)result[0]);
        }

        [TestMethod]
        public void ItemSignAttributeDAO_ValidateData_WhenInvalidCharInMultipleFields_ReturnsAllErrorCodes()
        {
            // Given.
            var testSignAttributes = isaTypical;
            isaTypical.Locality = "No p|pes allowed";
            isaTypical.SignRomanceLong = "No p|pes allowed";
            isaTypical.SignRomanceShort = "No p|pes allowed";
            isaTypical.ChicagoBaby = "No p|pes allowed";

            // When.
            ArrayList result = ItemSignAttributeDAO.ValidateSignAttributes(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        private ItemSignAttributeBO isaTypical = new ItemSignAttributeBO
        {
            ItemSignAttributeID = 10001,
            ItemKey = 55555,
            AirChilled = false,
            Biodynamic = false,
            CheeseRaw = false,
            ColorAdded = null,
            DryAged = false,
            GlutenFree = null,
            GrassFed = false,
            FreeRange = false,
            Kosher = null,
            MadeInHouse = false,
            Msc = false,
            NonGmo = true,
            PastureRaised = false,
            PremiumBodyCare = false,
            Vegan = null,
            Vegetarian = false,
            WholeTrade = false,
            AnimalWelfareRating = null,
            CheeseMilkType = null,
            ChicagoBaby = null,
            Locality = "Austin, TX",
            EcoScaleRating = null,
            FreshOrFrozen = null,
            HealthyEatingRating = null,
            SeafoodCatchType = null,
            SignRomanceLong = null,
            SignRomanceShort = null,
            Exclusive = null,
            TagUom = null
        };

        private ItemSignAttributeBO isaAllPropertyValues = new ItemSignAttributeBO
        {
            ItemSignAttributeID = 10001,
            ItemKey = 55555,
            AirChilled = true,
            Biodynamic = true,
            CheeseRaw = true,
            ColorAdded = true,
            DryAged = true,
            GlutenFree = true,
            GrassFed = true,
            FreeRange = true,
            Kosher = true,
            MadeInHouse = true,
            Msc = true,
            NonGmo = true,
            PastureRaised = true,
            PremiumBodyCare = true,
            Vegan = true,
            Vegetarian = true,
            WholeTrade = true,
            AnimalWelfareRating = "Step 1",
            CheeseMilkType = "Sheep Milk",
            ChicagoBaby = null,
            Locality = "Austin, TX",
            EcoScaleRating = "Baseline/Orange",
            FreshOrFrozen = "Fresh",
            HealthyEatingRating = "Better",
            SeafoodCatchType = "Wild",
            SignRomanceLong = "A unique blend of cow, sheep and goat's milk, crafted into a Greek cheese experience like no other. Grill, fry or pan sear for a crispy crust!",
            SignRomanceShort = "Asturian dialect for “The Song” this cheese is made from raw cow’s milk with a cheddary, crystalline texture.",
            Exclusive = new DateTime(2022,12,31),
            TagUom = 1
        };
    }
}