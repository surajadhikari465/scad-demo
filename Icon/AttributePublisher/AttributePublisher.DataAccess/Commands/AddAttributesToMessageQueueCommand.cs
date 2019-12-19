using AttributePublisher.DataAccess.Models;
using System.Collections.Generic;

namespace AttributePublisher.DataAccess.Commands
{
    public class AddAttributesToMessageQueueCommand
    {
        public List<AttributeModel> Attributes { get; set; }
    }
}