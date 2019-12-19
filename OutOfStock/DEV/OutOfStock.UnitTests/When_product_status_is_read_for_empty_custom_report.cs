using System;
using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using OOS.Model;
using OOSCommon.DataContext;
using OutOfStock.Service;
using Rhino.Mocks;

namespace OutOfStock.UnitTests
{
    [Scenario]
    public class When_product_status_is_read_for_empty_custom_report
    {
        private ReadProductStatusService sut;
        private IEnumerable<ProductStatus> status;

        [When]
        public void Given()
        {
            sut = CreateSubjectUnderTest();
            var report = When();
            status = sut.For(report);           
        }

        private ReadProductStatusService CreateSubjectUnderTest()
        {
            var factory = MockRepository.GenerateMock<ICreateDisposableEntities>();
            var objContext = new DisposableMockOOSEntities();
            factory.Expect(p => p.New()).Return(objContext);
            return new ReadProductStatusService(factory);
        }

        private OOSCustomReport When()
        {
            var from = DateTime.Now;
            var to = DateTime.Now;
            return new OOSCustomReport(from, to);
        }


        [Then]
        public void No_product_status_is_found()
        {
            status.Count().ShouldBeEqualTo(0);
        }
    }
}
