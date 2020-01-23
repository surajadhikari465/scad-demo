using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Mammoth.PrimeAffinity.Library.Tests.Tests.MessageBuilders
{
    [TestClass]
    public class PrimeAffinityMessageBuilderTests
    {
        private PrimeAffinityMessageBuilder messageBuilder;
        private Serializer<items> serializer;
        private PrimeAffinityMessageBuilderParameters request;
        private PrimeAffinityMessageBuilderSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new Serializer<items>();
            settings = new PrimeAffinityMessageBuilderSettings { PrimeAffinityPsgName = "Test", PrimeAffinityPsgType = "Consumable" };
            messageBuilder = new PrimeAffinityMessageBuilder(serializer, settings);
            request = new PrimeAffinityMessageBuilderParameters();
        }

        [TestMethod]
        public void BuildMessage_PricesExist_ShouldReturnMessageWithPsgAssociation()
        {
            //Given
            request.PrimeAffinityMessageModels = new List<PrimeAffinityMessageModel>
            {
                new PrimeAffinityMessageModel
                {
                    BusinessUnitID = 1234,
                    ItemID = 12345,
                    ItemTypeCode = ItemTypes.Codes.RetailSale,
                    MessageAction = ActionEnum.AddOrUpdate,
                    ScanCode = "123456",
                    StoreName = "Test Store"
                },
                new PrimeAffinityMessageModel
                {
                    BusinessUnitID = 12341,
                    ItemID = 123451,
                    ItemTypeCode = ItemTypes.Codes.RetailSale,
                    MessageAction = ActionEnum.AddOrUpdate,
                    ScanCode = "1234561",
                    StoreName = "Test Store1"
                },
                new PrimeAffinityMessageModel
                {
                    BusinessUnitID = 12342,
                    ItemID = 123452,
                    ItemTypeCode = ItemTypes.Codes.RetailSale,
                    MessageAction = ActionEnum.AddOrUpdate,
                    ScanCode = "1234562",
                    StoreName = "Test Store2"
                }
            };

            //When
            var message = messageBuilder.BuildMessage(request);

            //Then
            Assert.AreEqual(File.ReadAllText("TestMessages/PsgMessage.txt"), message);
        }
    }
}
