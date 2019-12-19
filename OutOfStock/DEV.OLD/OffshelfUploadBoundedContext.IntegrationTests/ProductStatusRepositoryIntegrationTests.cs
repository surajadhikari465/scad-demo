using System;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model;
using OOS.Model.UnitTests;
using OOSCommon.DataContext;

namespace OffshelfUploadBoundedContext.IntegrationTests
{
    [TestFixture]
    public class ProductStatusRepositoryIntegrationTests
    {
        private IProductStatusRepository sut;

        [SetUp]
        public void Setup()
        {
            sut = CreateObjectUnderTest();
        }

        [Test]
        public void Create()
        {
            Assert.IsNotNull(sut);
        }

        private IProductStatusRepository CreateObjectUnderTest()
        {
            var entityFactory = new EntityFactory(MockConfigurator.New());
            return new ProductStatusRepository(entityFactory);
        }

        [Test]
        public void Given_a_new_product_status_when_a_new_product_status_is_added_then_the_product_status_is_found()
        {
            var productStatus = NewProductStatus();
            sut.Insert(productStatus);

            var searched = SearchFor(productStatus);
            Assert.AreEqual(productStatus, searched, "The product statuses are not equal.");
        }


        private ProductStatus SearchFor(ProductStatus productStatus)
        {
            var upc = productStatus.Upc;
            var region = productStatus.Region;
            var vendorKey = productStatus.VendorKey;
            var vin = productStatus.Vin;
            return sut.For(region, vendorKey, vin, upc); ;
        }

        private ProductStatus NewProductStatus()
        {
            var upc = "0063551914501";
            var region = "NC";
            var vendorKey = "REN";
            var vin = "000000060028";
            var reason = "Disco'd by manufacturer";
            var startDate = Convert.ToDateTime("01/10/2012");
            //return new ProductStatus(region, vendorKey, vin, upc, reason, startDate, "Going to expire soon", Convert.ToDateTime("12/01/2012"));
            throw new NotImplementedException();
        }

        [Test]
        public void Given_a_non_existing_product_status_when_the_product_status_is_searched_then_the_product_status_is_not_found()
        {
            var productStatus = SearchFor(NewNonExistingProductStatus());
            Assert.IsNull(productStatus);
        }

        private ProductStatus NewNonExistingProductStatus()
        {
            var region = "NC";
            var vendor = "REN";
            var vin = "000000060028";
            var upc = "test_upc";
            var reason = "Disco'd by manufacturer";
            var startDate = DateTime.MinValue;
            var status = "Expired";
            var expirationDate = DateTime.MinValue;
            //return new ProductStatus(region, vendor, vin, upc, reason, startDate, status, expirationDate);
            throw new NotImplementedException();
        }

        [Test]
        public void Given_an_existing_product_status_when_the_same_product_status_is_added_then_the_product_status_is_not_added_again()
        {
            var productStatus = NewProductStatus();
            sut.Insert(productStatus);
            sut.Insert(productStatus);
            Assert.AreEqual(1, sut.Count);
        }

        [Test]
        public void Given_existing_product_status_when_the_product_status_is_updated_then_the_product_status_is_updated()
        {
            var productStatus = NewProductStatus();
            
            sut.Insert(productStatus);
            var status = NewExpiredProductStatus();
            sut.Modify(status);

            var changed = SearchFor(productStatus);
            Assert.AreEqual(1, sut.Count, "The product status not modified");
            Assert.AreEqual("Expired", changed.Status, "Product Status not equal.");
        }

        private ProductStatus NewExpiredProductStatus()
        {
            var upc = "0063551914501";
            var region = "NC";
            var vendorKey = "REN";
            var vin = "000000060028";
            var reason = "Disco'd by manufacturer";
            var startDate = Convert.ToDateTime("01/10/2012");
            //return new ProductStatus(region, vendorKey, vin, upc, reason, startDate, "Expired", Convert.ToDateTime("12/01/2012"));
            throw new NotImplementedException();
        }

        [Test]
        public void Given_a_new_product_status_when_the_product_status_is_modified_then_the_product_status_is_not_updated()
        {
            sut.Modify(NewNonExistingProductStatus());
            Assert.AreEqual(0, sut.Count);
        }

        [Test]
        public void Given_an_existing_product_status_when_the_product_status_is_removed_the_product_status_is_not_found()
        {
            var status = NewProductStatus();
            sut.Insert(status);
            Assert.AreEqual(1, sut.Count);
            sut.Remove(status);
            Assert.AreEqual(0, sut.Count);

        }

        [Test]
        public void When_a_new_product_status_in_another_region_is_seeded()
        {
            var upc = "0063551914501";
            var region = "SW";
            var vendorKey = "REN";
            var vin = "000000060028";
            var reason = "Disco'd by manufacturer";
            var startDate = Convert.ToDateTime("01/10/2012");
            //var status = new ProductStatus(region, vendorKey, vin, upc, reason, startDate, "Going to expire soon", Convert.ToDateTime("12/01/2012"));
            throw new NotImplementedException();

            //sut.Insert(status);
            //sut.Count.ShouldBeEqualTo(1);
        }

    }
}
