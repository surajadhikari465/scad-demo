using Icon.Infor.Listeners.HierarchyClass.MessageBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Moq;
using Icon.Infor.Listeners.HierarchyClass.Requests;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.MessageBuilders
{
    [TestClass]
    public class HierarchyClassMessageBuilderTests
    {
        private HierarchyClassMessageBuilder messageBuilder;
        private Serializer<Contracts.HierarchyType> serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new Serializer<Contracts.HierarchyType>();
            messageBuilder = new HierarchyClassMessageBuilder(serializer);
        }

        [TestMethod]
        public void BuildMessage_HierarchyClassesExist_ShouldBuildTheMessage()
        {
            //Given
            HierarchyClassEsbServiceRequest request = new HierarchyClassEsbServiceRequest
            {
                Action = ActionEnum.AddOrUpdate,
                HierarchyClasses = new List<VimHierarchyClassModel>
                {
                    new VimHierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "Test",
                        HierarchyName = "Test",
                        HierarchyLevelName = "Test",
                        MessageId = "Test",
                        ParentHierarchyClassId = 1,
                        HierarchyClassTraits = new Dictionary<string, string>
                        {
                            { "Test", "Test" }
                        }
                    },
                    new VimHierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "Test",
                        HierarchyName = "Test",
                        HierarchyLevelName = "Test",
                        MessageId = "Test",
                        ParentHierarchyClassId = 1,
                        HierarchyClassTraits = new Dictionary<string, string> { }
                    }
                }
            };
            
            //When
            var message = messageBuilder.BuildMessage(request);

            //Then
            Assert.IsNotNull(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildMessage_HierarchyClassesDontExist_ShouldThrowArgumentException()
        {
            //Given
            HierarchyClassEsbServiceRequest request = new HierarchyClassEsbServiceRequest();

            //When
            var message = messageBuilder.BuildMessage(request);
        }
    }
}
