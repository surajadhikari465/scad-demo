using Icon.Framework;
using System;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests
{
    public static class TestHelpers
    {
        public static MessageQueueProduct GetFakeMessageQueueProduct(int messageStatusId, int id, string departmentSale, string retailSaleFlag)
        {
            return new MessageQueueProduct
            {
                MessageTypeId = MessageTypes.Product,
                MessageStatusId = messageStatusId,
                MessageHistoryId = null,
                ItemId = id,
                LocaleId = 1,
                ItemTypeCode = retailSaleFlag,
                ItemTypeDesc = "Retail Sale",
                ScanCodeId = 123,
                ScanCode = "123",
                ScanCodeTypeId = 1,
                ScanCodeTypeDesc = "Test",
                ProductDescription = "Test Product Description",
                PosDescription = "TEST POS DESC",
                PackageUnit = "1",
                RetailSize = "1",
                RetailUom = "EA",
                FoodStampEligible = "0",
                BrandId = 1,
                BrandName = "Test Brand",
                BrandLevel = 1,
                BrandParentId = null,
                BrowsingClassId = 1,
                BrowsingClassName = "Test Browsing",
                BrowsingLevel = 1,
                BrowsingParentId = null,
                MerchandiseClassId = 1,
                MerchandiseClassName = "Test Merchandise",
                MerchandiseLevel = 1,
                MerchandiseParentId = 2,
                ProhibitDiscount = true,
                TaxClassId = 1,
                TaxClassName = "0000000 Test Tax",
                TaxLevel = 1,
                TaxParentId = null,
                FinancialClassId = "1",
                FinancialClassName = "Test Financial",
                FinancialLevel = 1,
                FinancialParentId = null,
                DepartmentSale = departmentSale,
                InsertDate = DateTime.Now,
                InProcessBy = null,
                ProcessedDate = null
            };
        }

        public static MessageQueueProduct GetFakeMessageQueueProductWithNutritionalData(int messageStatusId, int id, string departmentSale, string retailSaleFlag)
        {
            var productMessage = TestHelpers.GetFakeMessageQueueProduct(messageStatusId, id, departmentSale, retailSaleFlag);
            productMessage.MessageQueueNutrition.Add(
                                    new MessageQueueNutrition() 
                                    {
                                        RecipeName = "Test recipe Name",
                                        Calcium = 20,
                                        Allergens = "Test Allergens",
                                        Biotin =  15,
                                        HshRating = 5,
                                        ServingSizeDesc = "8 oz",
                                        ServingPerContainer = "5",
                                        Calories = 200,
                                        CaloriesFat = 100
                                    }
            );
            return productMessage;
        }

        public static Contracts.items GetFakeProductMiniBulk()
        {
            var miniBulk = new Contracts.items();
            miniBulk.item = new Contracts.ItemType[100];
            miniBulk.item[0] = new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = 1234,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = "RTL",
                        description = "Retail Sale"
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = "1",
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
                                    id = 123,
                                    code = "9999988888",
                                    typeId = 1,
                                    typeIdSpecified = true,
                                    typeDescription = "UPC"
                                }                                    
                            },
                            hierarchies = new Contracts.HierarchyType[]
                            {
                                new Contracts.HierarchyType
                                {
                                    id = 1,
                                    @class = new Contracts.HierarchyClassType[]
                                    {
                                        new Contracts.HierarchyClassType
                                        {
                                            id = "4444",
                                            name = "Muffins",
                                            level = 4,
                                            parentId = new Contracts.hierarchyParentClassType
                                            {
                                                Value = 333
                                            }
                                        }
                                    },
                                    name = "Merchandise"
                                }
                            },
                            traits = new Contracts.TraitType[]
                            {
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.ProductDescription,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = "Product Description",
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "Chocolate Muffin"
                                            }
                                        }
                                    }
                                }
                            }
                        }                       
                    }
                }
            };

            return miniBulk;
        }

        public static Contracts.items GetFakeDepartmentSaleMiniBulk()
        {
            var miniBulk = new Contracts.items();
            miniBulk.item = new Contracts.ItemType[100];
            miniBulk.item[0] = new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = 1234,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = "RTL",
                        description = "Retail Sale"
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = "1",
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
                                    id = 123,
                                    code = "9999988888",
                                    typeId = 1,
                                    typeIdSpecified = true,
                                    typeDescription = "UPC"
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
                                            id = "4444",
                                            name = "Muffins",
                                            level = 4,
                                            parentId = new Contracts.hierarchyParentClassType
                                            {
                                                Value = 333
                                            }
                                        }
                                    },
                                    name = HierarchyNames.Merchandise
                                }
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
                                                value = "1100"
                                            }
                                        }
                                    }
                                }
                            }
                        }                       
                    }
                }
            };

            return miniBulk;
        }

        public static Contracts.items GetFakeItemLocaleMiniBulk(bool includeLinkedItem, string itemTypeCode, int itemId=1)
        {
            var miniBulk = new Contracts.items();
            miniBulk.item = new Contracts.ItemType[1];

            miniBulk.item[0] = new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = itemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = itemTypeCode,
                        description = itemTypeCode
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = "300",
                        name = "Lamar",
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
                                    id = 123456,
                                    code = "44444444444",
                                    typeId = 1,
                                    typeIdSpecified = true,
                                    typeDescription = "UPC"
                                }                                    
                            },
                            traits = new Contracts.TraitType[]
                            {
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.LockedForSale,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.LockedForSale,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "0"
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
                                                value = "0"
                                            }
                                        }
                                    }
                                },                                
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.TmDiscountEligible,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.TmDiscountEligible,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "1"
                                            }
                                        }
                                    }
                                },
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.CaseDiscountEligible,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.CaseDiscountEligible,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "0"
                                            }
                                        }
                                    }
                                },
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.RestrictedHours,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.RestrictedHours,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "0"
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
                                                value = "0"
                                            }
                                        }
                                    }
                                },
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.QuantityRequired,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.QuantityRequired,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "0"
                                            }
                                        }
                                    }
                                },
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.PriceRequired,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.PriceRequired,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "1"
                                            }
                                        }
                                    }
                                },
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.QuantityProhibit,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.QuantityProhibit,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "0"
                                            }
                                        }
                                    }
                                },
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.VisualVerify,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.VisualVerify,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = "0"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            if (includeLinkedItem)
            {
                (miniBulk.item[0].locale[0].Item as Contracts.StoreItemAttributesType).links = new Contracts.LinkTypeType[]
                {
                    new Contracts.LinkTypeType
                    {
                        childId = 123,
                        childIdSpecified = true,
                        parentId = 123,
                        parentIdSpecified = true
                    }
                };
            }

            return miniBulk;
        }

        public static MessageQueueItemLocale GetFakeMessageQueueItemLocale(int id, int localeId, string itemTypeCode, string linkedItemScanCode = null)
        {
            return new MessageQueueItemLocale
            {
                ItemId = id,
                LocaleId = localeId,
                MessageActionId = MessageActionTypes.AddOrUpdate,
                AgeCode = 1,
                InsertDate = DateTime.Now,
                BusinessUnit_ID = 10276,
                Sold_By_Weight = true,
                ScanCode = "4444444444",
                Case_Discount = true,
                LockedForSale = false,
                Recall = false,
                LocaleName = "Test",
                Price_Required = true,
                RegionCode = "SW",
                QtyProhibit = false,
                Restricted_Hours = false,
                Quantity_Required = false,
                ScaleForcedTare = false,
                TMDiscountEligible = true,
                LinkedItemScanCode = linkedItemScanCode,
                VisualVerify = false,
                PosScaleTare = 1,
                ItemTypeCode = itemTypeCode,
                ItemTypeDesc = "Retail Sale",
                ScanCodeId = 123,
                ScanCodeTypeDesc = "UPC",
                ScanCodeTypeId = ScanCodeTypes.Upc,
                MessageStatusId = MessageStatusTypes.Ready,
                ChangeType = "ScanCodeAdd",
                MessageTypeId = MessageTypes.ItemLocale
            };
        }

        public static MessageQueueHierarchy GetFakeMessageQueueHierarchy(int hierarchyId, string levelName, bool itemsAttached, string hierarchyClassId="123")
        {
            return new MessageQueueHierarchy
            {
                HierarchyId = hierarchyId,
                HierarchyName = "Merchandise",
                HierarchyClassId = hierarchyClassId,
                HierarchyClassName = "Muffins",
                HierarchyLevel = 3,
                HierarchyLevelName = levelName,
                HierarchyParentClassId = 456,
                InsertDate = DateTime.Now,
                ItemsAttached = itemsAttached,
                MessageActionId = MessageActionTypes.AddOrUpdate,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Hierarchy
            };
        }

        public static MessageQueuePrice GetFakeMessageQueuePrice(int id, int localeId, decimal? price, decimal? salePrice, decimal? previousSalePrice, DateTime? previousSaleEndDate = null)
        {
            return new MessageQueuePrice
            {
                MessageTypeId = MessageTypes.Price,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                IRMAPushID = 1,
                RegionCode = "SW",
                BusinessUnit_ID = 10111,
                ItemId = id,
                ItemTypeCode = "RTL",
                ItemTypeDesc = "Retail Sale",
                LocaleId = localeId,
                LocaleName = "Lamar",
                ScanCodeId = 1,
                ScanCodeTypeId = 1,
                ScanCodeTypeDesc = "UPC",
                ChangeType = String.Empty,
                Price = price,
                Multiple = 1,
                SalePrice = salePrice,
                SaleMultiple = 1,
                SaleStartDate = new DateTime(2015, 1, 1).Date,
                SaleEndDate = new DateTime(2016, 1, 1).Date,
                PreviousSalePrice = previousSalePrice,
                PreviousSaleMultiple = 1,
                PreviousSaleStartDate = new DateTime(2015, 2, 2).Date,
                PreviousSaleEndDate = previousSaleEndDate.HasValue ? previousSaleEndDate.Value : new DateTime(2016, 2, 2).Date,
                InsertDate = DateTime.Now,
                ScanCode = "123456",
                UomCode = "EA",
                CurrencyCode = "USD"
            };
        }

        public static MessageQueuePrice GetFakeMessageQueuePriceWithMultiple(int id, int localeId, decimal? price, decimal? salePrice, decimal? previousSalePrice, int? multiple, int? saleMultiple, DateTime? previousSaleEndDate = null)
        {
            return new MessageQueuePrice
            {
                MessageTypeId = MessageTypes.Price,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                IRMAPushID = 1,
                RegionCode = "SW",
                BusinessUnit_ID = 10111,
                ItemId = id,
                ItemTypeCode = "RTL",
                ItemTypeDesc = "Retail Sale",
                LocaleId = localeId,
                LocaleName = "Lamar",
                ScanCodeId = 1,
                ScanCodeTypeId = 1,
                ScanCodeTypeDesc = "UPC",
                ChangeType = String.Empty,
                Price = price,
                Multiple = multiple,
                SalePrice = salePrice,
                SaleMultiple = saleMultiple,
                SaleStartDate = new DateTime(2015, 1, 1).Date,
                SaleEndDate = new DateTime(2016, 1, 1).Date,
                PreviousSalePrice = previousSalePrice,
                PreviousSaleMultiple = 1,
                PreviousSaleStartDate = new DateTime(2015, 2, 2).Date,
                PreviousSaleEndDate = previousSaleEndDate.HasValue ? previousSaleEndDate.Value : new DateTime(2016, 2, 2).Date,
                InsertDate = DateTime.Now,
                ScanCode = "123456",
                UomCode = "EA",
                CurrencyCode = "USD"
            };
        }

        public static Contracts.items GetFakePriceMiniBulk()
        {
            var miniBulk = new Contracts.items();
            miniBulk.item = new Contracts.ItemType[3];

            miniBulk.item[0] = new Contracts.ItemType
            {
                id = 1,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = "RTL",
                        description = "Retail Sale"
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = "300",
                        name = "Lamar",
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
                                    id = 222,
                                    code = "2343456",
                                    typeId = 1,
                                    typeIdSpecified = true,
                                    typeDescription = "UPC"
                                }                                    
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                            {
                                id = "1",
                                uom = new Contracts.UomType
                                {
                                    codeSpecified = true,
                                    nameSpecified = true,
                                    code = Contracts.WfmUomCodeEnumType.EA,
                                    name = Contracts.WfmUomDescEnumType.EACH
                                },
                                currencyTypeCode = Contracts.CurrencyTypeCodeEnum.USD,
                                priceAmount = new Contracts.PriceAmount
                                {
                                    amount = 2.99m,
                                    amountSpecified = true
                                },
                                priceStartDate = new DateTime(1970, 1, 1).Date,
                                priceStartDateSpecified = true
                            }
                            }
                        }
                    }
                }
            };

            miniBulk.item[1] = new Contracts.ItemType
            {
                id = 1,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = "RTL",
                        description = "Retail Sale"
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.Delete,
                        ActionSpecified = true,
                        id = "300",
                        name = "Lamar",
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
                                    id = 222,
                                    code = "2343456",
                                    typeId = 1,
                                    typeIdSpecified = true,
                                    typeDescription = "UPC"
                                }                                    
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                            {
                                id = "1",
                                uom = new Contracts.UomType
                                {
                                    code = Contracts.WfmUomCodeEnumType.EA,
                                    name = Contracts.WfmUomDescEnumType.EACH
                                },
                                currencyTypeCode = Contracts.CurrencyTypeCodeEnum.USD,
                                priceAmount = new Contracts.PriceAmount
                                {
                                    amount = 1.99m,
                                    amountSpecified = true
                                },
                                priceStartDate = new DateTime(2015, 1, 1).Date,
                                priceStartDateSpecified = true,
                                priceEndDate = new DateTime(2016, 1, 1).Date,
                                priceEndDateSpecified = true
                            }
                            }
                        }
                    }
                }
            };

            miniBulk.item[2] = new Contracts.ItemType
            {
                id = 1,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = "RTL",
                        description = "Retail Sale"
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = "300",
                        name = "Lamar",
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
                                    id = 222,
                                    code = "2343456",
                                    typeId = 1,
                                    typeIdSpecified = true,
                                    typeDescription = "UPC"
                                }                                    
                            },
                            prices = new Contracts.PriceType[]
                            {
                                new Contracts.PriceType
                            {
                                id = "1",
                                uom = new Contracts.UomType
                                {
                                    codeSpecified = true,
                                    nameSpecified = true,
                                    code = Contracts.WfmUomCodeEnumType.EA,
                                    name = Contracts.WfmUomDescEnumType.EACH
                                },
                                currencyTypeCode = Contracts.CurrencyTypeCodeEnum.USD,
                                priceAmount = new Contracts.PriceAmount
                                {
                                    amount = 1.50m,
                                    amountSpecified = true
                                },
                                priceStartDate = new DateTime(2015, 1, 1).Date,
                                priceStartDateSpecified = true,
                                priceEndDate = new DateTime(2016, 1, 1).Date,
                                priceEndDateSpecified = true
                            }
                            }
                        }
                    }
                }
            };

            return miniBulk;
        }

        public static Contracts.HierarchyType GetFakeHierarchyMiniBulk(int hierarchyId, string levelName, string itemsAttached)
        {
            var miniBulk = new Contracts.HierarchyType();
            miniBulk.@class = new Contracts.HierarchyClassType[100];

            miniBulk.id = hierarchyId;
            miniBulk.name = "Merchandise";
            miniBulk.prototype = new Contracts.HierarchyPrototypeType
            {
                hierarchyLevelName = levelName,
                itemsAttached = itemsAttached
            };

            miniBulk.@class[0] = new Contracts.HierarchyClassType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                id = "123",
                name = "Muffins",
                level = 3,
                parentId = new Contracts.hierarchyParentClassType
                {
                    Value = 123
                }
            };

            return miniBulk;
        }

        public static Contracts.HierarchyType GetFakeHierarchyMiniBulk()
        {
            var miniBulk = new Contracts.HierarchyType();
            miniBulk.@class = new Contracts.HierarchyClassType[1];

            miniBulk.Action = Contracts.ActionEnum.AddOrUpdate;
            miniBulk.ActionSpecified = true;
            miniBulk.id = Hierarchies.Merchandise;
            miniBulk.name = HierarchyNames.Merchandise;
            miniBulk.prototype = new Contracts.HierarchyPrototypeType
            {
                hierarchyLevelName = "Brick",
                itemsAttached = "1"
            };

            miniBulk.@class[0] = new Contracts.HierarchyClassType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = "123",
                name = "Muffins",
                level = 2,
                parentId = new Contracts.hierarchyParentClassType
                {
                    Value = 1
                }
            };

            return miniBulk;
        }

        public static MessageQueueLocale GetFakeMessageQueueLocale(int localeId = 1)
        {
            return new MessageQueueLocale
            {
                MessageTypeId = MessageTypes.Locale,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                LocaleId = localeId,
                OwnerOrgPartyId = 1,
                LocaleName = "Lamar",
                LocaleOpenDate = new DateTime(2015, 1, 1).Date,
                LocaleCloseDate = new DateTime(2022, 1, 1).Date,
                LocaleTypeId = 4,
                ParentLocaleId = 2,
                BusinessUnitId = "10011",
                InsertDate = DateTime.Now,
                CountryCode = "USA",
                TerritoryCode = "TX",
                PostalCode = "78703",
                Latitude = null,
                Longitude = null,
                AddressLine1 = "Test Line 1",
                AddressLine2 = "Test Line 2",
                AddressLine3 = "Test Line 3",
                TimezoneCode = "CST",
                AddressId = 1,
                AddressUsageCode = "SHP",
                CityName = "Austin",
                CountryName = "United States",
                TerritoryName = "Texas",
                TimezoneName = "(UTC-06:00) Central Time (US & Canada)",
                PhoneNumber = "512-999-9999"
            };
        }

        public static MessageQueueLocale GetFakeMessageQueueLocaleRegion()
        {
            return new MessageQueueLocale
            {
                MessageTypeId = MessageTypes.Locale,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                LocaleId = 3,
                OwnerOrgPartyId = 1,
                LocaleName = "Southwest",
                LocaleOpenDate = new DateTime(2008, 1, 1).Date,
                LocaleCloseDate = null,
                LocaleTypeId = LocaleTypes.Region,
                ParentLocaleId = 1,
                BusinessUnitId = "0",
                InsertDate = DateTime.Now,
                CountryCode = null,
                TerritoryCode = null,
                PostalCode = null,
                Latitude = null,
                Longitude = null,
                AddressLine1 = null,
                AddressLine2 = null,
                AddressLine3 = null,
                TimezoneCode = null,
                AddressId = null,
                AddressUsageCode = null,
                CityName = null,
                CountryName = null,
                TerritoryName = null
            };
        }

        public static MessageQueueLocale GetFakeMessageQueueLocaleStore()
        {
            return new MessageQueueLocale
            {
                MessageTypeId = MessageTypes.Locale,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                LocaleId = 1,
                OwnerOrgPartyId = 1,
                StoreAbbreviation = "LMR",
                LocaleName = "Lamar",
                LocaleOpenDate = new DateTime(2015, 1, 1).Date,
                LocaleCloseDate = new DateTime(2022, 1, 1).Date,
                LocaleTypeId = LocaleTypes.Store,
                ParentLocaleId = 2,
                BusinessUnitId = "10011",
                InsertDate = DateTime.Now,
                AddressId = 55,
                AddressUsageCode = "SHP",
                CountryName = "United States",
                CountryCode = "USA",
                TerritoryName = "TER",
                TerritoryCode = "USA",
                CityName = "Austin",
                PostalCode = "78746",
                Latitude = "Lat",
                Longitude = "Long",
                AddressLine1 = "Test Line 1",
                AddressLine2 = "Test Line 2",
                AddressLine3 = "Test Line 3",
                TimezoneCode = "HST",
                TimezoneName = "(UTC-06:00) Central Time (US & Canada)",
                PhoneNumber = "512-999-9999"
            };
        }

        public static Contracts.LocaleType GetFakeLocaleMiniBulk()
        {
            return new Contracts.LocaleType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = "300",
                name = "Lamar",
                openDate = new DateTime(2015, 1, 1).Date,
                openDateSpecified = true,
                closeDate = new DateTime(2022, 1, 1).Date,
                closeDateSpecified = true,
                type = new Contracts.LocaleTypeType
                {
                    code = Contracts.LocaleCodeType.STR,
                    description = Contracts.LocaleDescType.Store
                },
                addresses = new Contracts.AddressType[]
                {
                    new Contracts.AddressType
                    {
                        id = 1,
                        idSpecified = true,
                        type = new Contracts.AddressTypeType
                        {
                            code = "Physical",
                            description = Contracts.AddressDescType.physical,
                            Item = new Contracts.PhysicalAddressType
                            {
                                addressLine1 = "711 University Ave",
                                addressLine2 = "San Diego",
                                addressLine3 = "California",
                                country = new Contracts.CountryType
                                {
                                    code = "USA",
                                    name = "United States"
                                },
                                postalCode = "91203"
                            }
                        },
                        usage = new Contracts.AddressUsageType
                        {
                            code = "Street",
                            description = Contracts.AddressUsgDescType.street
                        }
                    }
                },
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
                                    value = "10010"
                                }
                            }
                        }
                    }
                }
            };
        }

        internal static MessageQueueProductSelectionGroup GetFakeMessageQueueProductSelectionGroup(int productSelectionGroupId = 1)
        {
            return new MessageQueueProductSelectionGroup
            {
                MessageTypeId = MessageTypes.ProductSelectionGroup,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                MessageActionId = MessageActionTypes.AddOrUpdate,
                ProductSelectionGroupId = productSelectionGroupId,
                ProductSelectionGroupName = "Test",
                ProductSelectionGroupTypeName = "Test",
                ProductSelectionGroupTypeId = ProductSelectionGroupTypes.Consumable,
                InProcessBy = null,
                ProcessedDate = null
            };
        }
    }
}
