using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class ArchiveHierarchyClassesCommandHandlerTests
    {
        private ArchiveHierarchyClassesCommandHandler commandHandler;
        private ArchiveHierarchyClassesCommand command;
        private List<HierarchyClassModel> testHierarchyClasses;
        private Guid testInforMessageId;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new ArchiveHierarchyClassesCommandHandler();
            testHierarchyClasses = new List<HierarchyClassModel>();

            testInforMessageId = Guid.NewGuid();
            command = new ArchiveHierarchyClassesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (IconContext context = new IconContext())
            {
                context.MessageArchiveHierarchy.RemoveRange(
                    context.MessageArchiveHierarchy.Where(hc => hc.InforMessageId == testInforMessageId));
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void ArchiveHierarchyClasses_HierarchyClassesExist_ShouldAddArchiveRecordsForHierarchyClass()
        {
            //Given
            testHierarchyClasses.AddRange(new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = 1234, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 1", InforMessageId = testInforMessageId.ToString() },
                new HierarchyClassModel { HierarchyClassId = 12345, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 2", InforMessageId = testInforMessageId.ToString()},
                new HierarchyClassModel { HierarchyClassId = 123456, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 3", InforMessageId = testInforMessageId.ToString() },
                new HierarchyClassModel { HierarchyClassId = 1234567, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 4", InforMessageId = testInforMessageId.ToString() }
            });
            command.Models = testHierarchyClasses;

            //When
            commandHandler.Execute(command);

            //Then
            using (IconContext context = new IconContext())
            {
                var messageArchiveHierarchyRecords = context.MessageArchiveHierarchy.Where(h => h.InforMessageId == testInforMessageId);
                Assert.AreEqual(testHierarchyClasses.Count, messageArchiveHierarchyRecords.Count());
            }
        }
    }
}
