using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Exceptions;
using PushController.Controller.CacheHelpers;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;

namespace PushController.Tests.Controller.CacheHelperTests
{
    [TestClass]
    public class LocaleCacheHelperTests
    {
        private LocaleCacheHelper cacheHelper;
        private Mock<IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>>> mockGetLocalesQueryHandler;
        private List<int> testBusinessUnits;
        private int testLocaleId;

        [TestInitialize]
        public void Initialize()
        {
            mockGetLocalesQueryHandler = new Mock<IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>>>();
            testLocaleId = 99;

            Cache.businessUnitToLocale.Clear();
        }

        private void BuildCacheHelper()
        {
            cacheHelper = new LocaleCacheHelper(mockGetLocalesQueryHandler.Object);
        }

        [TestMethod]
        public void PopulateLocaleCache_LocaleIsAlreadyCached_LocaleShouldNotBeCachedAgain()
        {
            // Given.
            BuildCacheHelper();

            testBusinessUnits = new List<int> { 11111, 22222, 33333 };

            foreach (var businessUnit in testBusinessUnits)
            {
                Cache.businessUnitToLocale.Add(businessUnit, new Locale());
            }

            // When.
            cacheHelper.Populate(new List<int> { 22222 });

            // Then.
            Assert.AreEqual(testBusinessUnits.Count, Cache.businessUnitToLocale.Count);
        }

        [TestMethod]
        public void PopulateLocaleCache_LocaleIsNotYetCached_LocaleShouldBeCached()
        {
            // Given.
            mockGetLocalesQueryHandler.Setup(q => q.Execute(It.IsAny<GetLocalesByBusinessUnitIdQuery>())).Returns(
                new List<Locale> 
                { 
                    new Locale 
                    { 
                        localeID = testLocaleId,
                        LocaleTrait = new List<LocaleTrait> 
                        { 
                            new LocaleTrait 
                            { 
                                traitID = Traits.PsBusinessUnitId,
                                traitValue = "44444",
                                Trait = new Trait
                                {
                                    traitID = Traits.PsBusinessUnitId,
                                    traitCode = TraitCodes.PsBusinessUnitId
                                }
                            } 
                        } 
                    } 
                });

            BuildCacheHelper();

            testBusinessUnits = new List<int> { 11111, 22222, 33333 };

            foreach (var businessUnit in testBusinessUnits)
            {
                Cache.businessUnitToLocale.Add(businessUnit, new Locale());
            }

            // When.
            cacheHelper.Populate(new List<int> { 44444 });

            // Then.
            var cachedLocale = Cache.businessUnitToLocale[44444];

            Assert.AreEqual(testBusinessUnits.Count + 1, Cache.businessUnitToLocale.Count);
            Assert.IsNotNull(cachedLocale);
            Assert.AreEqual("44444", cachedLocale.LocaleTrait.Single(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue);
            Assert.AreEqual(testLocaleId, cachedLocale.localeID);
        }

        [TestMethod]
        public void RetrieveCachedLocale_LocaleIsFound_LocaleShouldBeReturned()
        {
            // Given.
            BuildCacheHelper();

            testBusinessUnits = new List<int> { 11111, 22222, 33333 };

            foreach (var businessUnit in testBusinessUnits)
            {
                Cache.businessUnitToLocale.Add(businessUnit, new Locale
                    {
                        localeID = testLocaleId,
                        LocaleTrait = new List<LocaleTrait> 
                        { 
                            new LocaleTrait 
                            { 
                                traitID = Traits.PsBusinessUnitId,
                                traitValue = "22222",
                                Trait = new Trait
                                {
                                    traitID = Traits.PsBusinessUnitId,
                                    traitCode = TraitCodes.PsBusinessUnitId
                                }
                            } 
                        }
                    });
            }

            // When.
            var retrievedLocale = cacheHelper.Retrieve(22222);

            // Then.
            Assert.AreEqual("22222", retrievedLocale.LocaleTrait.Single(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue);
            Assert.AreEqual(testLocaleId, retrievedLocale.localeID);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessUnitNotFoundException))]
        public void RetrieveCachedLocale_LocaleIsNotFound_ExceptionShouldBeThrown()
        {
            // Given.
            BuildCacheHelper();

            testBusinessUnits = new List<int> { 11111, 22222, 33333 };

            foreach (var businessUnit in testBusinessUnits)
            {
                Cache.businessUnitToLocale.Add(businessUnit, new Locale
                    {
                        localeID = testLocaleId,
                        LocaleTrait = new List<LocaleTrait> 
                        { 
                            new LocaleTrait 
                            { 
                                traitID = Traits.PsBusinessUnitId,
                                traitValue = "22222",
                                Trait = new Trait
                                {
                                    traitID = Traits.PsBusinessUnitId,
                                    traitCode = TraitCodes.PsBusinessUnitId
                                }
                            } 
                        }
                    });
            }

            // When.
            var retrievedLocale = cacheHelper.Retrieve(44444);

            // Then.
            // Expected exception.
        }
    }
}
