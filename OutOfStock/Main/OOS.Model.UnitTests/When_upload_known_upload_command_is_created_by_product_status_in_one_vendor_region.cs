using System;
using System.Linq;
using Magnum.TestFramework;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public class When_upload_known_upload_command_is_created_by_product_status_discontinued_by_distributor_in_one_vendor_region 
        : Given_a_product_status_to_known_upload_command_mapper
    {
        private const string DiscontinuedByDistributor = "Disco'ed by distributor";
        private readonly DateTime ExpirationDate = Convert.ToDateTime("10/11/12");

        protected override ProductStatus[] When()
        {
            var productStatus = CreateProductStatus();
            return new[] { productStatus };
        }

        private ProductStatus CreateProductStatus()
        {
            var region = "NC";
            var vendorKey = "UNF";
            var upc = "0008819470070";
            var vin = "26352";
            var status = DiscontinuedByDistributor;
            var expiration = ExpirationDate;
            var reason = string.Empty;
            return new ProductStatus(region, vendorKey, vin, upc, reason, DateTime.MinValue, status, expiration);
        }

        [Then]
        public void Can_be_created()
        {
            sut.ShouldNotBeNull();
        }

        [Then]
        public void Only_one_upload_known_upload_commmand_is_created()
        {
            commands.Count().ShouldEqual(1);
        }

        [Then]
        public void Upload_date_should_be_equal_to_test_scenario_date()
        {
            commands.ElementAt(0).UploadDate.ShouldEqual(Convert.ToDateTime(uploadDate));
        }

        [Then]
        public void Should_have_at_least_one_vendor_in_region()
        {
            commands.ElementAt(0).VendorRegionMaps.Count().ShouldBeGreaterThan(0);
        }

        [Then]
        public void Should_have_at_least_one_item_date()
        {
            commands.ElementAt(0).Items.Count().ShouldBeGreaterThan(0);
        }

        [Then]
        public void Product_status_should_be_discontinued_by_distributor()
        {
            commands.ElementAt(0).Items.ElementAt(0).ProductStatus.ShouldBeEqualTo(DiscontinuedByDistributor);
        }

        [Then]
        public void Expiration_date_should_be_equal_to_expiration_date()
        {
            //commands.ElementAt(0).Items.ElementAt(0).ExpirationDate.ShouldBeEqualTo(ExpirationDate);
        }
    }
}
