using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageQueueProductSelectionGroup
    {
        int MessageQueueId { get; set; }
        DateTime InsertDate { get; set; }
        int MessageTypeId { get; set; }
        int MessageStatusId { get; set; }
        int? MessageHistoryId { get; set; }
        int MessageActionId { get; set; }
        int ProductSelectionGroupId { get; set; }
        string ProductSelectionGroupName { get; set; }
        int ProductSelectionGroupTypeId { get; set; }
        string ProductSelectionGroupTypeName { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }
}
