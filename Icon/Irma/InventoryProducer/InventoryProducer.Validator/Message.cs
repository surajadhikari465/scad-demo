using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InventoryProducer.Validator
{
    internal record Message
    {
        public long MessageNumber { get; set; }
        public string RawMessage { get; set; }
        public DateTime InsertDate { get; set; }
        public XmlDocument XMLMessage { get; set; }

        public void LoadXML()
        {
            this.XMLMessage = new XmlDocument();
            this.XMLMessage.LoadXml(this.RawMessage);
        }
    }
}
