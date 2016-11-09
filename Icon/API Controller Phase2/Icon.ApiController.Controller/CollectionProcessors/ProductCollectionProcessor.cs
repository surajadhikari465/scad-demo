using Icon.ApiController.Common;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.CollectionProcessors
{
    public class ProductCollectionProcessor : ICollectionProcessor<List<int>>
    {
        private ILogger<ProductCollectionProcessor> logger;
        private ISerializer<Contracts.items> serializer;
        private IQueryHandler<GetItemsByIdParameters, List<Item>> getItemsQueryHandler;
        private IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass> getFinancialClassByMerchandiseClassQuery;
        private ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private IEsbProducer producer;

        public ProductCollectionProcessor(
            ILogger<ProductCollectionProcessor> logger,
            ISerializer<Contracts.items> serializer,
            IQueryHandler<GetItemsByIdParameters, List<Item>> getItemsQueryHandler,
            IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass> getFinancialClassByMerchandiseClassQuery,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            IEsbProducer producer)
        {
            this.logger = logger;
            this.serializer = serializer;
            this.getItemsQueryHandler = getItemsQueryHandler;
            this.getFinancialClassByMerchandiseClassQuery = getFinancialClassByMerchandiseClassQuery;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.updateMessageHistoryCommandHandler = updateMessageHistoryCommandHandler;
            this.producer = producer;
        }

        public void GenerateMessages(List<int> itemsById)
        {
            logger.Info("Resolving linked item sequencing...");

            var items = getItemsQueryHandler.Search(new GetItemsByIdParameters { ItemsById = itemsById.Distinct().ToList() });

            if (items.Count > 0)
            {
                var miniBulk = BuildMiniBulk(items);

                if (miniBulk.item.Length > 0)
                {
                    string serializedMessage = SerializeMiniBulk(miniBulk);

                    if (!String.IsNullOrEmpty(serializedMessage))
                    {
                        var productMessage = BuildProductMessage(serializedMessage);
                        SaveXmlMessageToMessageHistory(productMessage);
                        bool messageSent = SendMessageToEsb(productMessage);
                        ProcessResponse(messageSent, productMessage);
                    }
                }

                logger.Info(String.Format("Linked item message generation complete.  Number of linked items processed: {0}.", miniBulk.item.Length));
            }
        }

        private void ProcessResponse(bool messageSent, MessageHistory message)
        {
            if (messageSent)
            {
                logger.Info(String.Format("Message {0} has been sent successfully.", message.MessageHistoryId));

                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<MessageHistory>
                {
                    Message = message,
                    MessageStatusId = MessageStatusTypes.Sent
                };

                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);
            }
            else
            {
                logger.Error(String.Format("Message {0} failed to send.  Message will remain in Ready state for re-processing during the next controller execution.", message.MessageHistoryId));
            }
        }

        private bool SendMessageToEsb(MessageHistory message)
        {
            try
            {
                producer.Send(message.Message, new Dictionary<string, string> { { "IconMessageID", message.MessageHistoryId.ToString() } });
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Failed to send message {0}.  Error: {1}", message.MessageHistoryId, ex.ToString()));
                return false;
            }

            return true;
        }

        private void SaveXmlMessageToMessageHistory(MessageHistory message)
        {
            var command = new SaveToMessageHistoryCommand<MessageHistory>
            {
                Message = message
            };

            saveToMessageHistoryCommandHandler.Execute(command);
        }

        private MessageHistory BuildProductMessage(string xml)
        {
            // ESB wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                InsertDate = DateTime.Now,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Product,
                Message = xml,
                InProcessBy = ControllerType.Instance,
                ProcessedDate = null
            };
        }

        private string SerializeMiniBulk(Contracts.items products)
        {
            return serializer.Serialize(products, new Utf8StringWriter());
        }

        private Contracts.items BuildMiniBulk(List<Item> items)
        {
            List<Contracts.ItemType> itemTypeElements = new List<Contracts.ItemType>();

            foreach (var item in items)
            {
                try
                {
                    Contracts.ItemType itemContract = new Contracts.ItemType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = item.itemID,
                        @base = new Contracts.BaseItemType
                        {
                            type = new Contracts.ItemTypeType
                            {
                                code = item.ItemType.itemTypeCode,
                                description = item.ItemType.itemTypeDesc
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
                                }
                            }
                        }
                    };

                    Contracts.EnterpriseItemAttributesType enterpriseAttributes = new Contracts.EnterpriseItemAttributesType();

                    AddScanCode(item, enterpriseAttributes);
                    AddItemHierarchies(item, enterpriseAttributes);
                    AddItemTraits(item, enterpriseAttributes);

                    itemContract.locale[0].Item = enterpriseAttributes;

                    itemTypeElements.Add(itemContract);
                }
                catch (Exception ex)
                {
                    logger.Error(String.Format("An error occurred while attempting to add item {0} to the mini-bulk.  The item will not be included in the message to ESB.",
                        item.itemID));

                    ExceptionLogger<ProductCollectionProcessor> exceptionLogger = new ExceptionLogger<ProductCollectionProcessor>(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                }
            }

            return new Contracts.items { item = itemTypeElements.ToArray() };
        }

        private static void AddScanCode(Item item, Contracts.EnterpriseItemAttributesType enterpriseAttributes)
        {
            enterpriseAttributes.scanCodes = new Contracts.ScanCodeType[]
            {
                new Contracts.ScanCodeType
                {
                    id = item.ScanCode.Single().scanCodeID,
                    code = item.ScanCode.Single().scanCode,
                    typeId = item.ScanCode.Single().scanCodeTypeID,
                    typeIdSpecified = true,
                    typeDescription = item.ScanCode.Single().ScanCodeType.scanCodeTypeDesc
                }
            };
        }

        private void AddItemHierarchies(Item item, Contracts.EnterpriseItemAttributesType enterpriseAttributes)
        {
            List<Contracts.HierarchyType> itemHierarchies = new List<Contracts.HierarchyType>();

            var merchandiseHierarchy = CreateHierarchyType(Hierarchies.Merchandise, HierarchyNames.Merchandise,
                item.ItemHierarchyClass.SingleOrDefault(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Merchandise));

            var brandHierarchy = CreateHierarchyType(Hierarchies.Brands, HierarchyNames.Brands,
                item.ItemHierarchyClass.SingleOrDefault(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Brands));

            // The tax hierarchy portion of the message is populated differently than the other hierarchies.
            var taxHierarchy = CreateTaxHierarchyType(item.ItemHierarchyClass.SingleOrDefault(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Tax));

            // The financial hierarchy portion of the message is populated differently than the other hierarchies.
            var financialHierarchy = CreateFinancialHierarchyType(item.ItemHierarchyClass.SingleOrDefault(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Merchandise));

            if (merchandiseHierarchy != null)
            {
                itemHierarchies.Add(merchandiseHierarchy);
            }

            if (brandHierarchy != null)
            {
                itemHierarchies.Add(brandHierarchy);
            }

            if (taxHierarchy != null)
            {
                itemHierarchies.Add(taxHierarchy);
            }

            if (financialHierarchy != null)
            {
                itemHierarchies.Add(financialHierarchy);
            }

            enterpriseAttributes.hierarchies = itemHierarchies.ToArray();
        }

        private void AddItemTraits(Item item, Contracts.EnterpriseItemAttributesType enterpriseAttributes)
        {
            List<Contracts.TraitType> itemTraits = new List<Contracts.TraitType>();

            var productDescription = CreateTraitType(TraitCodes.ProductDescription, TraitDescriptions.ProductDescription,
                                        item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ProductDescription));

            var posDescription = CreateTraitType(TraitCodes.PosDescription, TraitDescriptions.PosDescription,
                                        item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.PosDescription));

            var packageUnit = CreateTraitType(TraitCodes.PackageUnit, TraitDescriptions.PackageUnit,
                                        item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.PackageUnit));

            var retailSize = CreateTraitType(TraitCodes.RetailSize, TraitDescriptions.RetailSize,
                                        item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.RetailSize));

            var retailUom = CreateTraitType(TraitCodes.RetailUom, TraitDescriptions.RetailUom,
                                        item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.RetailUom));

            var foodStampEligible = CreateTraitType(TraitCodes.FoodStampEligible, TraitDescriptions.FoodStampEligible,
                                        item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.FoodStampEligible));

            if (productDescription != null)
            {
                itemTraits.Add(productDescription);
            }

            if (posDescription != null)
            {
                itemTraits.Add(posDescription);
            }

            if (packageUnit.type.value[0].value != null &&
                retailSize.type.value[0].value != null &&
                retailUom.type.value[0].value != null)
            {
                itemTraits.Add(packageUnit);
                itemTraits.Add(retailSize);
                itemTraits.Add(retailUom);
            }

            if (foodStampEligible != null)
            {
                itemTraits.Add(foodStampEligible);
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
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
                                value = string.Empty
                            }
                        }
                    }
                }
            };

            itemTraits.AddRange(signAttributeTraits);

            enterpriseAttributes.traits = itemTraits.ToArray();
        }

        private Contracts.HierarchyType CreateTaxHierarchyType(ItemHierarchyClass itemHierarchyClass)
        {
            if (itemHierarchyClass == null)
            {
                return null;
            }

            var taxHierarchyClass = itemHierarchyClass.HierarchyClass;

            string taxCode = taxHierarchyClass.hierarchyClassName.Split(' ')[0];

            return new Contracts.HierarchyType
            {
                id = Hierarchies.Tax,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = taxCode,
                        name = taxHierarchyClass.hierarchyClassName,
                        level = taxHierarchyClass.hierarchyLevel.HasValue ? taxHierarchyClass.hierarchyLevel.Value : default(int),
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = taxHierarchyClass.hierarchyParentClassID.HasValue ? taxHierarchyClass.hierarchyParentClassID.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.Tax
            };
        }

        private Contracts.HierarchyType CreateFinancialHierarchyType(ItemHierarchyClass itemHierarchyClass)
        {
            if (itemHierarchyClass == null)
            {
                return null;
            }

            var financialHierarchyClass = getFinancialClassByMerchandiseClassQuery
                .Search(new GetFinancialClassByMerchandiseClassParameters { MerchandiseHierarchyClass = itemHierarchyClass.HierarchyClass });

            if (financialHierarchyClass == null)
            {
                return null;
            }

            string subTeamNumber = financialHierarchyClass.hierarchyClassName.Split('(')[1].Trim(')');

            return new Contracts.HierarchyType
            {
                id = Hierarchies.Financial,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = subTeamNumber,
                        name = subTeamNumber == "0000" ? "na" : subTeamNumber,
                        level = financialHierarchyClass.hierarchyLevel.HasValue ? financialHierarchyClass.hierarchyLevel.Value : default(int),
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = financialHierarchyClass.hierarchyParentClassID.HasValue ? financialHierarchyClass.hierarchyParentClassID.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.Financial
            };
        }

        private Contracts.HierarchyType CreateHierarchyType(int hierarchyId, string hierarchyName, ItemHierarchyClass itemHierarchyClass)
        {
            if (itemHierarchyClass == null)
            {
                return null;
            }
            else
            {
                var hierarchyClass = itemHierarchyClass.HierarchyClass;

                return new Contracts.HierarchyType
                {
                    id = hierarchyId,
                    @class = new Contracts.HierarchyClassType[]
                    {
                        new Contracts.HierarchyClassType
                        {
                            id = hierarchyClass.hierarchyClassID.ToString(),
                            name = hierarchyClass.hierarchyClassName,
                            level = hierarchyClass.hierarchyLevel.HasValue ? hierarchyClass.hierarchyLevel.Value : default(int),
                            parentId = new Contracts.hierarchyParentClassType
                            {
                                Value = hierarchyClass.hierarchyParentClassID.HasValue ? hierarchyClass.hierarchyParentClassID.Value : default(int)
                            }
                        }
                    },
                    name = hierarchyName
                };
            }
        }

        private Contracts.TraitType CreateTraitType(string traitCode, string traitDescription, ItemTrait itemTrait)
        {
            if (itemTrait == null)
            {
                return null;
            }
            else
            {
                return new Contracts.TraitType
                {
                    code = traitCode,
                    type = new Contracts.TraitTypeType
                    {
                        description = traitDescription,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = itemTrait.traitValue
                            }
                        }
                    }
                };
            }
        }
    }
}
