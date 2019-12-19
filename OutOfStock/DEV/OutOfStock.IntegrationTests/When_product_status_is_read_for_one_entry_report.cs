using System;
using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model;
using OOS.Model.UnitTests;
using OOSCommon.DataContext;
using OutOfStock.Service;

namespace OutOfStock.IntegrationTests
{
    [TestFixture]
    public class When_product_status_is_read_for_one_entry_report
    {
        private ReadProductStatusService sut;
        private IEnumerable<ProductStatus> status;

        [When]
        public void Given()
        {
            sut = CreateObjectUnderTest();
            var report = When();
            status = sut.For(report);
        }

        private ReadProductStatusService CreateObjectUnderTest()
        {
            var entityFactory = new EntityFactory(MockConfigurator.New());
            return new ReadProductStatusService(entityFactory);
        }

        private OOSCustomReport When()
        {
            var report = new OOSCustomReport(DateTime.Now, DateTime.Now);
            return report;
        }

        [Then]
        public void Sut_is_not_null()
        {
            sut.ShouldNotBeNull();
        }

        [Then]
        public void One_product_status_is_returned()
        {
            status.Count().ShouldBeEqualTo(1);
        }

        [Then]
        public void Product_status_upc_is_discontinued()
        {
            status.ElementAt(0).Status.ShouldEqual("Discontinued by distributor");
        }
    }
}
