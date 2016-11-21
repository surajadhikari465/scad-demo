using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class ArchiveVimHierarchyClassesCommandHandlerTests
    {
        private ArchiveVimHierarchyClassesCommandHandler commandHandler;
        private ArchiveVimHierarchyClassesCommand command;
        private List<VimHierarchyClassModel> testHierarchyClasses;
        private Guid messageId;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new ArchiveVimHierarchyClassesCommandHandler();
            testHierarchyClasses = new List<VimHierarchyClassModel>();

            messageId = Guid.NewGuid();
            command = new ArchiveVimHierarchyClassesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (IconContext context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(
                    "delete vim.MessageArchiveHierarchy where EsbMessageId = @guid",
                    new SqlParameter("guid", System.Data.SqlDbType.UniqueIdentifier) { Value = messageId });
            }
        }

        [TestMethod]
        public void ArchiveVimHierarchyClasses_HierarchyClassesExist_ShouldAddArchiveRecordsForHierarchyClass()
        {
            //Given
            testHierarchyClasses.AddRange(new List<VimHierarchyClassModel>
            {
                new VimHierarchyClassModel { HierarchyClassId = 1234, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 1", MessageId = messageId.ToString() },
                new VimHierarchyClassModel { HierarchyClassId = 12345, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 2", MessageId = messageId.ToString()},
                new VimHierarchyClassModel { HierarchyClassId = 123456, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 3", MessageId = messageId.ToString(), ErrorCode = "TestErrorCode", ErrorDetails = "TestErrorDetails" },
                new VimHierarchyClassModel { HierarchyClassId = 1234567, HierarchyName = Hierarchies.Names.Brands, HierarchyClassName = "Test 4", MessageId = messageId.ToString() }
            });
            command.HierarchyClasses = testHierarchyClasses;

            //When
            commandHandler.Execute(command);

            //Then
            using (IconContext context = new IconContext())
            {
                var messageArchiveHierarchyRecords = context.Database.SqlQuery<VimMessageArchiveHierarchy>(
                    "select * from vim.MessageArchiveHierarchy where EsbMessageId = @messageId",
                    new SqlParameter("messageId", System.Data.SqlDbType.UniqueIdentifier) { Value = messageId })
                    .ToList();
                Assert.AreEqual(testHierarchyClasses.Count, messageArchiveHierarchyRecords.Count());

                for (int i = 0; i < testHierarchyClasses.Count; i++)
                {
                    Assert.AreEqual(testHierarchyClasses[i].HierarchyClassId, messageArchiveHierarchyRecords[i].HierarchyClassId);
                    Assert.AreEqual(testHierarchyClasses[i].HierarchyName, messageArchiveHierarchyRecords[i].HierarchyName);
                    Assert.AreEqual(testHierarchyClasses[i].HierarchyClassName, messageArchiveHierarchyRecords[i].HierarchyClassName);
                    Assert.AreEqual(testHierarchyClasses[i].MessageId, messageArchiveHierarchyRecords[i].EsbMessageId.ToString());
                    Assert.AreEqual(testHierarchyClasses[i].ToJson(), messageArchiveHierarchyRecords[i].Context);
                    Assert.AreEqual(testHierarchyClasses[i].ErrorCode, messageArchiveHierarchyRecords[i].ErrorCode);
                    Assert.AreEqual(testHierarchyClasses[i].ErrorDetails, messageArchiveHierarchyRecords[i].ErrorDetails);
                }
            }
        }
    }
}
