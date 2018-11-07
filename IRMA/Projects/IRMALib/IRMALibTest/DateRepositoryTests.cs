using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WholeFoods.Common.IRMALib.Dates;

namespace IRMALibTest
{
    [TestClass]
    public class DateRepositoryTests : IRMALibTestBase
    {
        protected DateRepository dateRepo;

        [TestInitialize]
        public void TestInit()
        {
            dateRepo = new DateRepository(base.ConnectionString_FLD);
        }

        [TestMethod]
        public void CanGetCurrentFiscalInfo()
        {
            //Arrrange
            //Act
            int year = dateRepo.CurrentFiscalYear();
            int quarter = dateRepo.CurrentFiscalQuarter();
            int period = dateRepo.CurrentFiscalPeriod();
            //Assert
            Assert.IsNotNull(year);
            Assert.IsNotNull(quarter);
            Assert.IsNotNull(period);
        }

        [TestMethod]
        public void CanGetCorrectFiscalInfo()
        {
            //Arrrange
            DateTime myDate = new DateTime(2016, 11, 5, 10, 49, 34);
            //Act
            int year = dateRepo.FiscalYear(myDate);
            int quarter = dateRepo.FiscalQuarter(myDate);
            int period = dateRepo.FiscalPeriod(myDate);
            //Assert
            Assert.AreEqual(2017, year);
            Assert.AreEqual(1, quarter);
            Assert.AreEqual(2, period);
        }

        [TestMethod]
        public void CanFindLastWeekdayInQuarter()
        {
            //Arrrange
            DateTime myDate = new DateTime(2017, 1, 20);
            DateTime expectedDate = new DateTime(2017, 4, 7);
            //Act
            DateTime lastWeekday = dateRepo.LastWeekdayInQuarter(myDate);
            //Assert
            Assert.AreEqual(expectedDate.Date, lastWeekday.Date);
        }

        [TestMethod]
        public void CanFindNearestWeekdayAfter()
        {
            //Arrrange
            //Act
            DateTime myDate = new DateTime(2017, 1, 21);
            DateTime wkDayAfter = dateRepo.NearestWeekdayAfter(myDate);
            DateTime compDate = new DateTime(2017, 1, 23);
            //Assert
            Assert.AreEqual(compDate.Date, wkDayAfter.Date);
        }

        [TestMethod]
        public void CanFindNearestWeekdayBefore()
        {
            //Arrrange
            DateTime myDate = new DateTime(2017, 1, 14);
            DateTime compDate = new DateTime(2017, 1, 13);
            //Act
            DateTime wkDayBefore = dateRepo.NearestWeekdayBefore(myDate);
            //Assert
            Assert.AreEqual(compDate.Date, wkDayBefore.Date);
        }

        [TestMethod]
        public void CanFindQuarterStart()
        {
            DateTime? qStart = dateRepo.GetQuarterStart(1, 2017);
            DateTime test = new DateTime(2016, 9, 26);
            //Assert
            Assert.AreEqual(test.Date, qStart.Value.Date);
        }

        [TestMethod]
        public void FindQuarterStartReturnsNullIfOutOfRange()
        {
            //Arrrange
            //Act
            // table doesn't go out this far
            DateTime? qStart = dateRepo.GetQuarterStart(1, 2060);
            //Assert
            Assert.IsNull(qStart);
        }
    }
}
