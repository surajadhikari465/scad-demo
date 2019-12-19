using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using OOSCommon;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public class When_a_known_upload_has_one_region_vendor_map_and_one_item_data : Given_a_known_upload_to_be_translated_to_product_status
    {
        protected override void When()
        {
            var item = new OOSKnownItemData("test_upc_001", "3", "01/15/2013", "123456789012");
            var items = new List<OOSKnownItemData> { item };
            upload.Expect(p => p.ItemData).Return(items.ToArray()).Repeat.Once();

            var map = new OOSKnownVendorRegionMap("vendor_map_001", "NC", "REN");
            var maps = new List<OOSKnownVendorRegionMap> { map };
            upload.Expect(p => p.VendorRegionMap).Return(maps.ToArray()).Repeat.Once();
        }


        [Then]
        public void Sut_can_be_created()
        {
            sut.ShouldNotBeNull();
        }

        [Then]
        public void One_projection_is_produced()
        {
            upload.VerifyAllExpectations();
            projections.Count().ShouldEqual(1);
        }

    }
}
