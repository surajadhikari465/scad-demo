using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass]
    public class ValueFormatterTests
    {
        [TestMethod()]
        public void FormatValueForMessage_WhenAttributeIsNotIsShouldTransform_ShouldReturnSuppliedValue()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = false,
                IsSpecialTransform = false
            }, "Anything");

            // Then.
            Assert.AreEqual("Anything", response);
        }

        [TestMethod()]
        public void FormatValueForMessage_WhenAttributeIsNotPicklistOrBoolean_ShouldReturnSuppliedValue()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = false,
                IsSpecialTransform = true
            }, "Anything");

            // Then.
            Assert.AreEqual("Anything", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatBoolean_ValuesIsTrue_ResponseIs1()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = false,
                IsSpecialTransform = true,
                DataTypeName = ItemPublisherConstants.DataTypes.Boolean
            }, "TRUE");

            // Then.
            Assert.AreEqual("1", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatVegetarian_ValuesIsTrue_ResponseIs1()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "Vegetarian",
                IsPickList = true,
                IsSpecialTransform = true,
                DataTypeName = ItemPublisherConstants.DataTypes.Text
            }, "TRUE");

            // Then.
            Assert.AreEqual("1", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatVegetarian_ValuesIsNo_ResponseIs0()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "Vegetarian",
                IsPickList = true,
                IsSpecialTransform = true,
                DataTypeName = ItemPublisherConstants.DataTypes.Text
            }, "No");

            // Then.
            Assert.AreEqual("0", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatVegetarian_ValuesIsYes_ResponseIs1()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "Vegetarian",
                IsPickList = true,
                IsSpecialTransform = true,
                DataTypeName = ItemPublisherConstants.DataTypes.Text
            }, "Yes");

            // Then.
            Assert.AreEqual("1", response);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void FormatValueForMessageFormatBoolean_ValuesIsNotABoolean_ExceptionThrown()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = false,
                IsSpecialTransform = true,
                DataTypeName = ItemPublisherConstants.DataTypes.Boolean
            }, "NotaBoolean");
        }

        [TestMethod()]
        public void FormatValueForMessageFormatBoolean_ValuesIsFalse_ResponseIs0()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = false,
                IsSpecialTransform = true,
                DataTypeName = ItemPublisherConstants.DataTypes.Boolean
            }, "False");

            // Then.
            Assert.AreEqual("0", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatKosher_AttributeValuesIsYes_ReturnIsYes()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "Kosher",
                IsPickList = true,
                IsSpecialTransform = true
            }, "Yes");

            // Then.
            Assert.AreEqual("Yes", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatKosher_AttributeValuesIsNotYes_ReturnIsSuppliedValue()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "Kosher",
                IsPickList = true,
                IsSpecialTransform = true
            }, "No");

            // Then.
            Assert.AreEqual("No", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatKosher_AttributeValuesIsEmptyString_ReturnIsEmptyString()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "Kosher",
                IsPickList = true,
                IsSpecialTransform = true
            }, "");

            // Then.
            Assert.AreEqual(string.Empty, response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatPickList_ValuesIsYes_ResponseIs1()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = true,
                IsSpecialTransform = true
            }, "Yes");

            // Then.
            Assert.AreEqual("1", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatPickList_ValuesIsNo_ResponseIs0()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = true,
                IsSpecialTransform = true
            }, "No");

            // Then.
            Assert.AreEqual("0", response);
        }

        [TestMethod()]
        public void FormatValueForMessageFormatPickList_ValuesIsNotYesOrNo_ResponseIsSuppliedValue()
        {
            // Given.
            IValueFormatter formatter = new ValueFormatter();

            // When.
            string response = formatter.FormatValueForMessage(new ItemPublisher.Repositories.Entities.Attributes()
            {
                AttributeName = "A",
                IsPickList = true,
                IsSpecialTransform = true
            }, "Value");

            // Then.
            Assert.AreEqual("Value", response);
        }
    }
}