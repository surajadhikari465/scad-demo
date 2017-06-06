using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Models
{
    public class InforMessageHistoryModel
    {
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public string Message { get; set; }
        public Guid InforMessageId { get; set; }
    }
}
