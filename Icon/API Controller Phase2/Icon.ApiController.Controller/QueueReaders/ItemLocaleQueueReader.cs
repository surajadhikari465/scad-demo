using Icon.ApiController.Common;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Email;
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
    public class ItemLocaleQueueReader : IQueueReader<MessageQueueItemLocale, Contracts.items>
    {
        private ILogger<ItemLocaleQueueReader> logger;
        private IEmailClient emailClient;
        private IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>> getMessageQueueQuery;
        private IQueryHandler<GetItemByScanCodeParameters, Item> getItemByScanCodeQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler;
        private IProductSelectionGroupsMapper traitToProductSelectionGroupMapper;
        private bool sendPreviousItemLinkGroup;

        public ItemLocaleQueueReader(
            ILogger<ItemLocaleQueueReader> logger,
            IEmailClient emailClient,
            IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>> getMessageQueueQuery,
            IQueryHandler<GetItemByScanCodeParameters, Item> getItemByScanCodeQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler,
            IProductSelectionGroupsMapper traitToProductSelectionGroupMapper)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.getItemByScanCodeQuery = getItemByScanCodeQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.traitToProductSelectionGroupMapper = traitToProductSelectionGroupMapper;
            this.traitToProductSelectionGroupMapper.LoadProductSelectionGroups();
            this.sendPreviousItemLinkGroup = AppSettingsAccessor.GetBoolSetting("SendPreviousItemLinkGroup", false);
        }

        public List<MessageQueueItemLocale> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters);
        }

        public List<MessageQueueItemLocale> GroupMessagesForMiniBulk(List<MessageQueueItemLocale> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
            }

            // Use the first message in the queue to set the conditions for grouping the rest of the messages.
            int currentMessageIndex = 0;
            MessageQueueItemLocale message = messages[currentMessageIndex++];

            int baseLocaleId = message.LocaleId;
            string baseItemTypeCode = message.ItemTypeCode;
            string baseLinkedItem = message.LinkedItemScanCode;

            var groupedItemsById = new HashSet<int>();
            groupedItemsById.Add(message.ItemId);

            var groupedMessages = new List<MessageQueueItemLocale> { message };

            int miniBulkLimit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitItemLocale"], out miniBulkLimit))
            {
                miniBulkLimit = 100;
            }

            while (groupedMessages.Count < miniBulkLimit && currentMessageIndex < messages.Count)
            {
                message = messages[currentMessageIndex];

                if (!MessageContainsDifferentLocale(baseLocaleId, message.LocaleId) &&
                    !ItemAlreadyExistsInMiniBulk(groupedItemsById, message.ItemId) &&
                    !ItemHasDifferentItemType(baseItemTypeCode, message.ItemTypeCode) &&
                    !LinkedItemPresenceDoesNotMatchBaseMessage(baseLinkedItem, message.LinkedItemScanCode))
                {
                    groupedMessages.Add(message);
                    groupedItemsById.Add(message.ItemId);
                }

                currentMessageIndex++;
            }

            logger.Info(String.Format("Grouped {0} queued messages to be included in the mini-bulk.  Mini-bulk type: {1}",
                groupedMessages.Count, groupedMessages[0].LinkedItemScanCode == null ? "ItemLocale" : "LinkedItem"));

            return groupedMessages;
        }

        public Contracts.items BuildMiniBulk(List<MessageQueueItemLocale> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
            }

            var miniBulk = new Contracts.items();
            miniBulk.item = new Contracts.ItemType[messages.Count];

            int currentMiniBulkIndex = 0;
            foreach (var message in messages)
            {
                try
                {
                    var miniBulkEntry = new Contracts.ItemType
                    {
                        Action = GetMessageAction(message.MessageActionId),
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
                                Action = GetMessageAction(message.MessageActionId),
                                ActionSpecified = true,
                                id = message.BusinessUnit_ID.ToString(),
                                name = message.LocaleName,
                                traits = new Contracts.TraitType[]
                                {
                                    new Contracts.TraitType
                                    {
                                        code = TraitCodes.PsBusinessUnitId,
                                        type = new Contracts.TraitTypeType
                                        {
                                            description = TraitDescriptions.PsBusinessUnitId,
                                            value = new Contracts.TraitValueType[]
                                            {
                                                new Contracts.TraitValueType
                                                {
                                                    value = message.BusinessUnit_ID.ToString()
                                                }
                                            }
                                        }
                                    }
                                },
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
                                    traits = new Contracts.TraitType[]
                                    {
                                        new Contracts.TraitType
                                        {
                                            ActionSpecified = true,
                                            Action = message.LockedForSale ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                                            code = TraitCodes.LockedForSale,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.LockedForSale,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.LockedForSale ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            code = TraitCodes.Recall,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.Recall,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.Recall ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            code = TraitCodes.SoldByWeight,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.SoldByWeight,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.Sold_By_Weight ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            ActionSpecified = true,
                                            Action = message.Quantity_Required ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                                            code = TraitCodes.QuantityRequired,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.QuantityRequired,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.Quantity_Required ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            ActionSpecified = true,
                                            Action = message.Price_Required ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                                            code = TraitCodes.PriceRequired,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.PriceRequired,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.Price_Required ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            ActionSpecified = true,
                                            Action = message.QtyProhibit ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                                            code = TraitCodes.QuantityProhibit,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.QuantityProhibit,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.QtyProhibit ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            ActionSpecified = true,
                                            Action = message.VisualVerify ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                                            code = TraitCodes.VisualVerify,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.VisualVerify,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.VisualVerify ? "1" : "0"
                                                    }
                                                }
                                            }
                                        },
                                        new Contracts.TraitType
                                        {
                                            code = TraitCodes.PosScaleTare,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.PosScaleTare,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.PosScaleTare.HasValue ? (message.PosScaleTare.Value * .01).ToString() : "0"
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    selectionGroups = traitToProductSelectionGroupMapper.GetProductSelectionGroups(message)
                                }
                            }
                        }
                    };

                    ProcessLinkedItem(message, miniBulkEntry);

                    miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
                }
                catch (Exception ex)
                {
                    logger.Error(String.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
                        message.MessageQueueId));

                    ExceptionLogger<ItemLocaleQueueReader> exceptionLogger = new ExceptionLogger<ItemLocaleQueueReader>(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                    var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
                    {
                        QueuedMessages = new List<MessageQueueItemLocale> { message },
                        MessageStatusId = MessageStatusTypes.Failed,
                        ResetInProcessBy = true
                    };

                    updateMessageQueueStatusCommandHandler.Execute(command);

                    string errorMessage = String.Format(Resource.FailedToAddQueuedMessageToMiniBulkMessage, ControllerType.Type, ControllerType.Instance);
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
            }

            // The mini-bulk was defined at the start with the maximum allowable size. If any messages failed to be included in the mini-bulk,
            // there will be null elements in the array that were never assigned or initialized.  Exclude those elements so that only valid data is returned.
            miniBulk.item = miniBulk.item.Where(i => i != null).ToArray();

            logger.Info(String.Format("Gathered a mini-bulk of count: {0}", miniBulk.item.Length));

            return miniBulk;
        }

        private void ProcessLinkedItem(MessageQueueItemLocale message, Contracts.ItemType miniBulkEntry)
        {
            if (!String.IsNullOrEmpty(message.LinkedItemScanCode) || !String.IsNullOrEmpty(message.PreviousLinkedItemScanCode))
            {
                var links = new List<Contracts.LinkTypeType>();
                var groups = new List<Contracts.GroupTypeType>();

                if (!String.IsNullOrEmpty(message.LinkedItemScanCode))
                {
                    AddLinkedItem(message, message.LinkedItemScanCode, links, groups, Contracts.ActionEnum.AddOrUpdate);
                }

                if (sendPreviousItemLinkGroup && !String.IsNullOrWhiteSpace(message.PreviousLinkedItemScanCode) && message.LinkedItemScanCode != message.PreviousLinkedItemScanCode)
                {
                    AddLinkedItem(message, message.PreviousLinkedItemScanCode, links, groups, Contracts.ActionEnum.Delete);
                }

                (miniBulkEntry.locale[0].Item as Contracts.StoreItemAttributesType).links = links.Any() ? links.ToArray() : null;
                (miniBulkEntry.locale[0].Item as Contracts.StoreItemAttributesType).groups = groups.Any() ? groups.ToArray() : null;
            }
        }

        private void AddLinkedItem(MessageQueueItemLocale message, string linkedItemScanCode, List<Contracts.LinkTypeType> links, List<Contracts.GroupTypeType> groups, Contracts.ActionEnum action)
        {
            Item linkedItem;
            if (!Cache.scanCodeToItem.TryGetValue(linkedItemScanCode, out linkedItem))
            {
                linkedItem = getItemByScanCodeQuery.Search(new GetItemByScanCodeParameters { ScanCode = linkedItemScanCode });
                Cache.scanCodeToItem.Add(linkedItemScanCode, linkedItem);
            }

            string itemTypeCode = linkedItem.ItemType.itemTypeCode;
            string groupTypeDescription = null;

            if (itemTypeCode == ItemTypeCodes.Deposit)
            {
                groupTypeDescription = Contracts.RetailTransactionItemTypeEnum.Deposit.ToString();
            }
            else if (itemTypeCode == ItemTypeCodes.Fee)
            {
                groupTypeDescription = Contracts.RetailTransactionItemTypeEnum.Warranty.ToString();
            }
            else
            {
                logger.Warn(String.Format("Attempted to process linked item {0} for MessageQueueId {1} / ScanCode {2}, but it does not appear to be a bottle deposit, CRV, or Blackhawk item.",
                    linkedItemScanCode, message.MessageQueueId, message.ScanCode));
                return;
            }

            links.Add(new Contracts.LinkTypeType
                        {
                            parentIdSpecified = true,
                            childIdSpecified = true,
                            parentId = linkedItem.itemID,
                            childId = message.ItemId
                        });
            groups.Add(new Contracts.GroupTypeType
                        {
                            ActionSpecified = true,
                            Action = action,
                            id = linkedItem.itemID.ToString() + "_" + message.ItemId.ToString(),
                            description = groupTypeDescription
                        });
        }

        private Contracts.ActionEnum GetMessageAction(int messageActionId)
        {
            switch (messageActionId)
            {
                case MessageActionTypes.AddOrUpdate:
                    return Contracts.ActionEnum.AddOrUpdate;

                case MessageActionTypes.Delete:
                    return Contracts.ActionEnum.Delete;

                default:
                    throw new ArgumentException(String.Format("Invalid message action {0}:  Provided messageActionId does not match any available actions in the schema.", messageActionId));
            }
        }

        private bool LinkedItemPresenceDoesNotMatchBaseMessage(string baseLinkedItem, string linkedItem)
        {
            if (String.IsNullOrEmpty(baseLinkedItem))
            {
                bool neitherMessageHasLinkedItem = (String.IsNullOrEmpty(baseLinkedItem) && String.IsNullOrEmpty(linkedItem));
                return !neitherMessageHasLinkedItem;
            }
            else
            {
                bool bothMessagesHaveLinkedItem = (!String.IsNullOrEmpty(baseLinkedItem) && !String.IsNullOrEmpty(linkedItem));
                return !bothMessagesHaveLinkedItem;
            }
        }

        private bool MessageContainsDifferentLocale(int baseLocaleId, int localeId)
        {
            return baseLocaleId != localeId;
        }

        private bool ItemHasDifferentItemType(string baseItemTypeCode, string itemTypeCode)
        {
            return baseItemTypeCode != itemTypeCode;
        }

        private bool ItemAlreadyExistsInMiniBulk(HashSet<int> groupedItemsById, int itemId)
        {
            return groupedItemsById.Contains(itemId);
        }
    }
}
