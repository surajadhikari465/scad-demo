using Esb.Core.EsbServices;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class ArchiveVimMessageCommandHandlerTests
    {
        private ArchiveVimMessageCommandHandler commandHandler;
        private ArchiveVimMessageCommand command;
        private EsbServiceResponse response;
        private Guid messageId;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new ArchiveVimMessageCommandHandler();
            response = new EsbServiceResponse();

            messageId = Guid.NewGuid();
            command = new ArchiveVimMessageCommand { Response = response };
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (IconContext context = new IconContext())
            {
                context.Database.ExecuteSqlCommand(
                    "delete vim.MessageHistory where EsbMessageId = @guid",
                    new SqlParameter("guid", System.Data.SqlDbType.UniqueIdentifier) { Value = messageId });
            }
        }

        [TestMethod]
        public void ArchiveVimMessage_HierarchyClassesExist_ShouldAddArchiveRecordsForHierarchyClass()
        {
            //Given
            messageId = Guid.NewGuid();
            response.Message = new EsbServiceMessage
            {
                MessageId = messageId.ToString(),
                Text = "<test>test</test>"
            };

            //When
            commandHandler.Execute(command);

            //Then
            using (IconContext context = new IconContext())
            {
                var vimMessageHistory = context.Database.SqlQuery<VimMessageHistory>(
                    "select * from vim.MessageHistory where EsbMessageId = @messageId",
                    new SqlParameter("messageId", System.Data.SqlDbType.UniqueIdentifier) { Value = messageId })
                    .First();
                Assert.AreEqual(response.Message.Text, vimMessageHistory.Message);
                Assert.AreEqual(response.Message.MessageId, vimMessageHistory.EsbMessageId.ToString());
                Assert.AreEqual(MessageStatusTypes.Sent, vimMessageHistory.MessageStatusId);
                Assert.AreEqual(MessageTypes.Hierarchy, vimMessageHistory.MessageTypeId);
            }
        }
    }
}
