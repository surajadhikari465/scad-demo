using AttributePublisher.DataAccess.Models;
using AttributePublisher.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace AttributePublisher.Tests.Unit.MessageBuilders
{
    [TestClass]
    public class AttributeMessageBuilderTests
    {
        private AttributeMessageBuilder messageBuilder;
        private SerializerWithoutNamepaceAliases<AttributesType> serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new SerializerWithoutNamepaceAliases<AttributesType>();
            messageBuilder = new AttributeMessageBuilder(serializer);
        }

        [TestMethod]
        public void AttributeMessageBuilder_1Attribute_BuildsMessage()
        {
            //Given
            var attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeGroupName = "Test",
                    AttributeId = 1,
                    AttributeName = "TestName",
                    DataType = "TestDataTYpe",
                    TraitCode = "TestTraitCode",
                    XmlTraitDescription = "TestDescription"
                }
            };

            //When
            var result = messageBuilder.BuildMessage(attributes);

            //Then
            Assert.AreEqual(File.ReadAllText("TestMessages/AttributeMessage.txt"), result);
        }
    }
}
