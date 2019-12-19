using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class InclusiveTimeZoneEndDateSpecificationTests
    {
        private DateTime endDate;
        private DateTime todaysDate;

        [Test]
        public void Create()
        {
            var sut = CreateObjectUnderTest();
        }

        private InclusiveTimeZoneEndDateSpecification CreateObjectUnderTest()
        {
            return new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate);
        }

        [Test]
        public void Given_Todays_Date_Same_As_End_Date_Inclusive_Date_Should_Be_Two_Days_Past_End_Date()
        {
            todaysDate = Convert.ToDateTime("10/30/2012");
            endDate = todaysDate;
            var sut = CreateObjectUnderTest();
            
            var inclusiveDate = sut.InclusiveEndDate;

            Assert.AreEqual(inclusiveDate, endDate.AddDays(2));
        }

        [Test]
        public void Given_Todays_Date_Different_Than_End_Date_Inclusive_Date_Should_Be_One_Day_Past_End_Date()
        {
            todaysDate = Convert.ToDateTime("10/30/2012");
            endDate = Convert.ToDateTime("10/29/2012");

            var sut = CreateObjectUnderTest();

            var inclusiveDate = sut.InclusiveEndDate;
            Assert.AreEqual(inclusiveDate, endDate.AddDays(1));
        }

    }
}
