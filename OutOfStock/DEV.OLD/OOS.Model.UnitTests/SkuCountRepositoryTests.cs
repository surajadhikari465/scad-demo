using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OOSCommon.DataContext;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public class SkuCountRepositoryTests
    {
        [Test]
        public void Create()
        {
            var sut = CreateSystemUnderTest();
        }

        private SkuCountRepository CreateSystemUnderTest()
        {
            var entityFactory = MockRepository.GenerateStub<IOOSEntitiesFactory>();
            var oosEntities = new DisposableMockOOSEntities();
            entityFactory.Stub(p => p.New()).Return(oosEntities).Repeat.Any();
            return new SkuCountRepository(entityFactory);
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
            
            var searched = sut.For("BCA", "Whole Body");
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
        public void Given_New_Sku_Count_If_User_Modifies_Sku_Count_Then_Sku_Is_Not_Updated()
        {
            var sut = CreateSystemUnderTest();
            sut.Modify("BCA", "Whole Body", 999);
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
