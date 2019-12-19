using FastMember;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Services.NewItem.Constants;
using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Services.NewItem.Commands
{
    public class AddItemEventsToIconEventQueueCommandHandler : ICommandHandler<AddItemEventsToIconEventQueueCommand>
    {
        private IRenewableContext<IconContext> iconContext;

        public AddItemEventsToIconEventQueueCommandHandler(IRenewableContext<IconContext> iconContext)
        {
            this.iconContext = iconContext;
        }

        public void Execute(AddItemEventsToIconEventQueueCommand data)
        {
            try
            {
                using (ObjectReader reader = ObjectReader.Create(
                    ConvertToEventQueueModel(data.NewItems),
                    nameof(EventQueueModel.QueueId),
                    nameof(EventQueueModel.EventId),
                    nameof(EventQueueModel.EventMessage),
                    nameof(EventQueueModel.EventReferenceId),
                    nameof(EventQueueModel.RegionCode)))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)(iconContext.Context.Database.Connection)))
                    {
                        bulkCopy.DestinationTableName = "app.EventQueue";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
            catch(Exception ex)
            {
                foreach (var item in data.NewItems.Where(i => i.ErrorCode == null))
                {
                    item.ErrorCode = ApplicationErrors.Codes.FailedToAddGloConEventError;
                    item.ErrorDetails = string.Format(ApplicationErrors.Details.FailedToAddGloConEventError, ex.ToString());
                }
                throw;
            }
        }

        private IEnumerable<EventQueueModel> ConvertToEventQueueModel(IEnumerable<NewItemModel> newItems)
        {
            return newItems.Select(m => new EventQueueModel
            {
                EventId = EventTypes.ItemUpdate,
                EventMessage = m.ScanCode,
                EventReferenceId = m.IconItemId.Value,
                RegionCode = m.Region
            });
        }
    }
}
