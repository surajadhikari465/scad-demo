using Esb.Core.Serializer;
using Icon.Esb.Schemas.Infor.ContractTypes;
using Icon.Esb.Services.ConfirmationBod;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Services.Tests.ConfirmationBod
{
    [TestClass]
    public class ConfirmationBodMessageBuilderTests
    {
        private ConfirmationBodMessageBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new ConfirmationBodMessageBuilder(new Serializer<ConfirmBODType>());
        }

        [TestMethod]
        public void BuildMessage_DataIssue_MessageHasDataIssueAsReasonCode()
        {
            //Given
            ConfirmationBodEsbRequest request = new ConfirmationBodEsbRequest
            {
                BodId = Guid.NewGuid().ToString(),
                ErrorDescription = "Test Error description",
                ErrorReasonCode = "TestErrorReasonCode",
                ErrorType = ConfirmationBodEsbErrorTypes.Data,
                OriginalMessage = File.ReadAllText("TestMessages//4000089Message.xml"),
                TenantId = "Test"
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.IsNotNull(message);
        }
    }
}
