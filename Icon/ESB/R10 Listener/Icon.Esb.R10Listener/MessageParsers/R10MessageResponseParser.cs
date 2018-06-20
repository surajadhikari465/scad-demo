using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.Models;
using Icon.Esb.Subscriber;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Contracts = Icon.Esb.Schemas.Common.ContractTypes;

namespace Icon.Esb.R10Listener.MessageParsers
{
    public class R10MessageResponseParser : IMessageParser<R10MessageResponseModel>
    {
        private const string MessageIdPropertyName = "TransactionID";
        private XmlSerializer serializer;
        private TextReader textReader;

        public R10MessageResponseParser()
        {
            serializer = new XmlSerializer(typeof(Contracts.R10ResponseType));
        }

        public R10MessageResponseModel ParseMessage(IEsbMessage message)
        {
            string messageId;
            try
            {
                messageId = message.GetProperty(MessageIdPropertyName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse Message ID of message.", ex);
            }

            Contracts.R10ResponseType r10Response;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(message.MessageText)))
            {
                r10Response = serializer.Deserialize(textReader) as Contracts.R10ResponseType;
            }

            R10MessageResponseModel messageModel = new R10MessageResponseModel
            {
                MessageId = messageId,
                RequestSuccess = r10Response.requestSuccess,

                // This will encode the xml correctly so that it saves to the database.
                ResponseText = XDocument.Parse(message.MessageText).ToString()
            };

            if (!r10Response.requestSuccess)
            {
                if (r10Response.systemError != null)
                {
                    messageModel.SystemError = true;
                    messageModel.FailureReasonCode = r10Response.systemError.description.reasonCode;
                }
                else
                {
                    messageModel.SystemError = false;

                    var reasonCodes = r10Response.r10Message.r10ResponseReason
                        .Select(r => r.reasonCode)
                        .Distinct();

                    if (reasonCodes.Count() > 1)
                    {
                        messageModel.FailureReasonCode = String.Join(", ", reasonCodes);
                    }
                    else
                    {
                        messageModel.FailureReasonCode = reasonCodes.FirstOrDefault();
                    }

                    GetBusinessErrors(r10Response, messageModel);
                }
            }

            return messageModel;
        }

        private static void GetBusinessErrors(Contracts.R10ResponseType r10Response, R10MessageResponseModel messageModel)
        {
            XElement xmlAscii = null;
            List<BusinessErrorModel> businessErrors = null;

            try
            {
                xmlAscii = XElement.Parse(r10Response.r10Message.xmlAscii);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error occurred while parsing message for element [xmlAscii].  See inner exception for details.", ex);
            }

            if (xmlAscii != null)
            {
                try
                {
                    businessErrors = xmlAscii.Descendants(XName.Get("BusinessError", XmlNamespaceConstants.R10ServicesNamespace))
                        .Select(be => new BusinessErrorModel
                        {
                            Code = be.Element(XName.Get("Code", XmlNamespaceConstants.R10ServicesNamespace)).Value,
                            MainId = be.Descendants(XName.Get("MainId", XmlNamespaceConstants.R10ServicesNamespace)).FirstOrDefault() != null ? Convert.ToInt32(be.Descendants(XName.Get("MainId", XmlNamespaceConstants.R10ServicesNamespace)).FirstOrDefault().Value) : -1
                        }).ToList();

                    messageModel.BusinessErrors = businessErrors;
                }

                catch (Exception ex)
                {
                    throw new ArgumentException("Error occurred while parsing message for [BusinessError] elements.  See inner exception for details.", ex);
                }
            }
        }
    }
}
