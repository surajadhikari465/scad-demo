using Icon.Esb.Subscriber;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.AttributeListener.MessageParsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace Mammoth.Esb.AttributeListener.Tests.MessageParsers
{
	[TestClass]
	public class AttributeMessageParserTests
	{
		private AttributeMessageParser parser;

		[TestInitialize]
		public void Initialize()
		{
			parser = new AttributeMessageParser();
		}

        [TestMethod]
        public void ParseMessage_SingleAttributeInMessage()
        {
            //Given
            Mock<IEsbMessage> message = new Mock<IEsbMessage>();
            message.Setup(m => m.MessageText)
                .Returns(File.ReadAllText("TestMessages/AttributeMessage_Single.xml"));

            //When
            var attributes = parser.ParseMessage(message.Object);

            //Then
            Assert.AreEqual(1, attributes.Attribute.Length);

            var attribute = attributes.Attribute[0];
            Assert.AreEqual("TestName", attribute.Name);
            Assert.AreEqual("TTC", attribute.TraitCode);
            Assert.AreEqual("TestDescription", attribute.Description);
            Assert.AreEqual("Test", attribute.Group);
            Assert.AreEqual("TestDataType", attribute.DataType);
        }

        [TestMethod]
		public void ParseMessage_MultipleAttributesInMessage()
		{
			//Given
			Mock<IEsbMessage> message = new Mock<IEsbMessage>();
			message.Setup(m => m.MessageText)
				.Returns(File.ReadAllText("TestMessages/AttributeMessage_Multi.xml"));

			//When
            var attributes = parser.ParseMessage(message.Object);

            //Then
            Assert.AreEqual(2, attributes.Attribute.Length);

            var attribute = attributes.Attribute[0];
            Assert.AreEqual("TestName", attribute.Name);
            Assert.AreEqual("TTC", attribute.TraitCode);
            Assert.AreEqual("TestDescription", attribute.Description);
            Assert.AreEqual("Test", attribute.Group);
            Assert.AreEqual("TestDataType", attribute.DataType);

            attribute = attributes.Attribute[1];
            Assert.AreEqual("TestName2", attribute.Name);
            Assert.AreEqual("TTD", attribute.TraitCode);
            Assert.AreEqual("TestDescription2", attribute.Description);
            Assert.AreEqual("Test2", attribute.Group);
            Assert.AreEqual("TestDataType2", attribute.DataType);
        }
	}
}