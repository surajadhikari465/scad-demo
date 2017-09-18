using Esb.Core.Constants;
using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Contracts = Icon.Esb.Schemas.Infor.ContractTypes;

namespace Icon.Esb.Services.ConfirmationBod
{
    public class ConfirmationBodMessageBuilder : IMessageBuilder<ConfirmationBodEsbRequest>
    {
        private const string ConfirmBodVersion = "9.2";
        private const string ConfirmBodIdVerb = "?BOD&Verb=Confirm";
        private const string DateFormat = "O";
        private ISerializer<Contracts.ConfirmBODType> serializer;
        private UTF8Encoding encoding;

        public ConfirmationBodMessageBuilder(ISerializer<Contracts.ConfirmBODType> serializer)
        {
            this.serializer = serializer;
            this.encoding = new UTF8Encoding();
        }

        public string BuildMessage(ConfirmationBodEsbRequest request)
        {
            Contracts.ConfirmBODType confirmBODType = new Contracts.ConfirmBODType
            {
                ApplicationArea = new Contracts.ApplicationAreaType
                {
                    CreationDateTime = DateTime.UtcNow.ToString(DateFormat),
                    BODID = new Contracts.MetaIdentifierType { Value = request.BodId + ConfirmBodIdVerb }
                },
                versionID = ConfirmBodVersion,
                DataArea = new Contracts.ConfirmBODDataAreaType
                {
                    Confirm = new Contracts.ConfirmType
                    {
                        TenantID = new Contracts.MetaIdentifierType { Value = request.TenantId }
                    },
                    BOD = new Contracts.BODType[]
                    {
                        new Contracts.BODType
                        {
                            BODFailureMessage = new Contracts.BODFailureMessageType
                            {
                                ErrorProcessMessage = new Contracts.ErrorProcessMessageType[]
                                {
                                    new Contracts.ErrorProcessMessageType
                                    {
                                        Description = new Contracts.DescriptionType[]
                                        {
                                            new Contracts.DescriptionType
                                            {
                                                Value = request.ErrorDescription
                                            }
                                        },
                                        ReasonCode = new Contracts.ReasonCodeType
                                        {
                                            Value = request.ErrorReasonCode
                                        },
                                        Type = new Contracts.TypeType { Value = request.ErrorType.ToString() }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var serializedMessage = serializer.Serialize(confirmBODType, new Utf8StringWriter());

            //Parsing the message back into XML so that we can inject the CDATA section without it being
            //XML encoded.
            var message = XDocument.Parse(serializedMessage);
            message.Descendants(XName.Get("BOD", NamespaceConstants.XmlNamespaces.InforSchema))
                .First()
                .Add(new XElement(XName.Get("OriginalBOD", NamespaceConstants.XmlNamespaces.InforSchema),
                        new XElement(XName.Get("MessageContent", NamespaceConstants.XmlNamespaces.InforSchema),
                            new XCData(Convert.ToBase64String(encoding.GetBytes(request.OriginalMessage))))));
            return message.ToString();
        }
    }
}
