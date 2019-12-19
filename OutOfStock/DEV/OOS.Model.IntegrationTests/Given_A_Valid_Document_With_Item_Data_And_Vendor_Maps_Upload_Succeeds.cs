using System;
using System.Collections.Generic;
using System.Linq;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model.IntegrationTests;
using OOSCommon.Import;

namespace OOS.Model.Integration.Tests
{
    [TestFixture]
    public class Given_A_Valid_Document_With_Item_Data_And_Vendor_Maps_Upload_Succeeds : Given_a_known_upload_service
    {
        private KnownUpload uploadDoc;
        private bool result;

        protected override void Given()
        {
            uploadDoc = CreateValidKnownUpload();
        }

        private KnownUpload CreateValidKnownUpload()
        {
            var date = Convert.ToDateTime("12/25/2012 8:38:24 AM");
            var itemData = new List<OOSKnownItemData> { new OOSKnownItemData("TestUpc002", "3", "2012-10-09 21:09:14.287", "12345"), };
            var vendorMaps = new List<OOSKnownVendorRegionMap> { new OOSKnownVendorRegionMap("RegionMap2Name", "NC", "VendorKey1"), };
            return KnownUploadFrom(date, itemData, vendorMaps);
        }


        [When]
        public void A_Known_Upload_Document_Is_Uploaded()
        {
            result = sut.Upload(uploadDoc);
        }


        private KnownUpload KnownUploadFrom(DateTime uploadDate, IEnumerable<OOSKnownItemData> itemData, IEnumerable<OOSKnownVendorRegionMap> vendorMaps)
        {
            var upload = new KnownUpload(uploadDate);
            itemData.ToList().ForEach(upload.AddItem);
            vendorMaps.ToList().ForEach(upload.AddVendorRegion);
            return upload;
        }

        [Then]
        public void The_Document_Upload_Is_Successful()
        {
            result.ShouldBeTrue();
        }
    }
}
