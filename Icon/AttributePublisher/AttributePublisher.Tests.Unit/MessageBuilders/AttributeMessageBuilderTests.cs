using AttributePublisher.DataAccess.Models;
using AttributePublisher.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace AttributePublisher.Tests.Unit.MessageBuilders
{
    [TestClass]
    public class AttributeMessageBuilderTests
    {
        private AttributeMessageBuilder messageBuilder;
        private SerializerWithoutNamepaceAliases<TraitsType> serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new SerializerWithoutNamepaceAliases<TraitsType>();
            messageBuilder = new AttributeMessageBuilder(serializer);
        }

        [TestMethod]
        public void AttributeMessageBuilder_WhenOneAttributeMessagePassed_BuildsMessage()
        {
            //Given
            var attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeId = 123,
                    Description ="TestDescription",
                    TraitCode ="TestTraitCode",
                    MaxLengthAllowed =10,
                    MinimumNumber = "1",
                    MaximumNumber ="5",
                    NumberOfDecimals ="2",
                    XmlTraitDescription ="TestXMLDescription",
                    CharacterSetRegexPattern ="TestPattern",
                    AttributeGroupId = 1,
                    AttributeGroupName ="Test",
                    DataType = "TestDataType",
                    IsPickList = true,
                    PickListValues = new List<string>
                    {
                        "TestPickListValue1",
                        "TestPickListValue2"
                    }
                }
            };

            //When
            var result = messageBuilder.BuildMessage(attributes);

            //Then
            Assert.AreEqual(File.ReadAllText("TestMessages/AttributeMessage.txt"), result);
        }

        [TestMethod]
        public void AttributeMessageBuilder_OneAttributeThatIsNotPickList_BuildsMessage()
        {
            //Given
            var attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeId = 123,
                    Description ="TestDescription",
                    TraitCode ="TestTraitCode",
                    MaxLengthAllowed =10,
                    MinimumNumber = "1",
                    MaximumNumber ="5",
                    NumberOfDecimals ="2",
                    XmlTraitDescription ="TestXMLDescription",
                    CharacterSetRegexPattern ="TestPattern",
                    AttributeGroupId = 1,
                    AttributeGroupName ="Test",
                    DataType = "TestDataType",
                    IsPickList = false
                }
            };

            //When
            var result = messageBuilder.BuildMessage(attributes);

            //Then
            Console.WriteLine(result);
            Assert.AreEqual(File.ReadAllText("TestMessages/AttributeMessage_NotPickList.txt"), result);
        }

        [TestMethod]
        public void AttributeMessageBuilder_OneAttributeThatDoesNotHaveNumberDataTypeFields_BuildsMessage()
        {
            //Given
            var attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeId = 123,
                    Description ="TestDescription",
                    TraitCode ="TestTraitCode",
                    MaxLengthAllowed = 10,
                    XmlTraitDescription ="TestXMLDescription",
                    CharacterSetRegexPattern ="TestPattern",
                    AttributeGroupId = 1,
                    AttributeGroupName ="Test",
                    DataType = "TestDataType",
                    IsPickList = false
                }
            };

            //When
            var result = messageBuilder.BuildMessage(attributes);

            //Then
            Console.WriteLine(result);
            Assert.AreEqual(File.ReadAllText("TestMessages/AttributeMessage_NoNumberFields.txt"), result);
        }

        [TestMethod]
        public void AttributeMessageBuild_OneAttributeWithPrecisionOf10_BuildsMessage()
        {
            //Given
            var attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeId = 123,
                    Description ="TestDescription",
                    TraitCode ="TestTraitCode",
                    MaxLengthAllowed =10,
                    MinimumNumber = "1",
                    MaximumNumber ="99999999.99",
                    NumberOfDecimals ="2",
                    XmlTraitDescription ="TestXMLDescription",
                    CharacterSetRegexPattern ="TestPattern",
                    AttributeGroupId = 1,
                    AttributeGroupName ="Test",
                    DataType = "TestDataType",
                    IsPickList = false
                }
            };

            //When
            var result = messageBuilder.BuildMessage(attributes);

            //Then
            Console.WriteLine(result);
            Assert.AreEqual(File.ReadAllText("TestMessages/AttributeMessage_Precision10.txt"), result);
        }

        [TestMethod]
        public void AttributeMessageBuilder_WhenTwoAttributeMessagesPassed_BuildsMessage()
        {
            //Given
            var attributes = new List<AttributeModel>
            {
                new AttributeModel
                {
                    AttributeId = 123,
                    Description ="TestDescription",
                    TraitCode ="TestTraitCode",
                    MaxLengthAllowed =10,
                    MinimumNumber = "1",
                    MaximumNumber ="5",
                    NumberOfDecimals ="2",
                    XmlTraitDescription ="TestXMLDescription",
                    CharacterSetRegexPattern ="TestPattern",
                    AttributeGroupId = 1,
                    AttributeGroupName ="Test",
                    DataType = "TestDataType",
                    IsPickList = true,
                    PickListValues = new List<string>
                    {
                        "TestPickListValue1",
                        "TestPickListValue2"
                    }
                },
                 new AttributeModel
                {
                    AttributeId = 234,
                    Description ="TestDescription2",
                    TraitCode ="TestTraitCode2",
                    MaxLengthAllowed =10,
                    MinimumNumber = "1",
                    MaximumNumber ="5",
                    NumberOfDecimals ="2",
                    XmlTraitDescription ="TestXMLDescription2",
                    CharacterSetRegexPattern ="TestPattern2",
                    AttributeGroupId = 1,
                    AttributeGroupName ="Test2",
                    DataType = "TestDataType2",
                    IsPickList = true,
                    PickListValues = new List<string>
                    {
                        "TestPickListValue11",
                        "TestPickListValue22"
                    }
                 }
            };            

            //When
            var result = messageBuilder.BuildMessage(attributes);
            //Then
            Assert.AreEqual(File.ReadAllText("TestMessages/AttributeMessage2.txt"), result);
        }
    }
}