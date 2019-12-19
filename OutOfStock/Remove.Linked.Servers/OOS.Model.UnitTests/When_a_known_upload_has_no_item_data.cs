using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    public class When_a_known_upload_has_no_item_data  : Given_a_known_upload_to_be_translated_to_product_status
    {
        private const string VendorMapName = "vendor_map_001";
        private const string Region = "NC";
        private const string VendorKey = "REN";

        protected override void When()
        {
            upload.Expect(p => p.ItemData).Return(null).Repeat.Once();
            var map = new OOSKnownVendorRegionMap(VendorMapName, Region, VendorKey);
            var maps = new List<OOSKnownVendorRegionMap> { map };
            upload.Expect(p => p.VendorRegionMap).Return(maps.ToArray()).Repeat.Once();
        }

        [Then]
        public void No_projections_should_be_produced()
        {
            upload.VerifyAllExpectations();
            projections.Count().ShouldBeEqualTo(0);
        }
    }
}
