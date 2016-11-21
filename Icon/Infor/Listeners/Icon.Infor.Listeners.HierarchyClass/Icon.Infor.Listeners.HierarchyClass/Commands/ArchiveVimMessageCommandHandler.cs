using FastMember;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ArchiveVimMessageCommandHandler : ICommandHandler<ArchiveVimMessageCommand>
    {
        public void Execute(ArchiveVimMessageCommand data)
        {
            if (data.Response != null && data.Response.Message != null)
            {
                using (var sqlBulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
                {
                    using (var reader = ObjectReader.Create(new[] { new VimMessageHistory(data.Response) },
                        nameof(VimMessageHistory.MessageHistoryId),
                        nameof(VimMessageHistory.EsbMessageId),
                        nameof(VimMessageHistory.MessageTypeId),
                        nameof(VimMessageHistory.MessageStatusId),
                        nameof(VimMessageHistory.Message)))
                    {
                        sqlBulkCopy.DestinationTableName = "vim.MessageHistory";
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
