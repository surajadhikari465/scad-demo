using Icon.Common.Xml;
using Icon.Ewic.Models;
using Icon.Framework;
using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Contracts = Icon.Esb.Schemas.Ewic.ContractTypes;

namespace Icon.Ewic.Serialization.Serializers
{
    public class EwicExclusionSerializer : ISerializer<EwicExclusionMessageModel>
    {
        private Utf8StringWriter textWriter;
        private XmlWriterSettings serializerSettings;
        private XmlSerializerNamespaces serializerNamespaces;
        private XmlWriter xmlWriter;
        private XmlSerializer serializer;

        public EwicExclusionSerializer()
        {
            serializerSettings = new XmlWriterSettings();

            // These settings prevent things like tab and newline characters from appearing in the serialized string.
            serializerSettings.NewLineHandling = NewLineHandling.None;
            serializerSettings.Indent = false;

            // UTF-8 is the desired format for ESB.
            serializerSettings.Encoding = Encoding.UTF8;

            serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add("r10", "http://retalix.com/R10/services");
        }

        public string Serialize(EwicExclusionMessageModel messageModel)
        {
            var exclusionManagementRequest = new Contracts.WicProductExclusionManagementRequest
            {
                Header = new Contracts.RetalixCommonHeaderType
                {
                    MessageId = new Contracts.RequestIDCommonData
                    {
                        Timestamp = DateTime.Now,
                        TimestampSpecified = true,
                        Name = Constants.MessageIdHeader,
                        Value = messageModel.MessageHistoryId.ToString()
                    }
                },
                ProductExclusions = new Contracts.WicProductExclusionWithAction[] 
                { 
                    new Contracts.WicProductExclusionWithAction
                    {
                        Action = messageModel.ActionTypeId == MessageActionTypes.AddOrUpdate ? Contracts.ActionTypeAddUpdateDeleteCodes.AddOrUpdate : Contracts.ActionTypeAddUpdateDeleteCodes.Delete,
                        ActionSpecified = true,
                        NationalBarcode = messageModel.ScanCode.PadLeft(15, '0'),
                        StateIdentifierCode = messageModel.AgencyId,
                        ProductDescription = messageModel.ProductDescription
                    }
                }
            };

            string exclusionMessageXml = SerializeExclusionMessage(exclusionManagementRequest);

            var r10Request = new Contracts.R10RequestType
            {
                applicationName = Constants.Application,
                notification = new Contracts.NotificationType
                {
                    toEmail = "madelyn.lescalleet@wholefoods.com,kyle.milner@wholefoods.com",
                    ccEmail = "kiran.mokkapati@wholefoods.com"
                },
                requestDetails = new Contracts.RequestDetailsType
                {
                    r10RequestURI = Constants.R10ExclusionRequestUri,
                    r10Payload = exclusionMessageXml,
                    isPayloadEncrypted = Contracts.IsEncrypted.N
                }
            };

            string r10RequestMessageXml = SerializeR10Request(r10Request);

            return r10RequestMessageXml;
        }

        private string SerializeR10Request(Contracts.R10RequestType r10Request)
        {
            textWriter = new Utf8StringWriter();
            xmlWriter = XmlWriter.Create(textWriter, serializerSettings);
            serializer = new XmlSerializer(typeof(Contracts.R10RequestType));

            serializer.Serialize(xmlWriter, r10Request, serializerNamespaces);

            string xml = textWriter.ToString();

            return xml;
        }

        private string SerializeExclusionMessage(Contracts.WicProductExclusionManagementRequest exclusionMessageContract)
        {
            textWriter = new Utf8StringWriter();
            xmlWriter = XmlWriter.Create(textWriter, serializerSettings);
            serializer = new XmlSerializer(typeof(Contracts.WicProductExclusionManagementRequest));

            serializer.Serialize(xmlWriter, exclusionMessageContract, serializerNamespaces);

            string xml = textWriter.ToString();

            return xml;
        }
    }
}
