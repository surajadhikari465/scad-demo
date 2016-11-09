using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Icon.Esb.ItemMovementListener.Models
{
    [XmlRoot("ItemMovement", Namespace="http://services.wfm.com/Enterprise/ItemMvmt/ItemEntity/V1")]
    public class ItemMovementModel
    {
        public TransactionModel Transaction { get; set; }

        public override string ToString()
        {
             XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
             ns.Add("ns0", "http://services.wfm.com/Enterprise/ItemMvmt/ItemEntity/V1");
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(ItemMovementModel));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);            
            xs.Serialize(xmlTextWriter, this, ns);
            string result = Encoding.UTF8.GetString(memoryStream.ToArray());
            return result;
        }

        public static ItemMovementModel FromString(string xmlString)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ItemMovementModel));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            XmlTextReader xreader = new XmlTextReader(memoryStream);
            ItemMovementModel itemMovementModel = (ItemMovementModel)xs.Deserialize(xreader);
            return itemMovementModel;
        }
    }
}

