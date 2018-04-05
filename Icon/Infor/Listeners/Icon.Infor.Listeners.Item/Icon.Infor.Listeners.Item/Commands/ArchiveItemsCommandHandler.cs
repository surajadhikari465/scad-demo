using FastMember;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.Item.Commands
{
    public class ArchiveItemsCommandHandler : ICommandHandler<ArchiveItemsCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public ArchiveItemsCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Execute(ArchiveItemsCommand data)
        {
            using (var context = contextFactory.CreateContext())
            {
                using (var sqlBulkCopy = new SqlBulkCopy(context.Database.Connection as SqlConnection))
                {
                    context.Database.Connection.Open();
                    using (var reader = ObjectReader.Create(data.Models.Select(m => new MessageArchiveProductModel(m)),
                        nameof(MessageArchiveProductModel.MessageArchiveId),
                        nameof(MessageArchiveProductModel.ItemId),
                        nameof(MessageArchiveProductModel.ScanCode),
                        nameof(MessageArchiveProductModel.InforMessageId),
                        nameof(MessageArchiveProductModel.Context),
                        nameof(MessageArchiveProductModel.ErrorCode),
                        nameof(MessageArchiveProductModel.ErrorDetails)))
                    {
                        sqlBulkCopy.DestinationTableName = "infor.MessageArchiveProduct";
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}