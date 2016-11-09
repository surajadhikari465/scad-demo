using Icon.Esb.Subscriber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb
{
    public class Utility
    {
        private static int bomCharacter = 65279;

        /// <summary>
        /// Convert a List to a DataTable.
        /// </summary>
        /// <typeparam name="T">any type.</typeparam>
        /// <param name="data">list of type T."/></param>
        /// <returns>DataTable.</returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// Parses a TIBCO EMS Message for the Text portion of the message, removing the message headers. 
        /// </summary>
        /// <param name="message">TIBCO Message to parse.</param>
        /// <returns>The Text of the message.</returns>
        public static string ParseMessageForXml(string message)
        {
            var indexOfXml = message.IndexOf("Text={") + 6;
            var xml = message.Substring(indexOfXml).TrimEnd(' ', '}');
            return xml;
        }

        public static string RemoveUnusableCharactersFromXml(string xml)
        {
            return xml.Replace("" + (char)bomCharacter, "");
        }

        public static XDocument RemoveUnusableCharactersFromXml(XDocument xml)
        {
            return Utility.RemoveUnusableCharactersFromXml(XDocument.Parse(xml.ToString()));
        }

        public static XElement RemoveUnusableCharactersFromXml(XElement xml)
        {
            return Utility.RemoveUnusableCharactersFromXml(XElement.Parse(xml.ToString()));
        }

        public static int GetMessageHistoryId(IEsbMessage message)
        {
            // TransactionIDs (which are equivalent to Icon's MessageHistoryId) can have # followed by a number in them when the response was from a retry 
            // in the ESB. So we split the TransactionID in order to avoid breaking the parse. 
            // They can also have the source system name followed by an underscore. So we split on the underscore as well.
            var messageHistoryId = message.GetProperty("TransactionID");
            messageHistoryId = messageHistoryId.Split('#')[0];
            if(messageHistoryId.Contains('_'))
                messageHistoryId = messageHistoryId.Split('_')[1];

            return int.Parse(messageHistoryId);
        }
    }
}
