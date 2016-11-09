using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageQueueHierarchy
    {
        int MessageQueueId { get; set; }
        int MessageTypeId { get; set; }
        int MessageStatusId { get; set; }
        int? MessageHistoryId { get; set; }
        int MessageActionId { get; set; }
        DateTime InsertDate { get; set; }
        int HierarchyId { get; set; }
        string HierarchyName { get; set; }
        string HierarchyLevelName { get; set; }
        bool ItemsAttached { get; set; }
        string HierarchyClassId { get; set; }
        string HierarchyClassName { get; set; }
        int HierarchyLevel { get; set; }
        int? HierarchyParentClassId { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }
}
