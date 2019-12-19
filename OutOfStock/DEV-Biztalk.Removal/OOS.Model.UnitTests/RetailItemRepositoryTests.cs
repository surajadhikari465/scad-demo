using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.Repository;
using Rhino.Mocks;
using SharedKernel;
using StructureMap;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class RetailItemRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
            var userProfile = Bootstrapper.GetUserProfile();
            ObjectFactory.Configure(p => p.For<IUserProfile>().Use(userProfile));
        }

        [Test]
        public void TestCreateSystemUnderTest()
        {
            var sut = CreateSystemUnderTest();
        }

        [Test]
        [Category("Integration Test")]
        [ExpectedException("OOS.Model.InvalidUPCException")]
        public void TestSystemThrowsExceptionWhenProductUPCNotValid()
        {
            var sut = CreateSystemUnderTest();
            
            var upc = "";
            var item = sut.For(upc, "LMR");
        }

        private IRetailItemRepository CreateSystemUnderTest()
        {
            return ObjectFactory.GetInstance<RetailItemRepository>();
        }

        [Test]
        [Category("Integration Test")]
        public void TestSystemReturnsSingleRetailItemWhenStoreUPCValid()
        {
            var sut = CreateSystemUnderTest();
            var upc = "0001600027564";
            var item = sut.For(upc, "LMR");
            Assert.AreEqual(3.39, item.Price);
        }

        [Test]
        public void TestRetailInfoHasPriceWhenUPCValid()
        {
            var sut = CreateSystemUnderTest();
            var upcs = new List<string> {"0008941900014", 
                                        "0008860300970", 
                                        "0083560300502",
                                        "0083560300538",
                                        "0001813831501",
                                        "0008469241855",
                                        "0084329100499",
                                        "0001669782202",
                                        "0008143403101",
                                        "0008143403101",
                                        "0779806708069",
                                        "0078458500068",
                                        "0003598826271",
                                        "0066494100260",
                                        "0069944600005",
                                        "0008858600399",
                                        "0008143403100",
                                        "0843700869590",
                                        "0080329424860",
                                        "0008689102287",
                                        "0076150372399",
                                        "0075573800087",
                                        "0001235408184",
                                        "0000000080578"};

            var item = sut.For(upcs, "LMR");
            Assert.AreEqual(23, item.Count);
            Assert.Greater(item[0].Price, new Cost(item[0]).UnitCost);
        }

        [Test]
        [Category("Integration Test")]
        public void Test()
        {
            var upc = "0009492255667";
            var sut = CreateSystemUnderTest();
            var item = sut.For(upc, "LMR");
            Assert.AreEqual(19.99, item.Price);
        }
    }
}
