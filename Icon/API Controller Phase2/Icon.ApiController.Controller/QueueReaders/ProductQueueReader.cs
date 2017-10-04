using Icon.ApiController.Common;
using Icon.ApiController.Controller.Extensions;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
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
    public class ProductQueueReader : IQueueReader<MessageQueueProduct, Contracts.items>
    {
        private ILogger<ProductQueueReader> logger;
        private IEmailClient emailClient;
        private IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>> getMessageQueueQuery;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>> updateMessageQueueStatusCommandHandler;
        private IProductSelectionGroupsMapper productSelectionGroupMapper;
        private IUomMapper uomMapper;
        
        public ProductQueueReader(
            ILogger<ProductQueueReader> logger,
            IEmailClient emailClient,
            IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>> getMessageQueueQuery,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>> updateMessageQueueStatusCommandHandler,
            IProductSelectionGroupsMapper productSelectionGroupMapper,
            IUomMapper uomMapper)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.getMessageQueueQuery = getMessageQueueQuery;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.productSelectionGroupMapper = productSelectionGroupMapper;
            this.uomMapper = uomMapper;
            this.productSelectionGroupMapper.LoadProductSelectionGroups();
        }

        public List<MessageQueueProduct> GetQueuedMessages()
        {
            var parameters = new GetMessageQueueParameters<MessageQueueProduct>
            {
                Instance = ControllerType.Instance,
                MessageQueueStatusId = MessageStatusTypes.Ready
            };

            return getMessageQueueQuery.Search(parameters);
        }

        public List<MessageQueueProduct> GroupMessagesForMiniBulk(List<MessageQueueProduct> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
            }

            // Use the first message in the queue to set the conditions for grouping the rest of the messages.
            int currentMessageIndex = 0;
            MessageQueueProduct message = messages[currentMessageIndex++];

            string baseDepartmentSale = message.DepartmentSale;
            string baseItemType = message.ItemTypeCode;

            var groupedItemsById = new HashSet<int>();
            groupedItemsById.Add(message.ItemId);

            var groupedMessages = new List<MessageQueueProduct> { message };

            int miniBulkLimit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitProduct"], out miniBulkLimit))
            {
                miniBulkLimit = 100;
            }

            while (groupedMessages.Count < miniBulkLimit && currentMessageIndex < messages.Count)
            {
                message = messages[currentMessageIndex];

                if (!ItemAlreadyExistsInMiniBulk(groupedItemsById, message.ItemId) &&
                    !ItemHasDifferentDepartmentSaleValue(baseDepartmentSale, message.DepartmentSale)&&
                    !ItemHasRetailNonRetailMismatch(baseItemType, message.ItemTypeCode))
                {
                    groupedMessages.Add(message);
                    groupedItemsById.Add(message.ItemId);
                }

                currentMessageIndex++;
            }

            logger.Info(string.Format("Grouped {0} queued messages to be included in the mini-bulk.  Mini-bulk type: {1}",
                groupedMessages.Count, groupedMessages[0].DepartmentSale == "1" ? "DepartmentSale" : "Product"));

            return groupedMessages;
        }

        public Contracts.items BuildMiniBulk(List<MessageQueueProduct> messages)
        {
            if (messages == null || messages.Count == 0)
            {
                throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
            }

            var miniBulk = new Contracts.items();
            miniBulk.item = new Contracts.ItemType[messages.Count];

            if (messages[0].DepartmentSale == "1")
            {
                return BuildDepartmentSaleMiniBulk(miniBulk, messages);
            }
            else
            {
                return BuildProductMiniBulk(miniBulk, messages);
            }
        }

        private Contracts.items BuildDepartmentSaleMiniBulk(Contracts.items miniBulk, List<MessageQueueProduct> messages)
        {
            int currentMiniBulkIndex = 0;

            foreach (var message in messages)
            {
                try
                {
                    var miniBulkEntry = new Contracts.ItemType
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
                                id = Locales.WholeFoods.ToString(),
                                name = "Whole Foods Market",
                                type = new Contracts.LocaleTypeType
                                {
                                    code = Contracts.LocaleCodeType.CHN,
                                    description = Contracts.LocaleDescType.Chain
                                },
                                Item = new Contracts.EnterpriseItemAttributesType
                                {
                                    scanCodes = new Contracts.ScanCodeType[]
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
                                    hierarchies = new Contracts.HierarchyType[]
                                    {
                                        new Contracts.HierarchyType
                                        {
                                            id = Hierarchies.Merchandise,                                            
                                            @class = new Contracts.HierarchyClassType[]
                                            {
                                                new Contracts.HierarchyClassType
                                                {
                                                    id = message.MerchandiseClassId.ToString(),
                                                    name = message.MerchandiseClassName,
                                                    level = message.MerchandiseLevel,
                                                    parentId = new Contracts.hierarchyParentClassType
                                                    {
                                                        Value = message.MerchandiseParentId.HasValue ? message.MerchandiseParentId.Value : default(int)
                                                    }
                                                }
                                            },
                                            name = HierarchyNames.Merchandise
                                        },
                                        new Contracts.HierarchyType
                                        {
                                            id = Hierarchies.Tax,
                                            @class = new Contracts.HierarchyClassType[]
                                            {
                                                new Contracts.HierarchyClassType
                                                {
                                                    id = message.TaxClassName.Split(' ')[0],
                                                    name = message.TaxClassName,
                                                    level = message.TaxLevel,
                                                    parentId = new Contracts.hierarchyParentClassType
                                                    {
                                                        Value = message.TaxParentId.HasValue ? message.TaxParentId.Value : default(int)
                                                    }
                                                }
                                            },
                                            name = HierarchyNames.Tax
                                        },
                                    },
                                    traits = new Contracts.TraitType[]
                                    {
                                        new Contracts.TraitType
                                        {
                                            code = TraitCodes.DepartmentSale,
                                            type = new Contracts.TraitTypeType
                                            {
                                                description = TraitDescriptions.DepartmentSale,
                                                value = new Contracts.TraitValueType[]
                                                {
                                                    new Contracts.TraitValueType
                                                    {
                                                        value = message.FinancialClassId
                                                    }
                                                }
                                            }
                                        }
                                    }, 
                                }
                            }
                        }
                    };

                    miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
                }
                catch (Exception ex)
                {
                    HandleMiniBulkException(message, ex);
                }
            }

            // The mini-bulk was defined at the start with the maximum allowable size. If any messages failed to be included in the mini-bulk,
            // there will be null elements in the array that were never assigned or initialized.  Exclude those elements so that only valid data is returned.
            miniBulk.item = miniBulk.item.Where(i => i != null).ToArray();
            return miniBulk;
        }

        private Contracts.items BuildProductMiniBulk(Contracts.items miniBulk, List<MessageQueueProduct> messages)
        {
            int currentMiniBulkIndex = 0;

            foreach (var message in messages)
            {
                try
                {
                    var productSelectionGroups = productSelectionGroupMapper.GetProductSelectionGroups(message);
                    var miniBulkEntry = new Contracts.ItemType
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
                            },
                            consumerInformation = BuildConsumerInformation(message)
                        },
                        locale = new Contracts.LocaleType[]
                        {
                            new Contracts.LocaleType
                            {
                                id = Locales.WholeFoods.ToString(),
                                name = "Whole Foods Market",
                                type = new Contracts.LocaleTypeType
                                {
                                    code = Contracts.LocaleCodeType.CHN,
                                    description = Contracts.LocaleDescType.Chain
                                },
                                Item = new Contracts.EnterpriseItemAttributesType
                                {
                                    scanCodes = new Contracts.ScanCodeType[]
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
                                    hierarchies = new Contracts.HierarchyType[]
                                    {
                                        new Contracts.HierarchyType
                                        {
                                            id = Hierarchies.Merchandise,
                                            @class = new Contracts.HierarchyClassType[]
                                            {
                                                new Contracts.HierarchyClassType
                                                {
                                                    id = message.MerchandiseClassId.ToString(),
                                                    name = message.MerchandiseClassName,
                                                    level = message.MerchandiseLevel,
                                                    parentId = new Contracts.hierarchyParentClassType
                                                    {
                                                        Value = message.MerchandiseParentId.HasValue ? message.MerchandiseParentId.Value : default(int)
                                                    }
                                                }
                                            },
                                            name = HierarchyNames.Merchandise
                                        },
                                        new Contracts.HierarchyType
                                        {
                                            id = Hierarchies.Brands,
                                            @class = new Contracts.HierarchyClassType[]
                                            {
                                                new Contracts.HierarchyClassType
                                                {
                                                    id = message.BrandId.ToString(),
                                                    name = message.BrandName,
                                                    level = message.BrandLevel,
                                                    parentId = new Contracts.hierarchyParentClassType
                                                    {
                                                        Value = message.BrandParentId.HasValue ? message.BrandParentId.Value : default(int)
                                                    }
                                                }
                                            },
                                            name = HierarchyNames.Brands
                                        },
                                        new Contracts.HierarchyType
                                        {
                                            id = Hierarchies.Tax,
                                            @class = new Contracts.HierarchyClassType[]
                                            {
                                                new Contracts.HierarchyClassType
                                                {
                                                    id = message.TaxClassName.Split(' ')[0],
                                                    name = message.TaxClassName,
                                                    level = message.TaxLevel,
                                                    parentId = new Contracts.hierarchyParentClassType
                                                    {
                                                        Value = message.TaxParentId.HasValue ? message.TaxParentId.Value : default(int)
                                                    }
                                                }
                                            },
                                            name = HierarchyNames.Tax
                                        },
                                        new Contracts.HierarchyType
                                        {
                                            id = Hierarchies.Financial,
                                            @class = new Contracts.HierarchyClassType[]
                                            {
                                                new Contracts.HierarchyClassType
                                                {
                                                    id = message.FinancialClassId,
                                                    name = message.FinancialClassName,
                                                    level = message.FinancialLevel,
                                                    parentId = new Contracts.hierarchyParentClassType
                                                    {
                                                        Value = message.FinancialParentId.HasValue ? message.FinancialParentId.Value : default(int)
                                                    }
                                                }
                                            },
                                            name = HierarchyNames.Financial
                                        }
                                    },
                                    traits = BuildItemTraits(message),
                                    selectionGroups = productSelectionGroups
                                }
                            }
                        }
                    };

                    miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
                }
                catch (Exception ex)
                {
                    HandleMiniBulkException(message, ex);
                }
            }

            // The mini-bulk was defined at the start with the maximum allowable size. If any messages failed to be included in the mini-bulk,
            // there will be null elements in the array that were never assigned or initialized.  Exclude those elements so that only valid data is returned.
            miniBulk.item = miniBulk.item.Where(i => i != null).ToArray();
            return miniBulk;
        }

        private void HandleMiniBulkException(MessageQueueProduct message, Exception ex)
        {
            logger.Error(string.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
                message.MessageQueueId));

            ExceptionLogger<ProductQueueReader> exceptionLogger = new ExceptionLogger<ProductQueueReader>(logger);
            exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

            var command = new UpdateMessageQueueStatusCommand<MessageQueueProduct>
            {
                QueuedMessages = new List<MessageQueueProduct> { message },
                MessageStatusId = MessageStatusTypes.Failed,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);

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

        private Contracts.TraitType[] BuildItemTraits(MessageQueueProduct message)
        {
            var itemTraits = new List<Contracts.TraitType>
            {
                new Contracts.TraitType
                {
                    code = TraitCodes.ProductDescription,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ProductDescription,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ProductDescription
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PosDescription,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PosDescription,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.PosDescription
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.FoodStampEligible,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.FoodStampEligible,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.FoodStampEligible
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.ProhibitDiscount,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ProhibitDiscount,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ProhibitDiscount ? "1" : "0"
                            }
                        }
                    }
                }
            };
                        
            if (ShouldSendPhysicalCharacteristicTraits(message))
            {
                var physicalCharacteristicTraits = new List<Contracts.TraitType>
                {
                    new Contracts.TraitType
                    {
                        code = TraitCodes.PackageUnit,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.PackageUnit,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.PackageUnit
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.RetailSize,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.RetailSize,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.RetailSize == null ? string.Empty : message.RetailSize.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.RetailUom,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.RetailUom,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.RetailUom == null ? string.Empty : message.RetailUom,
                                    uom = new Contracts.UomType
                                    {
                                        code = uomMapper.GetEsbUomCode(message.RetailUom),
                                        codeSpecified = true,
                                        name = uomMapper.GetEsbUomDescription(message.RetailUom),
                                        nameSpecified = true
                                    }
                                }
                            }
                        }
                    }
                };

                itemTraits.AddRange(physicalCharacteristicTraits);
            }

            var signAttributeTraits = new List<Contracts.TraitType>
                {
                    new Contracts.TraitType
                    {
                        code = TraitCodes.AnimalWelfareRating,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.AnimalWelfareRating,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.AnimalWelfareRating == null ? string.Empty : message.AnimalWelfareRating
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.Biodynamic,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.Biodynamic,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.Biodynamic == null ? string.Empty : message.Biodynamic.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.CheeseMilkType,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.CheeseMilkType,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.CheeseMilkType == null ? string.Empty : message.CheeseMilkType
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.CheeseRaw,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.CheeseRaw,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.CheeseRaw == null ? string.Empty : message.CheeseRaw.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.EcoScaleRating,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.EcoScaleRating,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.EcoScaleRating == null ? string.Empty : message.EcoScaleRating
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.GlutenFree,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.GlutenFree,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.GlutenFreeAgency == null ? string.Empty : message.GlutenFreeAgency
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.HealthyEatingRating,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.HealthyEatingRating,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.HealthyEatingRating == null ? string.Empty : message.HealthyEatingRating
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.Kosher,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.Kosher,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.KosherAgency == null ? string.Empty : message.KosherAgency
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.Msc,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.Msc,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.Msc == null ? string.Empty : message.Msc.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.NonGmo,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.NonGmo,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.NonGmoAgency == null ? string.Empty : message.NonGmoAgency
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.Organic,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.Organic,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.OrganicAgency == null ? string.Empty : message.OrganicAgency
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.PremiumBodyCare,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.PremiumBodyCare,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.PremiumBodyCare == null ? string.Empty : message.PremiumBodyCare.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.FreshOrFrozen,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.FreshOrFrozen,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.SeafoodFreshOrFrozen == null ? string.Empty : message.SeafoodFreshOrFrozen
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.SeafoodCatchType,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.SeafoodCatchType,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.SeafoodCatchType == null ? string.Empty : message.SeafoodCatchType
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.Vegan,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.Vegan,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.VeganAgency == null ? string.Empty : message.VeganAgency
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.Vegetarian,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.Vegetarian,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.Vegetarian == null ? string.Empty : message.Vegetarian.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.WholeTrade,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.WholeTrade,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.WholeTrade == null ? string.Empty : message.WholeTrade.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.GrassFed,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.GrassFed,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.GrassFed == null ? string.Empty : message.GrassFed.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.PastureRaised,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.PastureRaised,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.PastureRaised == null ? string.Empty : message.PastureRaised.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.FreeRange,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.FreeRange,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.FreeRange == null ? string.Empty : message.FreeRange.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.DryAged,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.DryAged,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.DryAged == null ? string.Empty : message.DryAged.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.AirChilled,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.AirChilled,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.AirChilled == null ? string.Empty : message.AirChilled.ToString()
                                }
                            }
                        }
                    },
                    new Contracts.TraitType
                    {
                        code = TraitCodes.MadeInHouse,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.MadeInHouse,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.MadeInHouse == null ? string.Empty : message.MadeInHouse.ToString()
                                }
                            }
                        }
                    },
                 new Contracts.TraitType
                    {
                        code = TraitCodes.CustomerFriendlyDescription,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.CustomerFriendlyDescription,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.CustomerFriendlyDescription == null ? string.Empty : message.CustomerFriendlyDescription.ToString()
                                }
                            }
                        }
                    },
                   new Contracts.TraitType
                    {
                        code = TraitCodes.NutritionRequired,
                        type = new Contracts.TraitTypeType
                        {
                            description = TraitDescriptions.NutritionRequired,
                            value = new Contracts.TraitValueType[]
                            {
                                new Contracts.TraitValueType
                                {
                                    value = message.NutritionRequired == null ? string.Empty : message.NutritionRequired.ToString()
                                }
                            }
                        }
                    }
                };

            itemTraits.AddRange(signAttributeTraits);

            var nutritionTraits = BuildNutritionTraits(message);

            if (nutritionTraits != null)
            {
                itemTraits.AddRange(nutritionTraits);
            }

            return itemTraits.ToArray();
        }

        private static bool ShouldSendPhysicalCharacteristicTraits(MessageQueueProduct message)
        {
            // These three traits must all have values for them to be included in the message.
            // R10 will reject products with a 0 PackageUnit so we should not send products with a 0 package unit
            return (!string.IsNullOrWhiteSpace(message.PackageUnit) && message.PackageUnit != "0") 
                && !string.IsNullOrWhiteSpace(message.RetailSize) 
                && !string.IsNullOrWhiteSpace(message.RetailUom);
        }

        private bool ItemAlreadyExistsInMiniBulk(HashSet<int> groupedItemsById, int itemId)
        {
            return groupedItemsById.Contains(itemId);
        }

        private bool ItemHasDifferentDepartmentSaleValue(string baseDepartmentSale, string departmentSale)
        {
            return baseDepartmentSale != departmentSale;
        }

        private bool ItemHasRetailNonRetailMismatch(string baseItemType, string itemType)
        {
            if (baseItemType.Equals(ItemTypeCodes.NonRetail))
            {
                return baseItemType != itemType;
            }
            else
            {
                return itemType == ItemTypeCodes.NonRetail;
            }
            
        }

        private List<Contracts.TraitType> BuildNutritionTraits(MessageQueueProduct messageQueuProduct)
        {
            MessageQueueNutrition message = messageQueuProduct.MessageQueueNutrition.FirstOrDefault();

            if (message == null)
            {
                return null;
            }

            var nutritionTraits = new List<Contracts.TraitType>
            {
                new Contracts.TraitType
                {
                    code = TraitCodes.RecipeName,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.RecipeName,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.RecipeName == null ? string.Empty : message.RecipeName
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Allergens,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Allergens,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Allergens == null ? string.Empty : message.Allergens
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Ingredients,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Ingredients,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Ingredients == null ? string.Empty : message.Ingredients
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Hsh,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Hsh,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.HshRating == null ? string.Empty : message.HshRating.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PolyunsaturatedFat,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PolyunsaturatedFat,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.PolyunsaturatedFat == null ? string.Empty : message.PolyunsaturatedFat.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.MonounsaturatedFat,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.MonounsaturatedFat,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.MonounsaturatedFat  == null ? string.Empty : message.MonounsaturatedFat.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PotassiumWeight,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PotassiumWeight,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.PotassiumWeight == null ? string.Empty : message.PotassiumWeight.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PotassiumPercent,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PotassiumPercent,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.PotassiumPercent == null ? string.Empty : message.PotassiumPercent.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.DietaryFiberPercent,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.DietaryFiberPercent,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.DietaryFiberPercent == null ? string.Empty : message.DietaryFiberPercent.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.SolubleFiber,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.SolubleFiber,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.SolubleFiber == null ? string.Empty : message.SolubleFiber.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.InsolubleFiber,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.InsolubleFiber,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.InsolubleFiber == null ? string.Empty : message.InsolubleFiber.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.SugarAlcohol,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.SugarAlcohol,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.SugarAlcohol == null ? string.Empty : message.SugarAlcohol.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.OtherCarbohydrates,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.OtherCarbohydrates,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.OtherCarbohydrates == null ? string.Empty : message.OtherCarbohydrates.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.ProteinPercent,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ProteinPercent,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ProteinPercent == null ? string.Empty : message.ProteinPercent.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Betacarotene,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Betacarotene,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Betacarotene == null ? string.Empty : message.Betacarotene.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.VitaminD,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.VitaminD,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.VitaminD == null ? string.Empty : message.VitaminD.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.VitaminE,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.VitaminE,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.VitaminE == null ? string.Empty : message.VitaminE.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Thiamin,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Thiamin,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Thiamin == null ? string.Empty : message.Thiamin.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Riboflavin,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Riboflavin,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Riboflavin == null ? string.Empty : message.Riboflavin.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Niacin,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Niacin,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Niacin == null ? string.Empty : message.Niacin.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.VitaminB6,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.VitaminB6,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.VitaminB6 == null ? string.Empty : message.VitaminB6.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Folate,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Folate,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Folate == null ? string.Empty : message.Folate.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.VitaminB12,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.VitaminB12,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.VitaminB12 == null ? string.Empty : message.VitaminB12.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Biotin,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Biotin,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Biotin == null ? string.Empty : message.Biotin.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PantothenicAcid,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PantothenicAcid,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.PantothenicAcid == null ? string.Empty : message.PantothenicAcid.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Phosphorous,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Phosphorous,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Phosphorous == null ? string.Empty : message.Phosphorous.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Iodine,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Iodine,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Iodine == null ? string.Empty : message.Iodine.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Magnesium,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Magnesium,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Magnesium == null ? string.Empty : message.Magnesium.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Zinc,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Zinc,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Zinc == null ? string.Empty : message.Zinc.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Copper,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Copper,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Copper == null ? string.Empty : message.Copper.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Transfat,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Transfat,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Transfat == null ? string.Empty : message.Transfat.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Om6Fatty,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Om6Fatty,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Om6Fatty == null ? string.Empty : message.Om6Fatty.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Om3Fatty,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Om3Fatty,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Om3Fatty == null ? string.Empty : message.Om3Fatty.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Starch,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Starch,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Starch == null ? string.Empty : message.Starch.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Chloride,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Chloride,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Chloride == null ? string.Empty : message.Chloride.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Chromium,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Chromium,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Chromium == null ? string.Empty : message.Chromium.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.VitaminK,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.VitaminK,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.VitaminK == null ? string.Empty : message.VitaminK.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Manganese,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Manganese,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Manganese == null ? string.Empty : message.Manganese.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Molybdenum,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Molybdenum,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Molybdenum == null ? string.Empty : message.Molybdenum.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.Selenium,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Selenium,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.Selenium == null ? string.Empty : message.Selenium.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.TransfatWeight,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.TransfatWeight,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.TransfatWeight == null ? string.Empty : message.TransfatWeight.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.CaloriesFromTransFat,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.CaloriesFromTransFat,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.CaloriesFromTransfat == null ? string.Empty : message.CaloriesFromTransfat.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.CaloriesSaturatedFat,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.CaloriesSaturatedFat,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.CaloriesSaturatedFat == null ? string.Empty : message.CaloriesSaturatedFat.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.ServingPerContainer,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ServingPerContainer,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ServingPerContainer == null ? string.Empty : message.ServingPerContainer
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.ServingSizeDesc,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ServingSizeDesc,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ServingSizeDesc == null ? string.Empty : message.ServingSizeDesc
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.ServingsPerPortion,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ServingsPerPortion,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ServingsPerPortion == null ? string.Empty : message.ServingsPerPortion.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.ServingUnits,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ServingUnits,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.ServingUnits == null ? string.Empty : message.ServingUnits.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.SizeWeight,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.SizeWeight,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = message.SizeWeight == null ? string.Empty : message.SizeWeight.ToString()
                            }
                        }
                    }
                }
            };

            return nutritionTraits;
        }

        private Contracts.ConsumerInformationType BuildConsumerInformation(MessageQueueProduct messageQueuProduct)
        {
            MessageQueueNutrition message = messageQueuProduct.MessageQueueNutrition.FirstOrDefault();

            if (message == null)
            {
                return null;
            }

            var consumerInformation = new Contracts.ConsumerInformationType
            {
                stockItemConsumerProductLabel =  new Contracts.StockProductLabelType
                {
                    consumerLabelTypeCode = null,
                    servingSizeUom = null,
                    servingSizeUomCount = Decimal.Zero,
                    servingsInRetailSaleUnitCount = null,
                    caloriesCount = message.Calories.ToDecimal(),
                    caloriesFromFatCount = message.CaloriesFat.ToDecimal(),
                    totalFatGramsAmount = message.TotalFatWeight.ToDecimal(),
                    totalFatDailyIntakePercent = message.TotalFatPercentage.ToDecimal(),
                    saturatedFatGramsAmount = message.SaturatedFatWeight.ToDecimal(),
                    saturatedFatPercent = message.SaturatedFatPercent.ToDecimal(),
                    cholesterolMilligramsCount = message.CholesterolWeight.ToDecimal(),
                    cholesterolPercent = message.CholesterolPercent.ToDecimal(),
                    sodiumMilligramsCount = message.SodiumWeight.ToDecimal(),
                    sodiumPercent = message.SodiumPercent.ToDecimal(),
                    totalCarbohydrateMilligramsCount = message.TotalCarbohydrateWeight.ToDecimal(),
                    totalCarbohydratePercent = message.TotalCarbohydratePercent.ToDecimal(),
                    dietaryFiberGramsCount = message.DietaryFiberWeight.ToDecimal(),
                    sugarsGramsCount = message.Sugar.ToDecimal(),
                    proteinGramsCount = message.ProteinWeight.ToDecimal(),
                    vitaminADailyMinimumPercent = message.VitaminA.ToDecimal(),
                    vitaminBDailyMinimumPercent = Decimal.Zero,
                    vitaminCDailyMinimumPercent = message.VitaminC.ToDecimal(),
                    calciumDailyMinimumPercent = message.Calcium.ToDecimal(),
                    ironDailyMinimumPercent = message.Iron.ToDecimal(),
                    nutritionalDescriptionText = null,
                    isHazardousMaterial = message.HazardousMaterialFlag == null ? false : (message.HazardousMaterialFlag.Value == 1 ? true : false),
                    hazardousMaterialTypeCode = null
                }
            };

            return consumerInformation;
        }
    }
}
