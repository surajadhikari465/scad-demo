using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastMember;
using Icon.Framework;
using Icon.Common.Context;
using Infor.Services.NewItem.Models;
using Newtonsoft.Json;

namespace Infor.Services.NewItem.Commands
{
    public class ArchiveNewItemsCommandHandler : ICommandHandler<ArchiveNewItemsCommand>
    {
        private IRenewableContext<IconContext> context;

        public ArchiveNewItemsCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(ArchiveNewItemsCommand data)
        {
            if (data.NewItems != null && data.NewItems.Any())
            {
                using (ObjectReader reader = ObjectReader.Create(ConvertToArchiveModel(data.NewItems),
                    nameof(NewItemArchiveModel.MessageArchiveNewItemId),
                    nameof(NewItemArchiveModel.QueueId),
                    nameof(NewItemArchiveModel.Region),
                    nameof(NewItemArchiveModel.ScanCode),
                    nameof(NewItemArchiveModel.MessageHistoryId),
                    nameof(NewItemArchiveModel.Context),
                    nameof(NewItemArchiveModel.ErrorCode),
                    nameof(NewItemArchiveModel.ErrorDetails)))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)(context.Context.Database.Connection)))
                    {
                        bulkCopy.DestinationTableName = "infor.MessageArchiveNewItem";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }

        private IEnumerable<NewItemArchiveModel> ConvertToArchiveModel(IEnumerable<NewItemModel> newItems)
        {
            return newItems.Select(ni => new NewItemArchiveModel
            {
                QueueId = ni.QueueId,
                Region = ni.Region,
                ScanCode = ni.ScanCode,
                MessageHistoryId = ni.MessageHistoryId,
                Context = JsonConvert.SerializeObject(ni),
                ErrorCode = ni.ErrorCode,
                ErrorDetails = ni.ErrorDetails
            });
        }
    }
}
