using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.MessageParsers
{
    [TestClass]
    public class HierarchyClassMessageParserTests
    {
        private HierarchyClassMessageParser messageParser;
        private Mock<IEsbMessage> mockEsbMessage;
        private Mock<ILogger<HierarchyClassMessageParser>> mockLogger;
        private HierarchyClassListenerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new HierarchyClassListenerSettings();
            mockLogger = new Mock<ILogger<HierarchyClassMessageParser>>();
            messageParser = new HierarchyClassMessageParser(settings, mockLogger.Object);
            mockEsbMessage = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void ParseMessage_BrandMessageWith1Brand_ShouldReturn1BrandHierarchyClassModel()
        {
            //Given
            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(File.ReadAllText(@"TestMessages\BrandTestMessage.xml"));

            //When
            var hierarchyClasses = messageParser.ParseMessage(mockEsbMessage.Object);

            //Then
            Assert.AreEqual(1, hierarchyClasses.Count());

            var hierarchyClass = hierarchyClasses.First();
            Assert.AreEqual(129887, hierarchyClass.HierarchyClassId);
            Assert.AreEqual("Test Brand", hierarchyClass.HierarchyClassName);
            Assert.AreEqual(0, hierarchyClass.ParentHierarchyClassId);
            Assert.AreEqual("Brand", hierarchyClass.HierarchyLevelName);
            Assert.AreEqual(Hierarchies.Names.Brands, hierarchyClass.HierarchyName);

            Assert.AreEqual(1, hierarchyClass.HierarchyClassTraits.Count());
            Assert.AreEqual("TST B ABR", hierarchyClass.HierarchyClassTraits[Traits.Codes.BrandAbbreviation]);
            Assert.AreEqual(null, hierarchyClass.SequenceId);
        }

        [TestMethod]
        public void ParseMessage_ValidateSequenceIdIsTrue_ShouldParseSequenceId()
        {
            //Given
            settings.ValidateSequenceId = true;
            mockEsbMessage.Setup(m => m.GetProperty("SequenceID"))
                .Returns("12345");
            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(File.ReadAllText(@"TestMessages\BrandTestMessage.xml"));

            //When
            var hierarchyClasses = messageParser.ParseMessage(mockEsbMessage.Object);

            //Then
            Assert.AreEqual(1, hierarchyClasses.Count());

            var hierarchyClass = hierarchyClasses.First();
            Assert.AreEqual(129887, hierarchyClass.HierarchyClassId);
            Assert.AreEqual("Test Brand", hierarchyClass.HierarchyClassName);
            Assert.AreEqual(0, hierarchyClass.ParentHierarchyClassId);
            Assert.AreEqual("Brand", hierarchyClass.HierarchyLevelName);
            Assert.AreEqual(Hierarchies.Names.Brands, hierarchyClass.HierarchyName);

            Assert.AreEqual(1, hierarchyClass.HierarchyClassTraits.Count());
            Assert.AreEqual("TST B ABR", hierarchyClass.HierarchyClassTraits[Traits.Codes.BrandAbbreviation]);
            Assert.AreEqual(12345, hierarchyClass.SequenceId);
        }
    }
}
