using System;
using Icon.Web.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.Utility
{
    [TestClass]
    public class ConversionUtilityTests
    {
        [TestMethod]
        public void ToFormattedDateTimeString_ValidDateTime_FormatsStringWithSixPlacesAfterSecondsWithZ()
        {
            // Given
            DateTime dateTime = DateTime.UtcNow;
            string expectedDateTimeString = dateTime.ToString("yyyy-MM-ddThh:mm:ss.ffffffZ");

            // When
            string formattedDateTime = dateTime.ToFormattedDateTimeString();

            // Then
            Assert.AreEqual(expectedDateTimeString, formattedDateTime);
        }
    }
}
