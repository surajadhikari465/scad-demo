using Microsoft.XmlDiffPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Validator
{
    internal record TestInstance
    {
        public MessageArchiveEvent OldArchivedEvent { get; set; }
        public MessageArchiveEvent NewArchivedEvent { get; set; }
        public Event CreatedEvent { get; set; }
        public Message? OldMessage { get; set; }
        public Message? NewMessage { get; set; }
        public XmlDiff MessageDiff { get; set; }
        public bool? AreEqual { get; set; }
        public bool Successful { get; set; }
        public string DiffFileName { get; set; }
    }
}
