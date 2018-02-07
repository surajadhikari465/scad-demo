using Mammoth.PrimeAffinity.Library.MessageBuilders;
using System.Collections.Generic;

namespace Mammoth.PrimeAffinity.Library.Commands
{
    public class ArchivePrimeAffinityMessageCommand
    {
        public string Message { get; set; }
        public List<PrimeAffinityMessageModel> PrimeAffinityMessageModels { get; set; }
        public int MessageStatusId { get; set; }
        public string MessageId { get; set; }
        public string MessageHeadersJson { get; set; }
    }
}
