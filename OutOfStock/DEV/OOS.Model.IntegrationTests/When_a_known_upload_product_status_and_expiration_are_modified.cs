using System;
using Magnum.TestFramework;
using OOSCommon.Import;

namespace OOS.Model.IntegrationTests
{
    [Scenario]
    public class When_a_known_upload_product_status_and_expiration_are_modified : Given_a_known_upload_that_has_product_status_and_expiration
    {
        private DateTime updatedExpirationDate = Convert.ToDateTime("01/31/2013");
        private const string updatedProductStatus = "Disco'ed by manufacturer";

        protected override void When()
        {
            upload.AddItem(new OOSKnownItemData("0009999999999", "2", "01/28/2013", "122943", updatedProductStatus, updatedExpirationDate));
            upload.AddVendorRegion(new OOSKnownVendorRegionMap("REN-RM", "RM", "REN"));
            sut.Reset();
            sut.Modify(upload);
            searched = sut.For(date);
        }

        [Then]
        public void Two_item_data_are_found()
        {
            searched.ItemData.Length.ShouldBeEqualTo(2);
        }


    }
}
