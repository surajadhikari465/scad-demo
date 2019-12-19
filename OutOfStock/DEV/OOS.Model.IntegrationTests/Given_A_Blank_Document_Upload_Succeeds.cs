using System;
using Magnum.TestFramework;
using NUnit.Framework;
using OOS.Model.IntegrationTests;
using OOSCommon.Import;

namespace OOS.Model.Integration.Tests
{
    [TestFixture]
    public class Given_A_Blank_Document_Upload_Succeeds : Given_a_known_upload_service
    {
        private KnownUpload uploadDoc;
        private bool result;

        protected override void Given()
        {
            uploadDoc = new KnownUpload(DateTime.Now);
        }


        [When]
        public void A_Known_Upload_Document_Is_Uploaded()
        {
            result = sut.Upload(uploadDoc);
        }


        [Then]
        public void The_Document_Upload_Is_Successful()
        {
            result.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class Given_A_Document_With_Null_Upload_Date_Then_Document_Validation_Fails
    {
    }
        
    [TestFixture]
    public class Given_A_Document_With_Vendor_Key_Len_Greater_Than_10_Then_Document_Validation_Fails
    {
    }

    [TestFixture]
    public class Given_A_Document_With_Non_Integer_Reason_Code_Then_Document_Validation_Fails
    {
    }

    [TestFixture]
    public class Given_A_Document_With_Unmapped_Reason_Code_Then_Document_Validation_Fails
    {
    }

    [TestFixture]
    public class Given_A_Document_With_Non_Integer_Vin_Number_Then_Document_Validation_Fails
    {
    }
}
