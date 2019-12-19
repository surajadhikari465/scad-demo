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
    public class Given_A_Blank_Document_Upload_Succeeds
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
        public void The_Blank_Document()
        {
            Setup();
        
            uploadDoc = new KnownUpload(DateTime.Now);
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
