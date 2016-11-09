using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.DataAccess.Models
{
    public class ChangeQueueHistoryModel
    {
        public int HistoryId { get; set; }
        public int EventTypeId { get; set; }
        public string Identifier { get; set; }
        public int Item_Key { get; set; }
        public int? Store_No { get; set; }
        public int QueueID { get; set; }
        public int? EventReferenceId { get; set; }
        public DateTime QueueInsertDate { get; set; }
        public DateTime InsertDate { get; set; }
        public string MachineName { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
