using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Icon.Web.Tests.Unit.Framework
{
    [TestClass] [Ignore]
    public class NonMerchandiseTraitTests
    {
        private IconContext context;

        [TestMethod]
        public void NonMerchandiseConstants_CompareWithDatabaseValues_DatabaseValuesShouldMatchConstants()
        {
            // Given.
            context = new IconContext();
            
            // When.
            var databaseTraits = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern.Split('|');

            // Then.
            Assert.AreEqual(databaseTraits[0], NonMerchandiseTraits.BottleDeposit);
            Assert.AreEqual(databaseTraits[1], NonMerchandiseTraits.Crv);
            Assert.AreEqual(databaseTraits[2], NonMerchandiseTraits.Coupon);
            Assert.AreEqual(databaseTraits[3], NonMerchandiseTraits.BottleReturn);
            Assert.AreEqual(databaseTraits[4], NonMerchandiseTraits.CrvCredit);
        }
    }
}
