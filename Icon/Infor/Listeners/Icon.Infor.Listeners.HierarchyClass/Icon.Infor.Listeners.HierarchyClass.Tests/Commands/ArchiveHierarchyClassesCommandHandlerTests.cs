using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class ArchiveHierarchyClassesCommandHandlerTests
    {
        private ArchiveHierarchyClassesCommandHandler commandHandler;
        private ArchiveHierarchyClassesCommand command;
        private List<InforHierarchyClassModel> testHierarchyClasses;
        private Guid messageId;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new ArchiveHierarchyClassesCommandHandler();
            testHierarchyClasses = new List<InforHierarchyClassModel>();

            messageId = Guid.NewGuid();
            command = new ArchiveHierarchyClassesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (IconContext context = new IconContext())
            {
                context.MessageArchiveHierarchy.RemoveRange(
                    context.MessageArchiveHierarchy.Where(hc => hc.InforMessageId == messageId));
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void ArchiveHierarchyClasses_HierarchyClassesExist_ShouldAddArchiveRecordsForHierarchyClass()
        {
            //Given
            testHierarchyClasses.AddRange(new List<InforHierarchyClassModel>
            {
                new InforHierarchyClassModel { HierarchyClassId = 1234, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 1", InforMessageId = messageId.ToString() },
                new InforHierarchyClassModel { HierarchyClassId = 12345, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 2", InforMessageId = messageId.ToString()},
                new InforHierarchyClassModel { HierarchyClassId = 123456, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 3", InforMessageId = messageId.ToString(), ErrorCode = "TestErrorCode", },
                new InforHierarchyClassModel { HierarchyClassId = 1234567, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 4", InforMessageId = messageId.ToString() }
            });
            command.Models = testHierarchyClasses;

            //When
            commandHandler.Execute(command);

            //Then
            using (IconContext context = new IconContext())
            {
                var messageArchiveHierarchyRecords = context.MessageArchiveHierarchy.Where(h => h.InforMessageId == messageId).ToList();
                Assert.AreEqual(testHierarchyClasses.Count, messageArchiveHierarchyRecords.Count());

                for (int i = 0; i < testHierarchyClasses.Count; i++)
                {
                    Assert.AreEqual(testHierarchyClasses[i].HierarchyClassId, messageArchiveHierarchyRecords[i].HierarchyClassId);
                    Assert.AreEqual(testHierarchyClasses[i].HierarchyName, messageArchiveHierarchyRecords[i].HierarchyName);
                    Assert.AreEqual(testHierarchyClasses[i].HierarchyClassName, messageArchiveHierarchyRecords[i].HierarchyClassName);
                    Assert.AreEqual(testHierarchyClasses[i].InforMessageId, messageArchiveHierarchyRecords[i].InforMessageId.ToString());
                    Assert.AreEqual(testHierarchyClasses[i].ToJson(), messageArchiveHierarchyRecords[i].Context);
                    Assert.AreEqual(testHierarchyClasses[i].ErrorCode, messageArchiveHierarchyRecords[i].ErrorCode);
                    Assert.AreEqual(testHierarchyClasses[i].ErrorDetails, messageArchiveHierarchyRecords[i].ErrorDetails);
                }
            }
        }
    }
}
