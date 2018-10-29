using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class ArchiveCheckpointMessageCommandParameters
    {
        public Guid MessageId { get; set; }
        public int MessageStatusId { get; set; }
        public string Message { get; set; }
        public string MessageHeadersJson { get; set; }
    }
}
