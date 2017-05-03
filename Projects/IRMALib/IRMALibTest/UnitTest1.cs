using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WholeFoods.Common.IRMALib;
using WholeFoods.Common.IRMALib.Dates;

namespace IRMALibTest
{
    [TestClass]
    public class IRMALibTest
    {
        #region Config Tests

        [TestMethod]
        public void CanLoadConfig()
        {
            ConfigRepository cr = new ConfigRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            bool configLoaded = cr.LoadConfig(new Guid("2898A7FC-1CD3-4132-8A69-ED07CC526B14"), new Guid("20C5DDAC-659C-4B81-84F6-5F79CC390D10"));
            Assert.IsTrue(configLoaded);
        }

        [TestMethod]
        public void CanFindBasePath()
        {
            ConfigRepository cr = new ConfigRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            bool configLoaded = cr.LoadConfig(new Guid("2898A7FC-1CD3-4132-8A69-ED07CC526B14"), new Guid("20C5DDAC-659C-4B81-84F6-5F79CC390D10"));
            string basePath = "";
            bool gotBasePath = cr.ConfigurationGetValue("BasePath", ref basePath);
            Assert.IsTrue(gotBasePath);
        }

        [TestMethod]
        public void CanUpdateKey()
        {
            ConfigRepository cr = new ConfigRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            // add key
            bool canUpdate = cr.UpdateKeyValue(new Guid("2898A7FC-1CD3-4132-8A69-ED07CC526B14"), new Guid("20C5DDAC-659C-4B81-84F6-5F79CC390D10"), "test", "test", 0);
            Assert.IsTrue(canUpdate);
        }

        #endregion

        #region Fiscal Year Tests

        [TestMethod]
        public void CanGetCurrentFiscalInfo()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            int year = dr.CurrentFiscalYear();
            Assert.AreEqual(2013, year);
            int quarter = dr.CurrentFiscalQuarter();
            Assert.AreEqual(1, quarter);
            int period = dr.CurrentFiscalPeriod();
            Assert.AreEqual(2, period);
        }

        [TestMethod]
        public void CanGetCorrectFiscalInfo()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            DateTime myDate = new DateTime(2012, 11, 5, 10, 49, 34);
            int year = dr.FiscalYear(myDate);
            Assert.AreEqual(2013, year);
            int quarter = dr.FiscalQuarter(myDate);
            Assert.AreEqual(1, quarter);
            int period = dr.FiscalPeriod(myDate);
            Assert.AreEqual(2, period);
        }

        [TestMethod]
        public void CanFindLastWeekdayInQuarter()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            DateTime myDate = new DateTime(2013, 1, 20);
            DateTime lastWeekday = dr.LastWeekdayInQuarter(myDate);
            DateTime compDate = new DateTime(2013, 1, 18);
            Assert.AreEqual(compDate.Date, lastWeekday.Date);
        }

        [TestMethod]
        public void CanFindNearestWeekdayAfter()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            DateTime myDate = new DateTime(2013, 1, 19);
            DateTime wkDayAfter = dr.NearestWeekdayAfter(myDate);
            DateTime compDate = new DateTime(2013, 1, 21);
            Assert.AreEqual(compDate.Date, wkDayAfter.Date);
        }

        [TestMethod]
        public void CanFindNearestWeekdayBefore()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            DateTime myDate = new DateTime(2013, 1, 19);
            DateTime wkDayBefore = dr.NearestWeekdayBefore(myDate);
            DateTime compDate = new DateTime(2013, 1, 18);
            Assert.AreEqual(compDate.Date, wkDayBefore.Date);
        }

        [TestMethod]
        public void CanFindQuarterStart()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            DateTime? qStart = dr.GetQuarterStart(1, 2013);
            DateTime test = new DateTime(2012, 10, 1);
            Assert.AreEqual(test.Date, qStart.Value.Date);
        }

        [TestMethod]
        public void FindQuarterStartReturnsNullIfOutOfRange()
        {
            DateRepository dr = new DateRepository(@"Data Source=idt-MW\MWt;Initial Catalog=ItemCatalog_test;Integrated Security=True");
            // table doesn't go out this far
            DateTime? qStart = dr.GetQuarterStart(1, 2060);
            Assert.IsNull(qStart);
        }

        #endregion
    }
}
