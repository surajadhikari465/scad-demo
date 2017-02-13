using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Models
{
    public class NewItemArchiveModel
    {
        public int MessageArchiveNewItemId { get; set; }
        public int QueueId { get; set; }
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int? MessageHistoryId { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}