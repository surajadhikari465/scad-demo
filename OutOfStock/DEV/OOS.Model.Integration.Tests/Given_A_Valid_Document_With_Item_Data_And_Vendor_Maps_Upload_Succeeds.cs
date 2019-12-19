using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model.UnitTests;
using OOSCommon.Import;
using StructureMap;

namespace OOS.Model.Integration.Tests
{
    [TestFixture]
    public class Given_A_Valid_Document_With_Item_Data_And_Vendor_Maps_Upload_Succeeds
    {
        private KnownUploadService sut;
        private KnownUpload uploadDoc;
        private bool result;

        [When]
        public void A_Known_Upload_Document_Is_Uploaded()
        {
            result = sut.Upload(uploadDoc);
        }

        [Given]
        public void A_Valid_Document_With_Item_Data_And_Vendor_Maps()
        {
            Setup();
            uploadDoc = CreateKnownUpload();

        }

        private void Setup()
        {
            Bootstrap();
            sut = CreateObjectUnderTest();
        }

        private void Bootstrap()
        {
            ObjectFactory.Configure(config =>
            {
                config.For<ICreateKnownUploader>().Use<KnownUploaderFactory>();
                config.For<ILogService>().Use<LogService>();
                config.For<IConfigurator>().Use(MockConfigurator.New());
            });
        }

        private KnownUploadService CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<KnownUploadService>();
        }


        private KnownUpload CreateKnownUpload()
        {
            var itemData = new List<OOSKnownItemData>
                               {
                                   new OOSKnownItemData("TestUpc001", "3", "2012-11-28 21:48:14.287", "12345"),
                               };
            var vendorMaps = new List<OOSKnownVendorRegionMap>
                                {
                                    new OOSKnownVendorRegionMap("RegionMap1Name", "NC", "VendorKey1"),          
                                };
            var uploadDoc = new KnownUpload(DateTime.Now);
            itemData.ForEach(uploadDoc.AddItem);
            vendorMaps.ForEach(uploadDoc.AddVendorRegion);
            return uploadDoc;
        }


        [Then]
        public void The_Document_Upload_Is_Successful()
        {
            result.ShouldBeTrue();
        }
    }
}
