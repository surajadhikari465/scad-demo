using System.Collections.Generic;
using AttributePublisher.DataAccess.Models;

namespace AttributePublisher.DataAccess.Commands
{
    public class ArchiveMessagesCommand
    {
        public List<MessageArchiveModel> Messages { get; set; }
    }
}