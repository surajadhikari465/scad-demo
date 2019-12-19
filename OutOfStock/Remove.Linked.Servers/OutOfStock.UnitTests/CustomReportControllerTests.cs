using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using OOS.Model;
using OOS.Model.UnitTests;
using OutOfStock.Controllers;
using OutOfStock.Models;
using Rhino.Mocks;
using StructureMap;

namespace OutOfStock.UnitTests
{

    [TestFixture]
    public class CustomReportControllerTests
    {
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            ObjectFactory.Configure(x => x.SelectConstructor(() => new CustomReportViewModel(null, null)));
        }


        [Test]
        [Category("Integration Test")]
        public void TestCreateReportCentralUserProfileCannotSaveExcelFileInUnitTesting()
        {
            InjectCentralUserProfileMock();
            InjectExcelModel();

            var controller = CreateObjectUnderTest();

            var region = "sw";
            var stores = string.Join(",", StoreIdsFor(new List<string> { "kir", "lmr" }).Select(id => id.ToString()).ToList());
            var teams = "0";
            var subteams = "0";
            var startDate = "2/20/2012";
            var endDate = "2/26/2012";           

            var result = controller.Create(region, stores, teams, subteams, startDate, endDate, DateTime.Now.ToShortDateString());

            var userProfile = GetUserProfile();
            userProfile.VerifyAllExpectations();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        private void InjectExcelModel()
        {
            ICustomReportExcelModel model = ObjectFactory.GetInstance<CustomReportExcelModel>();
            ObjectFactory.Inject(model);
        }


        private void InjectCentralUserProfileMock()
        {
            var userProfile = MockRepository.GenerateStrictMock<IUserProfile>();
            userProfile.Stub(p => p.IsCentral()).Return(true).Repeat.Once();
            userProfile.Stub(p => p.IsStoreLevel()).Return(true).Repeat.Once();
            ObjectFactory.Inject(userProfile);
        }

        private CustomReportController CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<CustomReportController>();
        }

        private List<int> StoreIdsFor(List<string> storeAbbrevs)
        {
            var repository = ObjectFactory.GetInstance<IStoreRepository>();
            return storeAbbrevs.Where(abbrev => !string.IsNullOrWhiteSpace(abbrev)).Select(store => repository.ForAbbrev(store).Id).ToList();
        }

        private IUserProfile GetUserProfile()
        {
            return ObjectFactory.GetInstance<IUserProfile>();
        }

        [Test]
        [Category("Integration Test")]
        public void TestCentralUserAllStoresInRegionTakesLongTime()
        {
            InjectCentralUserProfileMock();
            InjectExcelModel();
            var controller = CreateObjectUnderTest();

            var region = "nc";
            var stores = string.Empty;
            var startDate = "04/26/2011";
            var endDate = "04/26/2012";

            var result = controller.Create(region, stores, null, null, startDate, endDate, DateTime.Now.ToShortDateString());

            var userProfile = GetUserProfile();
            userProfile.VerifyAllExpectations();
            Assert.IsInstanceOf(typeof(FileStreamResult), result);
        }

        [Test]
        public void TestDependenciesWithoutServiceLocationUsingMocks()
        {
        }

        [Test]
        public void TestIncomingURLs()
        {
            RouteTester.Match("~/", "SummaryReport", "Index");
            RouteTester.Match("~/SummaryReport", "SummaryReport", "Index");
            RouteTester.Match("~/CustomReport", "CustomReport", "Index");
            RouteTester.Match("~/CustomReport/Create/Region", "CustomReport", "Create", new { id = "Region" }, "POST");
            RouteTester.Fail("~/SummaryReport/Central/Lamar/Grocery");
        }

    }
}
