using System;
using System.Collections.Generic;
using Magnum.TestFramework;
using OOSCommon;
using OOSCommon.Import;
using OutOfStock.Messages;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public abstract class Given_a_known_upload
    {
        protected const string Vin = "123456";
        protected const string VendorKey = "vendor_key";
        protected const string Upc = "known_upc_001";
        protected const string ReasonCode = "2";
        protected const string StartDate = "01/03/2013";
        protected const string Region = "NC";

        protected DateTime date = Convert.ToDateTime("01/03/2012");
        protected IEnumerable<OOSKnownItemData> itemData = new[] { new OOSKnownItemData(Upc, ReasonCode, StartDate, Vin) };
        protected IEnumerable<OOSKnownVendorRegionMap> vendorMap = new[] { new OOSKnownVendorRegionMap("vendor_key-NC", Region, VendorKey) };

        protected KnownUpload sut;
        protected List<Event> events;
        protected IBuildKnownUpload builder;

        [When]
        public void When_a_command_arrives()
        {
            var handler = OnHandler();
            var cmd = Command(); 
            
            When();
            handler.Handle(cmd);
            sut = CreateObjectUnderTest();
            events = new List<Event>(sut.GetChanges());
        }

        private KnownUpload CreateObjectUnderTest()
        {
            return builder.ToKnownUpload();
        }

        protected abstract KnownUploadCommand Command();
        protected abstract Handles<KnownUploadCommand> OnHandler();
        protected abstract void When();
    }
}
