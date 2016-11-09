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
    public class EwicMappingSerializer : ISerializer<EwicMappingMessageModel>
    {
        private Utf8StringWriter textWriter;
        private XmlWriterSettings serializerSettings;
        private XmlSerializerNamespaces serializerNamespaces;
        private XmlWriter xmlWriter;
        private XmlSerializer serializer;

        public EwicMappingSerializer()
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

        public string Serialize(EwicMappingMessageModel messageModel)
        {
            var mappingManagementRequest = new Contracts.WicBarcodeMappingManagementRequest
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
                BarcodeMappings = new Contracts.WicBarcodeMappingWithAction[]
                {
                    new Contracts.WicBarcodeMappingWithAction
                    {
                        Action = messageModel.ActionTypeId == MessageActionTypes.AddOrUpdate ? Contracts.ActionTypeAddUpdateDeleteCodes.AddOrUpdate : Contracts.ActionTypeAddUpdateDeleteCodes.Delete,
                        ActionSpecified = true,
                        NationalBarcode = messageModel.AplScanCode,
                        BusinessUnitBarcode = messageModel.WfmScanCode,
                        StateIdentifierCode = messageModel.AgencyId,
                        ProductDescription = messageModel.ProductDescription
                    }
                }
            };

            string mappingMessageXml = SerializeMappingMessage(mappingManagementRequest);

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
                    r10RequestURI = Constants.R10MappingRequestUri,
                    r10Payload = mappingMessageXml,
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

        private string SerializeMappingMessage(Contracts.WicBarcodeMappingManagementRequest mappingMessageContract)
        {
            textWriter = new Utf8StringWriter();
            xmlWriter = XmlWriter.Create(textWriter, serializerSettings);
            serializer = new XmlSerializer(typeof(Contracts.WicBarcodeMappingManagementRequest));

            serializer.Serialize(xmlWriter, mappingMessageContract, serializerNamespaces);

            string xml = textWriter.ToString();

            return xml;
        }
    }
}
