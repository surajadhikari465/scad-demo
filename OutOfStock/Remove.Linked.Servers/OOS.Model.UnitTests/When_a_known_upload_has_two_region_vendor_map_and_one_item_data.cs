using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public class When_a_known_upload_has_two_region_vendor_map_and_one_item_data : Given_a_known_upload_to_be_translated_to_product_status
    {
        private const string Upc = "test_upc_001";
        private const string ReasonCode = "3";
        private const string StartDate = "01/15/2013";
        private const string Vin = "1234567";

        private const string FirstVendorMapName = "vendor_map_001";
        private const string FirstRegion = "NC";
        private const string VendorKey = "REN";
        private const string SecondVendorMapName = "vendor_map_002";
        private const string SecondRegion = "SW";

        protected override void When()
        {
            var item = new OOSKnownItemData(Upc, ReasonCode, StartDate, Vin);
            var items = new List<OOSKnownItemData> { item };
            upload.Expect(p => p.ItemData).Return(items.ToArray()).Repeat.Once();

            var firstMap = new OOSKnownVendorRegionMap(FirstVendorMapName, FirstRegion, VendorKey);
            var secondMap = new OOSKnownVendorRegionMap(SecondVendorMapName, SecondRegion, VendorKey);
            var maps = new List<OOSKnownVendorRegionMap> { firstMap, secondMap };
            upload.Expect(p => p.VendorRegionMap).Return(maps.ToArray()).Repeat.Once();
        }

        [Then]
        public void Two_product_status_projections_are_produced()
        {
            projections.Count().ShouldEqual(2);
        }

        [Then]
        public void Produced_projections_should_equal_two_product_status()
        {
            var firstProductStatus = new ProductStatus(FirstRegion, VendorKey, Vin, Upc);
            var secondProductStatus = new ProductStatus(SecondRegion, VendorKey, Vin, Upc);
            projections.ElementAt(0).ShouldEqual(firstProductStatus);
            projections.ElementAt(1).ShouldEqual(secondProductStatus);
        }
    }
}
