using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.QueueReaders
{
    public class PriceQueueReader : IQueueReader<MessageQueuePrice, Contracts.items>
    {
        private ILogger<PriceQueueReader> logger;
        private IEmailClient emailClient;
        private IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>> getMessageQueueQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>> updateMessageQueueStatusCommandHandler;

        public PriceQueueReader(
            ILogger<PriceQueueReader> logger,
            IEmailClient emailClient,
            IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>> getMessageQueueQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>> updateMessageQueueStatusCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
        }

        public List<MessageQueuePrice> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueuePrice>
            {
                Instance = ControllerType.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters);
        }

        public List<MessageQueuePrice> GroupMessagesForMiniBulk(List<MessageQueuePrice> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
            }

            // Use the first message in the queue to set the conditions for grouping the rest of the messages.
            int currentMessageIndex = 0;
            MessageQueuePrice message = messages[currentMessageIndex++];

            int baseLocaleId = message.LocaleId;

            var groupedItemsById = new HashSet<int>();
            groupedItemsById.Add(message.ItemId);

            var groupedMessages = new List<MessageQueuePrice> { message };

            int miniBulkLimit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitPrice"], out miniBulkLimit))
            {
                miniBulkLimit = 100;
            }

            while (groupedMessages.Count < miniBulkLimit && currentMessageIndex < messages.Count)
            {
                message = messages[currentMessageIndex];

                if (!MessageContainsDifferentLocale(baseLocaleId, message.LocaleId) &&
                    !ItemAlreadyExistsInMiniBulk(groupedItemsById, message.ItemId))
                {
                    groupedMessages.Add(message);
                    groupedItemsById.Add(message.ItemId);
                }

                currentMessageIndex++;
            }

            logger.Info(string.Format("Grouped {0} queued messages to be included in the mini-bulk.", groupedMessages.Count));

            return groupedMessages;
        }

        public Contracts.items BuildMiniBulk(List<MessageQueuePrice> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
            }

            var miniBulk = new Contracts.items();

            // One quirk with Price mini-bulks is that one message from the queue could create up to three mini-bulk entries, but those three entries should still only increment the
            // mini-bulk size by one.  To allow for the maximum case, the item array will initialized to three times the message count.
            miniBulk.item = new Contracts.ItemType[messages.Count * 3];

            int currentMiniBulkIndex = 0;

            foreach (var message in messages)
            {
                if (message.ChangeType == Constants.PriceChangeTypes.CancelAllSales)
                {
                    var miniBulkEntry = CreateDeleteTpr(message);

                    miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
                }
                else if (ShouldCreateExistingTprMessage(message))
                {
                    // This condition represents adding or updating a TPR for an item that has an existing TPR.  Three entries need to be created:
                    //  -> AddOrUpdate Reg price.
                    //  -> Delete TPR.
                    //  -> AddOrUpdate TPR.

                    try
                    {
                        var miniBulkEntries = CreateEntriesForExistingTpr(message);

                        for (int i = 0; i < miniBulkEntries.Length; i++)
                        {
                            miniBulk.item[currentMiniBulkIndex++] = miniBulkEntries[i];
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleMiniBulkException(message, ex);
                    }
                }
                else if (message.SalePrice != null)
                {
                    // This condition represents adding a new TPR for an item that currently doesn't have one.  Two entries need to be created:
                    //  -> AddOrUpdate Reg price.
                    //  -> AddOrUpdate TPR.

                    try
                    {
                        var miniBulkEntries = CreateEntriesForNewTpr(message);

                        for (int i = 0; i < miniBulkEntries.Length; i++)
                        {
                            miniBulk.item[currentMiniBulkIndex++] = miniBulkEntries[i];
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleMiniBulkException(message, ex);
                    }
                }
                else
                {
                    // This condition represents adding or updating the Reg price with no TPR involved at all.  One entry needs to be created:
                    //  -> AddOrUpdate Reg price.

                    try
                    {
                        var miniBulkEntry = CreateEntryForRegPrice(message);
                        miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
                    }
                    catch (Exception ex)
                    {
                        HandleMiniBulkException(message, ex);
                    }
                }
            }

            // Only take elements in the array that were actually assigned data.
            miniBulk.item = miniBulk.item.Where(i => i != null).ToArray();
            return miniBulk;
        }

        private Contracts.ItemType CreateDeleteTpr(MessageQueuePrice message)
        {
            return new Contracts.ItemType
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
                        Action = Contracts.ActionEnum.Delete,
                        ActionSpecified = true,
                        id = message.BusinessUnit_ID.ToString(),
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
                                    id = message.ScanCodeId,
                                    code = message.ScanCode,
                                    typeId = message.ScanCodeTypeId,
                                    typeIdSpecified = true,
                                    typeDescription = message.ScanCodeTypeDesc
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                                {
                                    type = new Contracts.PriceTypeType
                                    {
                                        description = ItemPriceDescriptions.TemporaryPriceReduction,
                                        id =  PriceTypeIdType.TPR
                                    },
                                    uom = new Contracts.UomType
                                    {
                                        codeSpecified = true,
                                        nameSpecified = true,
                                        code = GetUomCode(message.UomCode),
                                        name = GetUomName(message.UomName, message.ScanCode)
                                    },
                                    currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                    priceAmount = new Contracts.PriceAmount
                                    {
                                        amount = message.PreviousSalePrice.Value,
                                        amountSpecified = true
                                    },
                                    priceMultiple = message.PreviousSaleMultiple.HasValue ? message.PreviousSaleMultiple.Value : 1,
                                    priceStartDate = message.PreviousSaleStartDate.HasValue ? message.PreviousSaleStartDate.Value.Date : default(DateTime).Date,
                                    priceStartDateSpecified = message.PreviousSaleStartDate.HasValue,
                                    priceEndDate = message.PreviousSaleEndDate.HasValue ? message.PreviousSaleEndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : default(DateTime).Date,
                                    priceEndDateSpecified = message.PreviousSaleEndDate.HasValue
                                }
                            }
                        }
                    }
                }
            };
        }

        private static bool ShouldCreateExistingTprMessage(MessageQueuePrice message)
        {
            return message.SalePrice != null && message.PreviousSalePrice != null && message.SaleStartDate != message.PreviousSaleStartDate;
        }

        private void HandleMiniBulkException(MessageQueuePrice message, Exception ex)
        {
            logger.Error(string.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
                message.MessageQueueId));

            ExceptionLogger<PriceQueueReader> exceptionLogger = new ExceptionLogger<PriceQueueReader>(logger);
            exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

            FailMessageQueueEntry(message);

            string errorMessage = string.Format(Resource.FailedToAddQueuedMessageToMiniBulkMessage, ControllerType.Type, ControllerType.Instance);
            string emailSubject = Resource.FailedToAddQueuedMessageToMiniBulkEmailSubject;
            string emailBody = EmailHelper.BuildMessageBodyForMiniBulkError(errorMessage, message.MessageQueueId, ex.ToString());

            try
            {
                emailClient.Send(emailBody, emailSubject);
            }
            catch (Exception mailEx)
            {
                string mailErrorMessage = "A failure occurred while attempting to send the alert email.";
                exceptionLogger.LogException(mailErrorMessage, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private void FailMessageQueueEntry(MessageQueuePrice message)
        {
            var command = new UpdateMessageQueueStatusCommand<MessageQueuePrice>
            {
                QueuedMessages = new List<MessageQueuePrice> { message },
                MessageStatusId = MessageStatusTypes.Failed,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);
        }

        private Contracts.ItemType CreateEntryForRegPrice(MessageQueuePrice message)
        {
            return new Contracts.ItemType
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
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = message.BusinessUnit_ID.ToString(),
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
                                    id = message.ScanCodeId,
                                    code = message.ScanCode,
                                    typeId = message.ScanCodeTypeId,
                                    typeIdSpecified = true,
                                    typeDescription = message.ScanCodeTypeDesc
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                                {
                                    type = new Contracts.PriceTypeType
                                    {
                                            description = ItemPriceDescriptions.RegularPrice,
                                            id = PriceTypeIdType.REG
                                    },
                                    uom = new Contracts.UomType
                                    {
                                        codeSpecified = true,
                                        nameSpecified = true,
                                        code = GetUomCode(message.UomCode),
                                        name = GetUomName(message.UomName, message.ScanCode)
                                    },
                                    currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                    priceAmount = new Contracts.PriceAmount
                                    {
                                        amount = message.Price.Value,
                                        amountSpecified = true
                                    },
                                    priceMultiple = message.Multiple.Value,
                                    priceStartDate = new DateTime(1970, 1, 1).Date,
                                    priceStartDateSpecified = true
                                }
                            }
                        }
                    }
                }
            };
        }

        private Contracts.ItemType[] CreateEntriesForNewTpr(MessageQueuePrice message)
        {
            return new Contracts.ItemType[]
            {
                new Contracts.ItemType
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
                            Action = Contracts.ActionEnum.AddOrUpdate,
                            ActionSpecified = true,
                            id = message.BusinessUnit_ID.ToString(),
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
                                        id = message.ScanCodeId,
                                        code = message.ScanCode,
                                        typeId = message.ScanCodeTypeId,
                                        typeIdSpecified = true,
                                        typeDescription = message.ScanCodeTypeDesc
                                    }
                                },
                                prices = new Contracts.PriceType[]
                                {
                                    new Contracts.PriceType
                                    {
                                        type = new Contracts.PriceTypeType
                                        {
                                            description = ItemPriceDescriptions.RegularPrice,
                                            id = PriceTypeIdType.REG
                                        },
                                        uom = new Contracts.UomType
                                        {
                                                    codeSpecified = true,
                                                    nameSpecified = true,
                                                    code = GetUomCode(message.UomCode),
                                                    name = GetUomName(message.UomName, message.ScanCode)
                                        },
                                        currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                        priceAmount = new Contracts.PriceAmount
                                        {
                                            amount = message.Price.Value,
                                            amountSpecified = true
                                        },
                                        priceMultiple = message.Multiple.Value,
                                        priceStartDate = new DateTime(1970, 1, 1).Date,
                                        priceStartDateSpecified = true
                                    }
                                }
                            }
                        }
                    }
                },
                new Contracts.ItemType
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
                            Action = Contracts.ActionEnum.AddOrUpdate,
                            ActionSpecified = true,
                            id = message.BusinessUnit_ID.ToString(),
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
                                        id = message.ScanCodeId,
                                        code = message.ScanCode,
                                        typeId = message.ScanCodeTypeId,
                                        typeIdSpecified = true,
                                        typeDescription = message.ScanCodeTypeDesc
                                    }
                                },
                                prices = new Contracts.PriceType[]
                                {
                                    new Contracts.PriceType
                                    {
                                        type = new Contracts.PriceTypeType
                                        {
                                            description = ItemPriceDescriptions.TemporaryPriceReduction,
                                            id = PriceTypeIdType.TPR
                                        },
                                        uom = new Contracts.UomType
                                        {
                                            codeSpecified = true,
                                            nameSpecified = true,
                                            code = GetUomCode(message.UomCode),
                                            name = GetUomName(message.UomName, message.ScanCode)
                                        },
                                        currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                        priceAmount = new Contracts.PriceAmount
                                        {
                                            amount = message.SalePrice.Value,
                                            amountSpecified = true
                                        },
                                        priceMultiple = message.SaleMultiple.Value,
                                        priceStartDate = message.SaleStartDate.HasValue ? message.SaleStartDate.Value.Date : default(DateTime).Date,
                                        priceStartDateSpecified = message.SaleStartDate.HasValue,
                                        priceEndDate = message.SaleEndDate.HasValue ? message.SaleEndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : default(DateTime).Date,
                                        priceEndDateSpecified = message.SaleEndDate.HasValue
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private Contracts.ItemType[] CreateEntriesForExistingTpr(MessageQueuePrice message)
        {
            var regPriceEntry = new Contracts.ItemType
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
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = message.BusinessUnit_ID.ToString(),
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
                                    id = message.ScanCodeId,
                                    code = message.ScanCode,
                                    typeId = message.ScanCodeTypeId,
                                    typeIdSpecified = true,
                                    typeDescription = message.ScanCodeTypeDesc
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                                {
                                    type = new Contracts.PriceTypeType
                                    {
                                        description = ItemPriceDescriptions.RegularPrice,
                                        id = PriceTypeIdType.REG
                                    },
                                    uom = new Contracts.UomType
                                    {
                                        codeSpecified = true,
                                        nameSpecified = true,
                                        code = GetUomCode(message.UomCode),
                                        name = GetUomName(message.UomName, message.ScanCode)
                                    },
                                    currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                    priceAmount = new Contracts.PriceAmount
                                    {
                                        amount = message.Price.Value,
                                        amountSpecified = true
                                    },
                                    priceMultiple = message.Multiple.Value,
                                    priceStartDate = new DateTime(1970, 1, 1).Date,
                                    priceStartDateSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var tprAddOrUpdateEntry = new Contracts.ItemType
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
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = message.BusinessUnit_ID.ToString(),
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
                                    id = message.ScanCodeId,
                                    code = message.ScanCode,
                                    typeId = message.ScanCodeTypeId,
                                    typeIdSpecified = true,
                                    typeDescription = message.ScanCodeTypeDesc
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                                {
                                    type = new Contracts.PriceTypeType
                                    {
                                        description = ItemPriceDescriptions.TemporaryPriceReduction,
                                        id =  PriceTypeIdType.TPR
                                    },
                                    uom = new Contracts.UomType
                                    {
                                        codeSpecified = true,
                                        nameSpecified = true,
                                        code = GetUomCode(message.UomCode),
                                        name = GetUomName(message.UomName, message.ScanCode)
                                    },
                                    currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                    priceAmount = new Contracts.PriceAmount
                                    {
                                        amount = message.SalePrice.Value,
                                        amountSpecified = true
                                    },
                                    priceMultiple = message.SaleMultiple.Value,
                                    priceStartDate = message.SaleStartDate.HasValue ? message.SaleStartDate.Value.Date : default(DateTime).Date,
                                    priceStartDateSpecified = message.SaleStartDate.HasValue,
                                    priceEndDate = message.SaleEndDate.HasValue ? message.SaleEndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : default(DateTime).Date,
                                    priceEndDateSpecified = message.SaleEndDate.HasValue
                                }
                            }
                        }
                    }
                }
            };

            // Don't send TPR deletes for expired sales.
            if (message.PreviousSaleEndDate.Value < DateTime.Today.Date)
            {
                return new Contracts.ItemType[]
                {
                    regPriceEntry,
                    tprAddOrUpdateEntry
                };
            }
            else
            {
                var tprDeleteEntry = new Contracts.ItemType
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
                            Action = Contracts.ActionEnum.Delete,
                            ActionSpecified = true,
                            id = message.BusinessUnit_ID.ToString(),
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
                                        id = message.ScanCodeId,
                                        code = message.ScanCode,
                                        typeId = message.ScanCodeTypeId,
                                        typeIdSpecified = true,
                                        typeDescription = message.ScanCodeTypeDesc
                                    }
                                },
                                prices = new Contracts.PriceType[]
                                {
                                    new Contracts.PriceType
                                    {
                                        type = new Contracts.PriceTypeType
                                        {
                                            description = ItemPriceDescriptions.TemporaryPriceReduction,
                                            id = PriceTypeIdType.TPR
                                        },
                                        uom = new Contracts.UomType
                                        {
                                            codeSpecified = true,
                                            nameSpecified = true,
                                            code = GetUomCode(message.UomCode),
                                            name = GetUomName(message.UomName, message.ScanCode)
                                        },
                                        currencyTypeCode = GetCurrencyTypeCode(message.CurrencyCode),
                                        priceAmount = new Contracts.PriceAmount
                                        {
                                            amount = message.PreviousSalePrice.Value,
                                            amountSpecified = true
                                        },
                                        priceMultiple = message.SaleMultiple.Value,
                                        priceStartDate = message.PreviousSaleStartDate.HasValue ? message.PreviousSaleStartDate.Value.Date : default(DateTime).Date,
                                        priceStartDateSpecified = message.PreviousSaleStartDate.HasValue,
                                        priceEndDate = message.PreviousSaleEndDate.HasValue ? message.PreviousSaleEndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : default(DateTime).Date,
                                        priceEndDateSpecified = message.PreviousSaleEndDate.HasValue
                                    }
                                }
                            }
                        }
                    }
                };

                return new Contracts.ItemType[]
                {
                    regPriceEntry,
                    tprDeleteEntry,
                    tprAddOrUpdateEntry
                };
            }
        }

        private Contracts.WfmUomCodeEnumType GetUomCode(string uomCode)
        {
            Contracts.WfmUomCodeEnumType uomCodeEnum;
            if (Enum.TryParse<Contracts.WfmUomCodeEnumType>(uomCode, out uomCodeEnum))
            {
                return uomCodeEnum;
            }
            else
            {
                return Contracts.WfmUomCodeEnumType.EA;
            }
        }

        private Contracts.WfmUomDescEnumType GetUomName(string uomName, string scanCode)
        {
            Contracts.WfmUomDescEnumType uomNameEnum;
            if (Enum.TryParse<Contracts.WfmUomDescEnumType>(uomName, out uomNameEnum))
            {
                return uomNameEnum;
            }
            else
            {
                logger.Warn(string.Format("The UOM {0} is not recognized for scan code {1}.  EACH will be sent as the UOM.", uomName, scanCode));
                return Contracts.WfmUomDescEnumType.EACH;
            }
        }

        private Contracts.CurrencyTypeCodeEnum GetCurrencyTypeCode(string currencyTypeCode)
        {
            Contracts.CurrencyTypeCodeEnum currencyTypeCodeEnum;
            if (Enum.TryParse<Contracts.CurrencyTypeCodeEnum>(currencyTypeCode, out currencyTypeCodeEnum))
            {
                return currencyTypeCodeEnum;
            }
            else
            {
                return Contracts.CurrencyTypeCodeEnum.USD;
            }
        }

        private bool ItemAlreadyExistsInMiniBulk(HashSet<int> groupedItemsById, int itemId)
        {
            return groupedItemsById.Contains(itemId);
        }

        private bool MessageContainsDifferentLocale(int baseLocaleId, int localeId)
        {
            return baseLocaleId != localeId;
        }
    }
}
