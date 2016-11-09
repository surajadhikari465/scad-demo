using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Ewic.Tests.Serialization.Serializers
{
    [TestClass]
    public class EwicMappingSerializerTests
    {
        private EwicMappingSerializer serializer;
        private string testAplScanCode;
        private string testWfmScanCode;
        private string testAgencyId;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new EwicMappingSerializer();

            testAplScanCode = "222222222";
            testWfmScanCode = "222222223";
            testAgencyId = "ZX";
        }

        [TestMethod]
        public void Serialize_AddMapping_ActionShouldBeAddOrUpdate()
        {
            // Given.
            var model = new EwicMappingMessageModel
            {
                ActionTypeId = MessageActionTypes.AddOrUpdate,
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode,
                AgencyId = testAgencyId
            };

            // When.
            string serializedMessage = serializer.Serialize(model);

            // Then.
            bool isAddOrUpdate = serializedMessage.Contains("AddOrUpdate");
            bool isDelete = serializedMessage.Contains("Delete");

            Assert.IsTrue(isAddOrUpdate);
            Assert.IsFalse(isDelete);
        }

        [TestMethod]
        public void Serialize_RemovingMapping_ActionShouldBeDelete()
        {
            // Given.
            var model = new EwicMappingMessageModel
            {
                ActionTypeId = MessageActionTypes.Delete,
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode,
                AgencyId = testAgencyId
            };

            // When.
            string serializedMessage = serializer.Serialize(model);

            // Then.
            bool isAddOrUpdate = serializedMessage.Contains("AddOrUpdate");
            bool isDelete = serializedMessage.Contains("Delete");

            Assert.IsFalse(isAddOrUpdate);
            Assert.IsTrue(isDelete);
        }
    }
}
