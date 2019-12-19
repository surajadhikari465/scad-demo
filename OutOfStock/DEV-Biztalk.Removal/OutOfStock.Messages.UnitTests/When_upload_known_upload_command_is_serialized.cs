using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace OutOfStock.Messages.UnitTests
{
    [TestFixture]
    public class When_upload_known_upload_command_with_null_product_status_is_serialized
    {
        private IEnumerable<KnownUploadItem> items = new List<KnownUploadItem>
                                                         {
                                                             new KnownUploadItem
                                                             {
                                                                 ExpirationDate = DateTime.Now,
                                                                 ProductStatus = null,
                                                                 ReasonCode = "3",
                                                                 StartDate = DateTime.Now,
                                                                 Upc = "Test_upc_001",
                                                                 Vin = "26352"
                                                             }
                                                         };

        private IEnumerable<KnownUploadVendorRegion> vendorRegions = new List<KnownUploadVendorRegion>
                                                                         {
                                                                             new KnownUploadVendorRegion
                                                                             {
                                                                                 Region = "CN",
                                                                                 Vendor = "REN",
                                                                             }
                                                                         };

        private KnownUploadCommand sut;
        private MemoryStream stream;
        private KnownUploadCommand command;

        [TestFixtureSetUp]
        public void Setup()
        {
            sut = CreateObjectUnderTest();
            stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);
            command = formatter.Deserialize(stream) as KnownUploadCommand;
        }

        private KnownUploadCommand CreateObjectUnderTest()
        {
            return new KnownUploadCommand { Items = items, UploadDate = DateTime.Now, VendorRegionMaps = vendorRegions };
        }

        [Test]
        public void Can_be_deserialized()
        {
            Assert.IsNotNull(command);
        }

        [Test]
        public void Upload_item_count_is_equal()
        {
            Assert.AreEqual(command.Items.Count(), items.Count());
        }

        [Test]
        public void Should_only_be_one_item()
        {
            Assert.AreEqual(1, command.Items.Count());
        }

        [Test]
        public void Deserialized_product_status_is_null()
        {
            Assert.IsNull(command.Items.ElementAt(0).ProductStatus);
        }
    }
}
