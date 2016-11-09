using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.ApiController.QueueReaders
{
    public class MammothPriceQueueReader : IQueueReader<MessageQueuePrice, Contracts.items>
    {
        private ILogger logger;
        private IEmailClient emailClient;
        private IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>> getMessageQueueQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>> updateMessageQueueStatusCommandHandler;
        private ApiControllerSettings settings;

        public MammothPriceQueueReader(ILogger logger,
            IEmailClient emailClient,
            IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>> getMessageQueueQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>> updateMessageQueueStatusCommandHandler,
            ApiControllerSettings settings)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.settings = settings;
        }

        public List<MessageQueuePrice> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueuePrice>
            {
                Instance = settings.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters);
        }

        public List<MessageQueuePrice> GroupMessagesForMiniBulk(List<MessageQueuePrice> messagesReadyForMiniBulk)
        {
            return messagesReadyForMiniBulk
                .Take(settings.MiniBulkLimitPrice)
                .ToList();
        }

        public Contracts.items BuildMiniBulk(List<MessageQueuePrice> messages)
        {
            List<Contracts.ItemType> items = new List<Contracts.ItemType>();
            foreach (var message in messages)
            {
                try
                {
                    items.Add(BuildItemType(message, message.PriceTypeCode,
                        message.Price, message.Multiple, message.StartDate, message.EndDate));
                }
                catch (Exception)
                {
                    logger.Error(String.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
                        message.MessageQueueId));

                    var command = new UpdateMessageQueueStatusCommand<MessageQueuePrice>
                    {
                        QueuedMessages = new List<MessageQueuePrice> { message },
                        MessageStatusId = MessageStatusTypes.Failed,
                        ResetInProcessBy = true
                    };

                    updateMessageQueueStatusCommandHandler.Execute(command);
                }
            }
            return new Contracts.items { item = items.ToArray() };
        }

        private Contracts.ItemType BuildItemType(MessageQueuePrice message,
            string priceTypeCode,
            decimal price,
            int multiple,
            DateTime startDate,
            DateTime? endDate)
        {
            var itemType = new Contracts.ItemType
            {
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
                        Action = GetAction(message.MessageActionId),
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
                            prices = new Contracts.PriceType[]
                            {
                                CreatePriceType(message, priceTypeCode, price, multiple, startDate, endDate)
                            }
                        }
                    }
                }
            };

            return itemType;
        }

        private Contracts.PriceType CreatePriceType(MessageQueuePrice message,
            string priceTypeCode,
            decimal price,
            int multiple,
            DateTime startDate,
            DateTime? endDate)
        {
            var priceType = new Contracts.PriceType
            {
                type = new Contracts.PriceTypeType
                {
                    description = ItemPriceTypes.Descriptions.ByCode[priceTypeCode],
                    id = priceTypeCode,
                    type = string.IsNullOrWhiteSpace(message.SubPriceTypeCode) ? null
                        : new Contracts.PriceTypeType
                            {
                                description = message.SubPriceTypeCode,
                                id = message.SubPriceTypeCode,
                            }
                },
                uom = new Contracts.UomType
                {
                    codeSpecified = true,
                    nameSpecified = true,
                    code = GetEsbUomCode(message.UomCode),
                    name = GetEsbUomName(message.UomCode, message.ScanCode)
                },
                currencyTypeCode = GetEsbCurrencyTypeCode(message.CurrencyCode),
                priceAmount = new Contracts.PriceAmount
                {
                    amount = price,
                    amountSpecified = true
                },
                priceMultiple = multiple,
                priceStartDate = startDate,
                priceStartDateSpecified = true,
            };

            if (endDate.HasValue)
            {
                priceType.priceEndDate = endDate.Value;
                priceType.priceEndDateSpecified = true;
            }

            return priceType;
        }

        private Contracts.ActionEnum GetAction(int messageActionId)
        {
            if (messageActionId == MessageActions.AddOrUpdate)
                return Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.AddOrUpdate;
            else if (messageActionId == MessageActions.Delete)
                return Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete;
            else
                throw new ArgumentException(string.Format("Invalid MessageActionId passed. Value was {0}."));
        }

        private Contracts.WfmUomCodeEnumType GetEsbUomCode(string uomCode)
        {
            switch (uomCode)
            {
                case UomCodes.Each:
                    return Contracts.WfmUomCodeEnumType.EA;
                case UomCodes.Pound:
                    return Contracts.WfmUomCodeEnumType.LB;
                default:
                    return Contracts.WfmUomCodeEnumType.EA;
            }
        }

        private Contracts.WfmUomDescEnumType GetEsbUomName(string uomCode, string scanCode)
        {
            switch (uomCode)
            {
                case UomCodes.Each:
                    return Contracts.WfmUomDescEnumType.EACH;
                case UomCodes.Pound:
                    return Contracts.WfmUomDescEnumType.POUND;
                default:
                    logger.Warn(String.Format("The UOM {0} is not recognized for scan code {1}.  EACH will be sent as the UOM.", uomCode, scanCode));
                    return Contracts.WfmUomDescEnumType.EACH;
            }
        }

        private Contracts.CurrencyTypeCodeEnum GetEsbCurrencyTypeCode(string currencyTypeCode)
        {
            Contracts.CurrencyTypeCodeEnum currencyTypeCodeEnum;
            if (Enum.TryParse(currencyTypeCode, out currencyTypeCodeEnum))
            {
                return currencyTypeCodeEnum;
            }
            else
            {
                return Contracts.CurrencyTypeCodeEnum.USD;
            }
        }
    }
}
