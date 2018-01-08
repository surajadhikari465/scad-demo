using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Logging;
using Mammoth.ApiController.Infrastructure;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.ApiController.QueueReaders
{
    public class MammothItemLocaleQueueReader : IQueueReader<MessageQueueItemLocale, Contracts.items>
    {
        private ILogger logger;
        private IEmailClient emailClient;
        private IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>> getMessageQueueQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler;
        private ApiControllerSettings settings;

        public MammothItemLocaleQueueReader(ILogger logger,
            IEmailClient emailClient,
            IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>> getMessageQueueQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler,
            ApiControllerSettings settings)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.settings = settings;
        }

        public List<MessageQueueItemLocale> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueueItemLocale>
            {
                Instance = settings.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters);
        }

        public List<MessageQueueItemLocale> GroupMessagesForMiniBulk(List<MessageQueueItemLocale> messagesReadyForMiniBulk)
        {
            return messagesReadyForMiniBulk
                .Take(settings.MiniBulkLimitItemLocale)
                .ToList();
        }

        public Contracts.items BuildMiniBulk(List<MessageQueueItemLocale> messages)
        {
            List<Contracts.ItemType> items = new List<Contracts.ItemType>();
            foreach (var message in messages)
            {
                try
                {
                    ValidateItemLocaleMessage(message);
                    items.Add(BuildItemType(message));
                }
                catch (Exception)
                {
                    logger.Error(String.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
                        message.MessageQueueId));

                    var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
                    {
                        QueuedMessages = new List<MessageQueueItemLocale> { message },
                        MessageStatusId = MessageStatusTypes.Failed,
                        ResetInProcessBy = true
                    };

                    updateMessageQueueStatusCommandHandler.Execute(command);
                }
            }
            return new Contracts.items { item = items.ToArray() };
        }

        private void ValidateItemLocaleMessage(MessageQueueItemLocale message)
        {
            if (!string.IsNullOrWhiteSpace(message.ScaleExtraText))
                XmlConvert.VerifyXmlChars(message.ScaleExtraText);
        }

        private Contracts.ItemType BuildItemType(MessageQueueItemLocale message)
        {
            return new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = message.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = message.ItemTypeCode,
                        description = message.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = message.BusinessUnitId.ToString(),
                        name = message.LocaleName,
                        type = new Contracts.LocaleTypeType
                        {
                            code = Contracts.LocaleCodeType.STR,
                            description = Contracts.LocaleDescType.Store
                        },
                        Item = new Contracts.StoreItemAttributesType
                        {
                            scanCode = new Contracts.ScanCodeType[]
                            {
                                new Contracts.ScanCodeType
                                {
                                    code = message.ScanCode,
                                }
                            },
                            traits = new Contracts.TraitType[]
                            {
                                CreateTrait(message.CaseDiscount, Attributes.Codes.CaseDiscountEligible),
                                CreateTrait(message.TmDiscount, Attributes.Codes.TmDiscountEligible),
                                CreateTrait(message.AgeRestriction, Attributes.Codes.AgeRestrict),
                                CreateTrait(message.RestrictedHours, Attributes.Codes.RestrictedHours),
                                CreateTrait(message.Authorized, Attributes.Codes.AuthorizedForSale),
                                CreateTrait(message.Discontinued, Attributes.Codes.Discontinued),
                                CreateTrait(message.LabelTypeDescription, Attributes.Codes.LabelTypeDesc),
                                CreateTrait(message.LocalItem, Attributes.Codes.LocalItem),
                                CreateTrait(message.ProductCode, Attributes.Codes.ProductCode),
                                CreateTrait(message.RetailUnit, Attributes.Codes.RetailUnit),
                                CreateTrait(message.SignDescription, Attributes.Codes.SignCaption),
                                CreateTrait(message.Locality, Attributes.Codes.Locality),
                                CreateTrait(message.SignRomanceLong, Attributes.Codes.SignRomanceLong),
                                CreateTrait(message.SignRomanceShort, Attributes.Codes.SignRomanceShort),
                                CreateTrait(message.ColorAdded, Attributes.Codes.ColorAdded),
                                CreateTrait(message.CountryOfProcessing, Attributes.Codes.CountryOfProcessing),
                                CreateTrait(message.Origin, Attributes.Codes.Origin),
                                CreateTrait(message.ElectronicShelfTag, Attributes.Codes.ElectronicShelfTag),
                                CreateTrait(message.Exclusive, Attributes.Codes.Exclusive),
                                CreateTrait(message.NumberOfDigitsSentToScale, Attributes.Codes.NumberOfDigitsSentToScale),
                                CreateTrait(message.ChicagoBaby, Attributes.Codes.ChicagoBaby),
                                CreateTrait(message.TagUom, Attributes.Codes.TagUom),
                                CreateTrait(message.ScaleExtraText, Attributes.Codes.ScaleExtraText),
                                CreateTrait(message.LinkedItem, Attributes.Codes.LinkedScanCode),
                                CreateTrait(message.Msrp, Attributes.Codes.Msrp),
                                CreateTrait(message.SupplierName, Attributes.Codes.VendorName),
                                CreateTrait(message.SupplierItemID, Attributes.Codes.VendorItemId),
                                CreateTrait(message.SupplierCaseSize, Attributes.Codes.VendorCaseSize),
                                CreateTrait(message.IrmaVendorKey, Attributes.Codes.IrmaVendorKey),
                                CreateTrait(message.OrderedByInfor, Attributes.Codes.OrderedByInfor),

                            }
                        }
                    }
                }
            };
        }

        private Contracts.TraitType CreateTrait(decimal? traitValue, string traitCode)
        {
            var stringTraitValue = traitValue?.ToString("0.00");
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(DateTime? traitValue, string traitCode)
        {
            string stringTraitValue = traitValue?.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(bool? traitValue, string traitCode)
        {
            string stringTraitValue = null;
            if (traitValue.HasValue)
            {
                stringTraitValue = traitValue.Value ? "1" : "0";
                return CreateTrait(stringTraitValue, traitCode);
            }
            else
            {
                return CreateTrait(stringTraitValue, traitCode);
            }
        }

        private Contracts.TraitType CreateTrait(int? traitValue, string traitCode)
        {
            var stringTraitValue = traitValue?.ToString();
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(bool traitValue, string traitCode)
        {
            var stringTraitValue = traitValue ? "1" : "0";
            return CreateTrait(stringTraitValue, traitCode);
        }

        private Contracts.TraitType CreateTrait(string traitValue, string traitCode)
        {
            return new Contracts.TraitType
            {
                ActionSpecified = true,
                Action = traitValue == null ? Contracts.ActionEnum.Delete : Contracts.ActionEnum.AddOrUpdate,
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = Attributes.Descriptions.ByCode[traitCode],
                    value = new Contracts.TraitValueType[]
                    {
                        new Contracts.TraitValueType
                        {
                            value = traitValue == null ? string.Empty : traitValue
                        }
                    }
                }
            };
        }
    }
}