using Icon.Framework;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Models
{
    [TestClass]
    public class LocaleGridRowViewModelTests
    {
        private Locale testLocale;

        [TestMethod]
        public void LocaleGridRowViewModel_ConstructorWithLocale_BasicPropertiesAreSet()
        {
            //Given
            testLocale = TestHelpers.GetFakeLocaleWithAddress();
            testLocale.LocaleTrait = TestHelpers.MakeLocaleTraitsForLocale();

            //When
            var localeGridRowViewModel = new LocaleGridRowViewModel(testLocale);

            //Then
            Assert.AreEqual(testLocale.localeName, localeGridRowViewModel.LocaleName);
            Assert.AreEqual(testLocale.parentLocaleID, localeGridRowViewModel.ParentLocaleId);
            Assert.AreEqual(testLocale.ownerOrgPartyID, localeGridRowViewModel.OwnerOrgPartyId);
            Assert.AreEqual(testLocale.localeTypeID, localeGridRowViewModel.LocaleTypeId);
            Assert.AreEqual(testLocale.LocaleType.localeTypeDesc, localeGridRowViewModel.LocaleTypeDesc);
            Assert.AreEqual(testLocale.localeOpenDate, localeGridRowViewModel.OpenDate);
            Assert.AreEqual(testLocale.localeCloseDate, localeGridRowViewModel.CloseDate);
        }

        [TestMethod]
        public void LocaleGridRowViewModel_ConstructorWithLocale_CurrencyCodeSetAsExpected()
        {
            //Given
            const string expectedCurrencyCode = "GBP";
            testLocale = TestHelpers.GetFakeLocaleWithAddress();
            testLocale.LocaleTrait = TestHelpers.MakeLocaleTraitsForLocale();
            testLocale.LocaleTrait.Add(
                new LocaleTrait { traitID = Traits.Currency, traitValue = expectedCurrencyCode });

            //When
            var localeGridRowViewModel = new LocaleGridRowViewModel(testLocale);
            var actualCurrencyCode = localeGridRowViewModel.CurrencyCode;

            //Then
            Assert.AreEqual(expectedCurrencyCode, actualCurrencyCode);
        }
    }
}
