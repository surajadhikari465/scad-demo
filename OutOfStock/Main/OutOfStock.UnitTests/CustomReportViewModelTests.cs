using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model;
using OOS.Model.UnitTests;
using OutOfStock.Models;
using Rhino.Mocks;
using StructureMap;

namespace OutOfStock.UnitTests
{
    [TestFixture]
    public class CustomReportViewModelTests
    {
        private DateTime startDate;
        private DateTime endDate;

        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            ObjectFactory.Configure(x =>
            {
                x.SelectConstructor(() => new CustomReportViewModel(null, null));
                x.For<IUserProfile>().Use(Bootstrapper.GetUserProfile());
            });
        }


        [Test]
        [Category("Integration Test")]
        public void TestRunQueryPerformance()
        {
            var storeMap = StoreIdsFor(new List<string> { "kir", "lmr" }).ToDictionary(id => id, p => p.ToString());
            var teamMap = new List<string> { "grocery", "whole body" }.ToDictionary(team => team, team => team);
            var subTeamMap = new List<string> { "grocery", "juice bar" }.ToDictionary(sub => sub, sub => sub);
            startDate = Convert.ToDateTime("4/20/2012");
            endDate = Convert.ToDateTime("4/29/2012");

            var sut = CreateObjectUnderTest();
            var todaysDate = endDate;
            var viewModels = sut.RunQuery(startDate, endDate, storeMap, teamMap, subTeamMap, todaysDate);

            Assert.AreEqual(1953, viewModels.Count());
        }

        private CustomReportViewModel CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<CustomReportViewModel>();
        }

        private List<int> StoreIdsFor(List<string> storeAbbrevs)
        {
            var repository = ObjectFactory.GetInstance<IStoreRepository>();
            return storeAbbrevs.Where(abbrev => !string.IsNullOrWhiteSpace(abbrev)).Select(store => repository.ForAbbrev(store).Id).ToList();
        }

    }
}
