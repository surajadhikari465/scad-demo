using Icon.Framework;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Web.Tests.Unit.Models
{
    [TestClass]
    public class LocaleAccessorTests
    {
        private Locale testLocale;

        [TestMethod]
        public void LocaleAccessor_GetCurrencyCode_WorksForUSD()
        {
            //Given
            const string expectedCurrencyCode = "USD";
            testLocale = TestHelpers.GetFakeLocaleWithAddress();
            testLocale.LocaleTrait = TestHelpers.MakeLocaleTraitsForLocale();
            testLocale.LocaleTrait.Add(
                new LocaleTrait { traitID = Traits.CurrencyCode, traitValue = expectedCurrencyCode });

            //When
            var actualCurrencyCode = LocaleAccessor.GetCurrencyCode(testLocale);

            //Then
            Assert.AreEqual(expectedCurrencyCode, actualCurrencyCode);
        }
        [TestMethod]
        public void LocaleAccessor_GetCurrencyCode_WorksForGBP()
        {
            //Given
            const string expectedCurrencyCode = "GBP";
            testLocale = TestHelpers.GetFakeLocaleWithAddress();
            testLocale.LocaleTrait = TestHelpers.MakeLocaleTraitsForLocale();
            testLocale.LocaleTrait.Add(
                new LocaleTrait { traitID = Traits.CurrencyCode, traitValue = expectedCurrencyCode });

            //When
            var actualCurrencyCode = LocaleAccessor.GetCurrencyCode(testLocale);

            //Then
            Assert.AreEqual(expectedCurrencyCode, actualCurrencyCode);
        }

        [TestMethod]
        public void LocaleAccessor_GetCurrencyCode_WhenNoTrait_ReturnsEmptyString()
        {
            //Given
            string expectedCurrencyCode = String.Empty;
            testLocale = TestHelpers.GetFakeLocaleWithAddress();
            testLocale.LocaleTrait = TestHelpers.MakeLocaleTraitsForLocale();

            //When
            var actualCurrencyCode = LocaleAccessor.GetCurrencyCode(testLocale);

            //Then
            Assert.AreEqual(expectedCurrencyCode, actualCurrencyCode);
        }

    }
}
