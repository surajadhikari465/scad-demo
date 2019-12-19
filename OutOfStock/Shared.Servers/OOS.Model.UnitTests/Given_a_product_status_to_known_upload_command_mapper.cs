using System;
using System.Collections.Generic;
using Magnum.TestFramework;
using NUnit.Framework;
using OutOfStock.Messages;

namespace OOS.Model.UnitTests
{
    [TestFixture]
    public abstract class Given_a_product_status_to_known_upload_command_mapper
    {
        protected ProductStatusToKnownUploadCommandMapper sut;
        protected IEnumerable<KnownUploadCommand> commands;
        protected const string uploadDate = "11/12/2013";

        [When]
        public void Given()
        {
            sut = CreateObjectUnderTest();
            var statuses = When();
            commands = sut.Map(Convert.ToDateTime(uploadDate), statuses);
        }

        private ProductStatusToKnownUploadCommandMapper CreateObjectUnderTest()
        {
            return new ProductStatusToKnownUploadCommandMapper();
        }

        protected abstract ProductStatus[] When();

    }
}
