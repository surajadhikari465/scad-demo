using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magnum.TestFramework;
using NUnit.Framework;
using OOSCommon.DataContext;
using SharedKernel;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class CustomReportRepositoryTests
    {
        private List<int> storeIds;
        private List<string> teams;
        private List<string> subTeams;
        private DateTime startDate;
        private DateTime endDate;


        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            var userProfile = Bootstrapper.GetUserProfile();
            ObjectFactory.Configure(p => p.For<IUserProfile>().Use(userProfile));
        }
        

        [Test]
        [Category("Integration Test")]
        public void TestGenerateCustomReport()
        {
            storeIds = GetStoreIds(new List<string> { "kir", "lmr" });
            teams = new List<string> { "grocery", "whole body" };
            subTeams = new List<string> { "grocery", "juice bar" };
            startDate = Convert.ToDateTime("2/20/2012");
            endDate = Convert.ToDateTime("2/26/2012");

            var result = CustomReportFor();
            Assert.AreEqual("", result.ElementAt(0).ProductStatus);
            Assert.AreEqual(1929, result.Count);
        }

        private List<int> GetStoreIds(List<string> storeAbbrevs)
        {
            var repository = ObjectFactory.GetInstance<IStoreRepository>();
            var stores = new List<Store>();
            storeAbbrevs.ForEach(p => stores.Add(repository.ForAbbrev(p)));
            return stores.Where(p => p != null).Select(p => p.Id).ToList();
        }

        private List<CustomReportDTO> CustomReportFor()
        {
            var repository = CreateObjectUnderTest();
            endDate = new InclusiveTimeZoneEndDateSpecification(endDate, endDate).InclusiveEndDate;
            return repository.CustomReportFor(startDate, endDate, storeIds, teams, subTeams);
        }

        private CustomReportRepository CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<CustomReportRepository>();
        }

        [Test]
        [Category("Integration Test")]
        public void TestStoreListEmpty()
        {
            storeIds = GetStoreIds(new List<string> { "", "" });
            teams = new List<string> { "grocery", "whole body" };
            subTeams = new List<string> { "grocery", "juice bar" };
            startDate = Convert.ToDateTime("2/20/2012");
            endDate = Convert.ToDateTime("2/26/2012");

            var result = CustomReportFor();
            Assert.AreEqual(5802, result.Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestEmptyTeamNames()
        {
            storeIds = GetStoreIds(new List<string> { "", "" });
            teams = new List<string> { "", null };
            subTeams = new List<string> { "grocery", "juice bar" };
            startDate = Convert.ToDateTime("2/20/2012");
            endDate = Convert.ToDateTime("2/26/2012");

            var result = CustomReportFor();
            Assert.AreEqual(5802, result.Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestEmptySubteams()
        {
            storeIds = GetStoreIds(new List<string> { null, "" });
            teams = new List<string> { "grocery", "whole body" };
            subTeams = new List<string> { "", null };
            startDate = Convert.ToDateTime("2/20/2012");
            endDate = Convert.ToDateTime("2/26/2012");

            var result = CustomReportFor();
            Assert.AreEqual(10327, result.Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestPerformanceWithoutAnyFiltering()
        {
            storeIds = GetStoreIds(new List<string> { null, "" });
            teams = new List<string> { "", "" };
            subTeams = new List<string> { "", null };
            startDate = Convert.ToDateTime("12/25/2011");
            endDate = Convert.ToDateTime("3/01/2012");

            var result = CustomReportFor();
            Assert.AreEqual(50208, result.Count);
        }

        [Test]
        [Category("Integration Test")]
        public void TestPerformanceWithStoreFiltering()
        {
            storeIds = GetStoreIds(new List<string> { "lmr"});
            teams = new List<string> { "", "" };
            subTeams = new List<string> { "", null };
            startDate = Convert.ToDateTime("03/31/2012");
            endDate = Convert.ToDateTime("04/24/2012");

            var result = CustomReportFor();
            Assert.AreEqual(1592, result.Count);
        }

        [Test]
        [Category("Do Not Run")]
        public void PopulateProductDataBoundedContextAllAtOnce()
        {
            var upcs = GetReportedOOSUPCs();
            var productMap = GetOOSProductMap(upcs);
            
            Assert.Greater(productMap.Count, 0);

            // I am commenting the code out to insure we will not run the code unless intentionally
            // until TFS Category support is functional

            using (var oosDataContext = ObjectFactory.GetInstance<IOOSEntitiesFactory>().New() as OOSEntities)
            {
                var reportsGroupedByHeader = (from c in oosDataContext.REPORT_DETAIL select c).GroupBy(p => p.REPORT_HEADER_ID).ToList();

                Assert.Greater(reportsGroupedByHeader.Count, 0);

                foreach (var g in reportsGroupedByHeader)
                {
                    foreach (var reportDetail in g)
                    {
                        //var upc = reportDetail.UPC;
                        //if (!productMap.ContainsKey(upc)) continue;

                        //var productData = productMap[upc];
                        //reportDetail.BRAND = productData.Brand;
                        //reportDetail.LONG_DESCRIPTION = productData.LongDescription;
                        //reportDetail.ITEM_SIZE = productData.Size;
                        //reportDetail.ITEM_UOM = productData.UOM;
                        //reportDetail.CATEGORY_NAME = productData.CategoryName;
                        //reportDetail.CLASS_NAME = productData.ClassName;
                        
                    }
                    //oosDataContext.SaveChanges();

                }
            }
        }

        private List<string> GetReportedOOSUPCs()
        {
            var config = ObjectFactory.GetInstance<IConfigurator>();
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var upcsQuery = "select distinct UPC from REPORT_DETAIL";
                var oosItems = oosDataContext.ExecuteQuery<string>(upcsQuery, new object[] { });
                var upcs = oosItems.ToList();
                return upcs;
            }
        }

        private Dictionary<string, IProduct> GetOOSProductMap(IEnumerable<string> upcs)
        {
            var productRepository = ObjectFactory.GetInstance<IProductRepository>();
            var products = productRepository.For(upcs);
            return products.ToDictionary(p => p.UPC.Code, q => q);
        }




        [Test]
        public void When_product_status_for_a_product_store_scan_is_found()
        {
            storeIds = GetStoreIds(new List<string> { "bld",});
            teams = new List<string> { "grocery",};
            subTeams = new List<string> { "dairy", };
            startDate = Convert.ToDateTime("04/18/2012");
            endDate = Convert.ToDateTime("12/25/2012");

            var result = CustomReportFor();
            result.Where(p => p.UPC == "0002066210126").ElementAt(0).ProductStatus.ShouldEqual("Going to expire");
        }
    }
}
