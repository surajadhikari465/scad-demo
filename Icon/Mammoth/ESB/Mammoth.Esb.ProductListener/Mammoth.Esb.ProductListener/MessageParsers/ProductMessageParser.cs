using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.ProductListener.MessageParsers
{
    public class ProductMessageParser : MessageParserBase<Contracts.items, List<ProductModel>>
    {
        public override List<ProductModel> ParseMessage(IEsbMessage message)
        {
            List<ProductModel> products = new List<ProductModel>();
            Contracts.items items = DeserializeMessage(message);

            if (items.item.First().locale.First().Item.GetType() != typeof(Contracts.EnterpriseItemAttributesType))
            {
                return null;
            }

            foreach (var item in items.item)
            {
                var enterpriseItemAttributes = item.locale.First().Item as Contracts.EnterpriseItemAttributesType;
                
                var traits = enterpriseItemAttributes.traits;
                var hierarchyClasses = enterpriseItemAttributes.hierarchies;
                var consumerInformation = item.@base.consumerInformation;

                var product = new ProductModel();
                product.ItemID = item.id;
                product.ItemTypeID = GetItemTypeId(item);
                product.ScanCode = enterpriseItemAttributes.scanCodes.First().code;
                product.Desc_Product = GetTraitValue(TraitCodes.ProductDescription, traits);
                product.Desc_POS = GetTraitValue(TraitCodes.PosDescription, traits);
                product.PackageUnit = GetTraitValue(TraitCodes.PackageUnit, traits);
                product.RetailSize = GetTraitValue(TraitCodes.RetailSize, traits);
                product.RetailUOM = GetTraitValue(TraitCodes.RetailUom, traits);
                product.FoodStampEligible = GetBoolTraitValue(TraitCodes.FoodStampEligible, traits);
                product.SubBrickID = GetHierarchyClassIntId(HierarchyNames.Merchandise, hierarchyClasses);
                product.NationalClassID = GetHierarchyClassIntId(HierarchyNames.National, hierarchyClasses);
                product.BrandHCID = GetHierarchyClassIntId(HierarchyNames.Brands, hierarchyClasses);
                product.MessageTaxClassHCID = GetHierarchyClassStringId(HierarchyNames.Tax, hierarchyClasses);
                product.PSNumber = GetHierarchyClassIntId(HierarchyNames.Financial, hierarchyClasses);

                //TODO: INSERT sign attributes into the Mammoth DB via the AddOrUpdateProductsCommandHandler
                //product.AirChilled = GetBoolTraitValue(TraitCodes.AirChilled, traits);
                //product.AnimalWelfareRating = GetTraitValue(TraitCodes.AnimalWelfareRating, traits);
                //product.Biodynamic = GetBoolTraitValue(TraitCodes.Biodynamic, traits);
                //product.CheeseMilkType = GetTraitValue(TraitCodes.CheeseMilkType, traits);
                //product.CheeseRaw = GetBoolTraitValue(TraitCodes.CheeseRaw, traits);
                //product.DryAged = GetBoolTraitValue(TraitCodes.DryAged, traits);
                //product.EcoScaleRating = GetTraitValue(TraitCodes.EcoScaleRating, traits);
                //product.FreeRange = GetBoolTraitValue(TraitCodes.FreeRange, traits);
                //product.GlutenFreeAgency = GetTraitValue(TraitCodes.GlutenFree, traits);
                //product.GrassFed = GetBoolTraitValue(TraitCodes.GrassFed, traits);
                //product.HealthyEatingRating = GetTraitValue(TraitCodes.HealthyEatingRating, traits);
                //product.KosherAgency = GetTraitValue(TraitCodes.Kosher, traits);
                //product.MadeInHouse = GetBoolTraitValue(TraitCodes.MadeInHouse, traits);
                //product.Msc = GetBoolTraitValue(TraitCodes.Msc, traits);
                //product.NonGmoAgency = GetTraitValue(TraitCodes.NonGmo, traits);
                //product.OrganicAgency = GetTraitValue(TraitCodes.Organic, traits);
                //product.PastureRaised = GetBoolTraitValue(TraitCodes.PastureRaised, traits);
                //product.PremiumBodyCare = GetBoolTraitValue(TraitCodes.PremiumBodyCare, traits);
                //product.SeafoodCatchType = GetTraitValue(TraitCodes.SeafoodCatchType, traits);
                //product.SeafoodFreshOrFrozen = GetTraitValue(TraitCodes.FreshOrFrozen, traits);
                //product.VeganAgency = GetTraitValue(TraitCodes.Vegan, traits);
                //product.Vegetarian = GetBoolTraitValue(TraitCodes.Vegetarian, traits);
                //product.WholeTrade = GetBoolTraitValue(TraitCodes.WholeTrade, traits);

                products.Add(product);
            }

            return products;
        }

        private int GetItemTypeId(Contracts.ItemType item)
        {
            switch(item.@base.type.code)
            {
                case ItemTypeCodes.RetailSale:
                    return ItemTypes.RetailSale;
                case ItemTypeCodes.Coupon:
                    return ItemTypes.Coupon;
                case ItemTypeCodes.Deposit:
                    return ItemTypes.Deposit;
                case ItemTypeCodes.Fee:
                    return ItemTypes.Fee;
                case ItemTypeCodes.NonRetail:
                    return ItemTypes.NonRetail;
                case ItemTypeCodes.Return:
                    return ItemTypes.Return;
                case ItemTypeCodes.Tare:
                    return ItemTypes.Tare;
                default: throw new ArgumentException(string.Format("Item type {0} is not supported.", item.@base.type.code));
            }
        }

        private bool? GetBoolTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            string traitValue = GetTraitValue(traitCode, traits);
            if (String.IsNullOrEmpty(traitValue))
            {
                return null;
            }
            else if(traitValue == "0")
            {
                return false;
            }
            else if(traitValue == "1")
            {
                return true;
            }
            else
            {
                throw new ArgumentException(string.Format("'{0}' is not a supported boolean value for trait code '{1}'", traitValue, traitCode));
            }
        }

        private int? GetHierarchyClassIntId(string hierarchyName, Contracts.HierarchyType[] hierarchyClasses)
        {
            var hierarchy = hierarchyClasses.FirstOrDefault(h => h.name == hierarchyName);
            if(hierarchy != null)
            {
                return int.Parse(hierarchy.@class.First().id);
            }

            return null;
        }

        private string GetHierarchyClassStringId(string hierarchyName, Contracts.HierarchyType[] hierarchyClasses)
        {
            var hierarchy = hierarchyClasses.FirstOrDefault(h => h.name == hierarchyName);
            if (hierarchy != null)
            {
                return hierarchy.@class.First().id;
            }

            return null;
        }

        private string GetTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            var trait = traits.FirstOrDefault(t => t.code == traitCode);

            if (trait != null)
            {
                return trait.type.value.First().value;
            }

            return null;
        }
    }
}
