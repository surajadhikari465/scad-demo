using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Esb.EwicErrorResponseListener.Common.Models;
using Icon.Esb.Subscriber;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Contracts = Icon.Esb.Schemas.Common.ContractTypes;

namespace Icon.Esb.EwicErrorResponseListener.MessageParsers
{
    public class ErrorResponseMessageParser : IMessageParser<EwicErrorResponseModel>
    {
        private XmlSerializer serializer;
        private TextReader textReader;

        public ErrorResponseMessageParser()
        {
            serializer = new XmlSerializer(typeof(Contracts.R10ResponseType));
        }

        public EwicErrorResponseModel ParseMessage(IEsbMessage message)
        {
            int messageHistoryId = GetMessageHistoryId(message);

            Contracts.R10ResponseType r10Response = DeserializeResponse(message.MessageText);

            if (r10Response == null)
            {
                throw new InvalidOperationException("The message deserialized to a null object.  The EwicErrorResponseModel cannot be built.");
            }
            else
            {
                return BuildErrorResponseModel(message.MessageText, messageHistoryId, r10Response);
            }
        }

        private EwicErrorResponseModel BuildErrorResponseModel(string messageText, int messageHistoryId, Contracts.R10ResponseType r10Response)
        {
            string responseReason = r10Response.r10Message.r10ResponseReason[0].messageDescription;

            return new EwicErrorResponseModel
            {
                MessageHistoryId = messageHistoryId,
                RequestSuccess = r10Response.requestSuccess,
                SystemError = !r10Response.requestSuccess,
                ResponseReason = responseReason,

                // This will encode the xml correctly so that it saves to the database.
                ResponseText = XDocument.Parse(messageText).ToString()
            };
        }

        private Contracts.R10ResponseType DeserializeResponse(string messageText)
        {
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(messageText)))
            {
                return serializer.Deserialize(textReader) as Contracts.R10ResponseType;
            }
        }

        private int GetMessageHistoryId(IEsbMessage message)
        {
            try
            {
                return Int32.Parse(message.GetProperty("TransactionID"));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse TransactionID of message.", ex);
            }
        }
    }
}
