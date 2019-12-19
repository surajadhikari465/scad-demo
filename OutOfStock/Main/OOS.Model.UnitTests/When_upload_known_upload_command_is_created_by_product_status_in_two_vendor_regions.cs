using System;
using System.Linq;
using Magnum.TestFramework;
using OutOfStock.Messages;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public class When_upload_known_upload_command_is_created_by_product_status_in_two_vendor_regions : Given_a_product_status_to_known_upload_command_mapper
    {
        private string vendorKey = "UNF";
        private string upc = "0008819470070";
        private string vin = "26352";
        private string status = "";
        private DateTime expiration = Convert.ToDateTime("10/11/12");

        protected override ProductStatus[] When()
        {
            var statusInVendorRegion1 = CreateProductStatusInVendorRegion1();
            var statusInVendorRegion2 = CreateProductStatusInVendorRegion2();
            return new[] { statusInVendorRegion1, statusInVendorRegion2 };
        }

        private ProductStatus CreateProductStatusInVendorRegion2()
        {
            var region = "NC";
            return new ProductStatus(region, vendorKey, vin, upc, string.Empty, DateTime.MinValue, status, expiration);
        }

        private ProductStatus CreateProductStatusInVendorRegion1()
        {
            var region = "RM";
            return new ProductStatus(region, vendorKey, vin, upc, string.Empty, DateTime.MinValue, status, expiration);
        }

        [Then]
        public void Two_commands_are_created()
        {
            commands.Count().ShouldEqual(2);
        }

        [Then]
        public void Upload_known_upload_command_is_created()
        {
            commands.ElementAt(0).GetType().ShouldEqual(typeof(KnownUploadCommand));
        }

        [Then]
        public void First_known_upload_command_upload_date_should_be_equal_to_date_passed_into_factory()
        {
            commands.ElementAt(0).UploadDate.ShouldBeEqualTo(Convert.ToDateTime(uploadDate));
        }

        [Then]
        public void Second_known_upload_command_upload_should_not_be_equal_to_first_command_upload_date()
        {
            commands.ElementAt(1).UploadDate.ShouldNotBeEqualTo(commands.ElementAt(0).UploadDate);
        }

        [Then]
        public void Only_one_vendor_region_is_created_in_each_command()
        {
            commands.ElementAt(0).VendorRegionMaps.Count().ShouldBeEqualTo(1);
            commands.ElementAt(1).VendorRegionMaps.Count().ShouldBeEqualTo(1);
        }

        [Then]
        public void Only_one_item_data_is_created_in_each_command()
        {
            commands.ElementAt(0).Items.Count().ShouldBeEqualTo(1);
            commands.ElementAt(1).Items.Count().ShouldBeEqualTo(1);
        }
    }
}
