using Esb.Core.Serializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using WebSupport.DataAccess.Models;
using WebSupport.MessageBuilders;
using WebSupport.Models;
using WebSupport.ViewModels;
using Contracts = Icon.Esb.Schemas.Infor.ContractTypes;

namespace WebSupport.Tests.Models
{
    [TestClass]
    public class CheckPointRequestMessageBuilderTests
    {
        private CheckPointRequestMessageBuilder checkPointRequestMessageBuilder;
        private SerializerWithoutNamepaceAliases<Contracts.PriceChangeMaster> serializer;
        private CheckPointRequestBuilderModel request;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new SerializerWithoutNamepaceAliases<Contracts.PriceChangeMaster>();
            checkPointRequestMessageBuilder = new CheckPointRequestMessageBuilder(serializer);
            request = new CheckPointRequestBuilderModel();
        }

        [TestMethod]
        public void BuildMessage_ReturnsCheckPointRequestMessage()

        {   //Given
            request.CheckPointRequestViewModel = new CheckPointRequestViewModel
            {
              Store = "10001",
              ScanCode = "4282342774",
              RegionIndex = 0
            };

           request.getCurrentPriceInfo = new CheckPointMessageModel
           {
             BusinessUnitID = 1234,           
             ItemId = 1,
             PatchFamilyId = "1-1234",
             SequenceId = 1
           };

            //When
            var message = checkPointRequestMessageBuilder.BuildMessage(request);

            //Then
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Contracts.PriceChangeMaster));
            MemoryStream stream = new MemoryStream();
            XDocument document = XDocument.Parse(message);
            document.Save(stream);
            stream.Position = 0;
            Contracts.PriceChangeMaster priceChangeMaster = xmlSerializer.Deserialize(stream) as Contracts.PriceChangeMaster;

            Assert.IsNotNull(priceChangeMaster);
            Assert.AreEqual(priceChangeMaster.PriceChangeHeader[0].PatchFamilyID, request.getCurrentPriceInfo.PatchFamilyId);
            Assert.AreEqual(priceChangeMaster.PriceChangeHeader[0].PatchNum, request.getCurrentPriceInfo.SequenceId.ToString());
            Assert.AreEqual(priceChangeMaster.PriceChangeHeader[0].isCheckPoint, true);
        }
    }
}