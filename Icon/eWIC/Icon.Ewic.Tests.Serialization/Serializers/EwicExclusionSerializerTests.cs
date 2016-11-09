using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Ewic.Tests.Serialization.Serializers
{
    [TestClass]
    public class EwicExclusionSerializerTests
    {
        private EwicExclusionSerializer serializer;
        private string testExclusion;
        private string testAgencyId;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new EwicExclusionSerializer();

            testExclusion = "222222222";
            testAgencyId = "ZX";
        }

        [TestMethod]
        public void Serialize_AddingAnExclusion_ActionShouldBeAddOrUpdate()
        {
            // Given.
            var model = new EwicExclusionMessageModel
            {
                ActionTypeId = MessageActionTypes.AddOrUpdate,
                ScanCode = testExclusion,
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
        public void Serialize_RemovingAnExclusion_ActionShouldBeDelete()
        {
            // Given.
            var model = new EwicExclusionMessageModel
            {
                ActionTypeId = MessageActionTypes.Delete,
                ScanCode = testExclusion,
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

        [TestMethod]
        public void Serialize_SerializingNationalBarcode_ElementShouldBePaddedToFifteenCharacters()
        {
            // Given.
            var scanCodes = new List<string>
            {
                "222",
                "2222222",
                "222222222222222"
            };

            var models = new List<EwicExclusionMessageModel>
            {
                new EwicExclusionMessageModel
                {
                    ActionTypeId = MessageActionTypes.AddOrUpdate,
                    ScanCode = scanCodes[0],
                    AgencyId = testAgencyId
                },
                new EwicExclusionMessageModel
                {
                    ActionTypeId = MessageActionTypes.AddOrUpdate,
                    ScanCode = scanCodes[1],
                    AgencyId = testAgencyId
                },
                new EwicExclusionMessageModel
                {
                    ActionTypeId = MessageActionTypes.AddOrUpdate,
                    ScanCode = scanCodes[2],
                    AgencyId = testAgencyId
                }
            };

            // When.
            List<string> serializedMessages = new List<string>();

            foreach (var model in models)
            {
                serializedMessages.Add(serializer.Serialize(model));
            }

            // Then.
            for (int i = 0; i < models.Count; i++)
            {
                Assert.IsTrue(serializedMessages[i].Contains(scanCodes[i].PadLeft(15, '0')));
            }
        }
    }
}
