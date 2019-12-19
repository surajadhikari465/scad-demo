using Magnum.TestFramework;

namespace OOS.Model.IntegrationTests
{
    [Scenario]
    public class When_a_known_upload_that_has_product_status_and_expiration_date_is_inserted : Given_a_known_upload_that_has_product_status_and_expiration
    {
        protected override void When()
        {
            sut.Reset();
            searched = sut.For(date);
        }


        [Then]
        public void Can_be_created()
        {
            sut.ShouldNotBeNull();
        }

        [Then]
        public void Searched_inserted_upload_should_be_the_same()
        {
            searched.ShouldEqual(upload);
        }

        [Then]
        public void Only_single_item_data_is_found()
        {
            searched.ItemData.Length.ShouldEqual(1);
        }

        [Then]
        public void Only_single_vendor_region_is_found()
        {
            searched.VendorRegionMap.Length.ShouldEqual(1);
        }

        [Then]
        public void Product_status_is_matched_up()
        {
            searched.ItemData[0].ProductStatus.ShouldBeEqualTo(ExpireSoonProductStatus);
        }

        [Then]
        public void Expiration_date_should_be_matched_up()
        {
         //   searched.ItemData[0].ExpirationDate.ShouldBeEqualTo(date);
        }

    }
}
