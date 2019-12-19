using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using OOSCommon.Import;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public class When_a_known_upload_has_no_region_vendor_map : Given_a_known_upload_to_be_translated_to_product_status
    {
        protected override void When()
        {
            var item = new OOSKnownItemData("test_upc_001", "3", "01/15/2013", "123456789012");
            var items = new List<OOSKnownItemData> { item };
            upload.Expect(p => p.ItemData).Return(items.ToArray()).Repeat.Once();
            upload.Expect(p => p.VendorRegionMap).Return(null).Repeat.Once();
        }

        [Then]
        public void No_projections_should_be_produced()
        {
            upload.VerifyAllExpectations();
            projections.Count().ShouldBeEqualTo(0);
        }
    }
}
