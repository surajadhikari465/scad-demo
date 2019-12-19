using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model;
using OOS.Model.UnitTests;
using OutOfStock.Controllers;
using OutOfStock.Service;
using Rhino.Mocks;
using StructureMap;

namespace OutOfStock.UnitTests
{
    [TestFixture]
    public class SummaryReportServiceTests
    {
        private DateTime startDate;
        private DateTime endDate;

        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();


            DateTime.TryParse("12/26/2011", out startDate);
            DateTime.TryParse("2/19/2012", out endDate);
        }


        [Test]
        [Category("Integration Test")]
        public void TestSummaryReportServiceForRegionalBuyer()
        {
            InjectRegionalUserProfile();
            InjectSummaryReportAdapter();

            var service = CreateObjectUnderTest();
            var summary = service.SummaryReportFor(startDate, endDate, "sw");
            Assert.AreEqual(16, summary.Count());
            
            var userProfile = GetUserProfile();
            userProfile.VerifyAllExpectations();
        }

        private void InjectRegionalUserProfile()
        {
            var userProfile = MockRepository.GenerateMock<IUserProfile>();
            userProfile.Stub(p => p.IsRegionBuyer()).Return(true).Repeat.Once();
            userProfile.AssertWasNotCalled(p => p.UserStoreAbbreviation());
            ObjectFactory.Inject(userProfile);
        }

        private void InjectSummaryReportAdapter()
        {
            ISummaryReportAdapter adapter = new SummaryReportAdapter();
            ObjectFactory.Inject(adapter);
        }

        private IUserProfile GetUserProfile()
        {
            return ObjectFactory.GetInstance<IUserProfile>();
        }

        private SummaryReportService CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<SummaryReportService>();
        }

        [Test]
        [Category("Integration Test")]
        public void TestInvalidRegionReturnsEmptySummary()
        {
            InjectRegionalUserProfile();
            InjectSummaryReportAdapter();

            var service = CreateObjectUnderTest();
            var summary = service.SummaryReportFor(startDate, endDate, "CE");
            Assert.AreEqual(0, summary.Count());

            var userProfile = GetUserProfile();
            userProfile.VerifyAllExpectations();
        }

        [Test]
        [Category("Integration Test")]
        public void TestSummaryReportServiceForStoreLevelUser()
        {
            InjectLocalLevelUserProfile("kir");
            InjectSummaryReportAdapter();

            var service = CreateObjectUnderTest();
            var summary = service.SummaryReportFor(startDate, endDate, "sw");
            Assert.AreEqual(1, summary.Count());
            Assert.AreEqual("Kirby", summary.ElementAt(0).storeName);

            var userProfile = GetUserProfile();
            userProfile.VerifyAllExpectations();
        }

        private void InjectLocalLevelUserProfile(string storeAbbrev)
        {
            var userProfile = MockRepository.GenerateMock<IUserProfile>();
            userProfile.Stub(p => p.IsRegionBuyer()).Return(false).Repeat.Once();
            userProfile.Stub(p => p.UserStoreAbbreviation()).Return(storeAbbrev).Repeat.Once();
            ObjectFactory.Inject(userProfile);
        }

        [Test]
        [Category("Integration Test")]
        public void TestInvalidStoreReturnsEmptySummary()
        {
            InjectLocalLevelUserProfile("xxx");
            InjectSummaryReportAdapter();

            var service = CreateObjectUnderTest();
            var summary = service.SummaryReportFor(startDate, endDate, "sw");
            Assert.AreEqual(0, summary.Count());
            var userProfile = GetUserProfile();
            userProfile.VerifyAllExpectations();
        }

        [Test]
        public void TestInvalidRegionSummaryReturnsEmptySummary()
        {
            var repositoryFactory = MockRepository.GenerateStub<ISummaryRepositoryFactory>();
            repositoryFactory.Stub(p => p.New("CE")).Return(null);
            var summaryAdapter = MockRepository.GenerateStub<ISummaryReportAdapter>();
            var service = new SummaryReportService(repositoryFactory, summaryAdapter);

            var result = service.SummaryReportFor(startDate, endDate, "CE");

            Assert.IsTrue(result.Count() == 0);
        }

    }
}
