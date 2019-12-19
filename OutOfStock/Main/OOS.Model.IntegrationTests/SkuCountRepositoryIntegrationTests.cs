using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOS.Model.UnitTests;
using StructureMap;

namespace OOS.Model.IntegrationTests
{
    [TestFixture]
    public class SkuCountRepositoryIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
            Bootstrapper.Bootstrap();
        }

        [Test]
        public void Create()
        {
            var sut = CreateSystemUnderTest();
        }

        private ISkuCountRepository CreateSystemUnderTest()
        {
            return ObjectFactory.GetInstance<ISkuCountRepository>();
        }

        [Test]
        public void Given_An_Existing_Sku_Count_If_A_Search_Is_Performed_The_Sku_Is_Found()
        {
            var sut = CreateSystemUnderTest();
            var store = "PTH";
            var team = "Grocery";
            var skuCount = sut.For(store, team);
            Assert.AreEqual("PTH", skuCount.StoreAbbreviation);
            Assert.AreEqual("Grocery", skuCount.Team);
            Assert.AreEqual(9190, skuCount.Count);
        }

        [Test]
        public void Given_New_Sku_Count_If_User_Searches_Sku_Count_Then_Sku_Count_Is_Found()
        {
            var sut = CreateSystemUnderTest();
            sut.Insert("BCA", "Whole Body", 1234);

            var searched = sut.For("BCA", "Whole Body");
            Assert.AreEqual(1234, searched.Count);
        }

        [Test]
        public void Given_Non_Existing_Sku_Count_If_User_Searches_Sku_Count_Then_Sku_Count_Is_Not_Found()
        {
            var sut = CreateSystemUnderTest();

            var searched = sut.For("BEE", "Grocery");
            Assert.IsNull(searched);
        }

        [Test]
        public void Given_Existing_Sku_Count_If_User_Adds_The_Same_Sku_Count_Then_Sku_Count_Is_Not_Added_Twice()
        {
            var sut = CreateSystemUnderTest();
            sut.Insert("BCA", "Whole Body", 1234);
            sut.Insert("BCA", "Whole Body", 1234);

            Assert.AreEqual(1, sut.Count);
        }

        [Test]
        public void Given_Existing_Sku_Count_If_User_Adds_Sku_Count_Then_Sku_Count_Is_Updated()
        {
            var sut = CreateSystemUnderTest();
            sut.Insert("BCA", "Whole Body", 1234);

            sut.Modify("BCA", "Whole Body", 4322);
            var searched = sut.For("BCA", "Whole Body");
            Assert.AreEqual(4322, searched.Count);
        }

        [Test]
        public void Given_Existing_Sku_Count_If_User_Modifies_Sku_Count_Then_Sku_Is_Updated()
        {
            var sut = CreateSystemUnderTest();
            sut.Modify("BCA", "Whole Body", 999);
            Assert.AreEqual(1, sut.Count);
        }

        [Test]
        public void Given_New_Sku_Count_If_User_Modifies_Sku_Count_Then_Sku_Is_Not_Updated()
        {
            var sut = CreateSystemUnderTest();
            sut.Modify("", "Whole Body", 999);
            Assert.AreEqual(0, sut.Count);
        }

        [Test]
        public void Given_Existing_Sku_Count_If_User_Removes_Sku_Count_Then_Sku_Is_Not_Found()
        {
            var sut = CreateSystemUnderTest();
            sut.Remove("BCA", "Whole Body");

            var searched = sut.For("BCA", "Whole Body");
            Assert.IsNull(searched);
        }
    }
}
