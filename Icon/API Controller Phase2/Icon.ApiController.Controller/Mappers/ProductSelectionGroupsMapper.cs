
using Icon.ApiController.DataAccess.Queries;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.Mappers
{
    public class ProductSelectionGroupsMapper : IProductSelectionGroupsMapper
    {
        private IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>> getProductSelectionGroupsQueryHandler;

        private List<ProductSelectionGroup> traitProductSelectionGroups;
        private List<ProductSelectionGroup> merchandiseHierarchyProductSelectionGroups;
        private Dictionary<int, Func<MessageQueueProduct, string>> traitIdToProductMessageTraitValues;
        private Dictionary<int, Func<MessageQueueItemLocale, string>> traitIdToItemLocaleMessageTraitValues;

        public ProductSelectionGroupsMapper(IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>> getProductSelectionGroupsQueryHandler)
        {
            this.getProductSelectionGroupsQueryHandler = getProductSelectionGroupsQueryHandler;
        }

        /// <summary>
        /// Loades the list of PSGs and the Trait ID to message trait value Dictionaries.
        /// </summary>
        public void LoadProductSelectionGroups()
        {
            List<ProductSelectionGroup> productSelectionGroups = getProductSelectionGroupsQueryHandler.Search(new GetProductSelectionGroupsParameters());

            traitProductSelectionGroups = productSelectionGroups.Where(psg => psg.TraitId != null && psg.TraitValue != null).ToList();
            merchandiseHierarchyProductSelectionGroups = productSelectionGroups.Where(psg => psg.MerchandiseHierarchyClassId != null).ToList();
            LoadTraitIdToMessageTraitValues(traitProductSelectionGroups);
        }

        /// <summary>
        /// Loads two Dictionary objects of Trait IDs to Funcs. Each Func returns a value from a property on a message. 
        /// The Dictionaries are then filtered by PSGs. Any KeyValuePair with a Trait ID that doesn't exist in the PSGs is removed.
        /// </summary>
        /// <param name="traitProductSelectionGroups">The list of PSGs to filter the dictionaries by.</param>
        private void LoadTraitIdToMessageTraitValues(List<ProductSelectionGroup> traitProductSelectionGroups)
        {
            LoadTraitIdToProductMessageTraitValues(traitProductSelectionGroups);
            LoadTraitIdToItemLocaleMessageTraitValues(traitProductSelectionGroups);
        }

        /// <summary>
        /// Loads the Dictionary for the Product message
        /// </summary>
        /// <param name="traitProductSelectionGroups">The list of PSGs to filter the dictionaries by.</param>
        private void LoadTraitIdToProductMessageTraitValues(List<ProductSelectionGroup> traitProductSelectionGroups)
        {
            traitIdToProductMessageTraitValues = new Dictionary<int, Func<MessageQueueProduct, string>>();

            traitIdToProductMessageTraitValues.Add(Traits.ProductDescription, (m) => m.ProductDescription);
            traitIdToProductMessageTraitValues.Add(Traits.PosDescription, (m) => m.PosDescription);
            traitIdToProductMessageTraitValues.Add(Traits.PackageUnit, (m) => m.PackageUnit);
            traitIdToProductMessageTraitValues.Add(Traits.RetailSize, (m) => m.RetailSize);
            traitIdToProductMessageTraitValues.Add(Traits.RetailUom, (m) => m.RetailUom);
            traitIdToProductMessageTraitValues.Add(Traits.FoodStampEligible, (m) => m.FoodStampEligible);
            traitIdToProductMessageTraitValues.Add(Traits.DepartmentSale, (m) => m.DepartmentSale);
            traitIdToProductMessageTraitValues.Add(Traits.ProhibitDiscount, (m) => m.ProhibitDiscount.BoolToString());

            foreach (var traitIdToTraitValue in traitIdToProductMessageTraitValues.Where((kvp) => !traitProductSelectionGroups.Any(psg => psg.TraitId == kvp.Key)).ToList())
            {
                traitIdToProductMessageTraitValues.Remove(traitIdToTraitValue.Key);
            }
        }

        /// <summary>
        /// Loads the Dictionary for the ItemLocale message
        /// </summary>
        /// <param name="traitProductSelectionGroups">The list of PSGs to filter the dictionaries by.</param>
        private void LoadTraitIdToItemLocaleMessageTraitValues(List<ProductSelectionGroup> traitProductSelectionGroups)
        {
            traitIdToItemLocaleMessageTraitValues = new Dictionary<int, Func<MessageQueueItemLocale, string>>();

            traitIdToItemLocaleMessageTraitValues.Add(Traits.LockedForSale, (m) => m.LockedForSale.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.Recall, (m) => m.Recall.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.TmDiscountEligible, (m) => m.TMDiscountEligible.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.CaseDiscountEligible, (m) => m.Case_Discount.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.AgeRestrict, (m) => m.AgeCode.HasValue ? m.AgeCode.Value.ToString() : "0");
            traitIdToItemLocaleMessageTraitValues.Add(Traits.RestrictedHours, (m) => m.Restricted_Hours.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.SoldByWeight, (m) => m.Sold_By_Weight.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.QuantityRequired, (m) => m.Quantity_Required.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.PriceRequired, (m) => m.Price_Required.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.QuantityProhibit, (m) => m.QtyProhibit.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.VisualVerify, (m) => m.VisualVerify.BoolToString());
            traitIdToItemLocaleMessageTraitValues.Add(Traits.PosScaleTare, (m) => m.PosScaleTare.HasValue ? m.PosScaleTare.Value.ToString() : "0");

            foreach (var traitIdToTraitValue in traitIdToItemLocaleMessageTraitValues.Where((kvp) => !traitProductSelectionGroups.Any(psg => psg.TraitId == kvp.Key)).ToList())
            {
                traitIdToItemLocaleMessageTraitValues.Remove(traitIdToTraitValue.Key);
            }
        }

        /// <summary>
        /// Returns a SelectionGroupsType, which represents the Product Selection Groups for a Product message.
        /// </summary>
        /// <param name="productMessage">The Product message to generate Product Selection Groups from.</param>
        /// <returns>The SelectionGroupsType, which represents the Product Selection Groups.</returns>
        public Contracts.SelectionGroupsType GetProductSelectionGroups(MessageQueueProduct productMessage)
        {
            List<Contracts.GroupTypeType> groupTypes = CreatePsgElementsForTraits(productMessage, traitIdToProductMessageTraitValues);

            var productsMerchandisePsg = merchandiseHierarchyProductSelectionGroups.SingleOrDefault(psg => psg.MerchandiseHierarchyClassId == productMessage.MerchandiseClassId);

            if (productsMerchandisePsg != null)
            {
                groupTypes.Add(CreateMessageElementFromPsg(productsMerchandisePsg, true));

                foreach (var merchandisePsg in merchandiseHierarchyProductSelectionGroups
                    .Where(psg => psg.ProductSelectionGroupId != productsMerchandisePsg.ProductSelectionGroupId))
                {
                    groupTypes.Add(CreateMessageElementFromPsg(merchandisePsg, false));
                }
            }
            else
            {
                foreach (var merchandisePsg in merchandiseHierarchyProductSelectionGroups)
                {
                    groupTypes.Add(CreateMessageElementFromPsg(merchandisePsg, false));
                }
            }

            return groupTypes.Any() ? new Contracts.SelectionGroupsType() { group = groupTypes.ToArray() } : null;
        }

        /// <summary>
        /// Returns a SelectionGroupsType, which represents the Product Selection Groups for a ItemLocale message.
        /// </summary>
        /// <param name="itemLocaleMessage">The ItemLocale message to generate Product Selection Groups from.</param>
        /// <returns>The SelectionGroupsType, which represents the Product Selection Groups.</returns>
        public Contracts.SelectionGroupsType GetProductSelectionGroups(MessageQueueItemLocale itemLocaleMessage)
        {
            List<Contracts.GroupTypeType> productSelectionGroups = CreatePsgElementsForTraits(itemLocaleMessage, traitIdToItemLocaleMessageTraitValues);

            return productSelectionGroups.Any() ? new Contracts.SelectionGroupsType() { group = productSelectionGroups.ToArray() } : null;
        }

        /// <summary>
        /// Creates list of GroupTypeTypes, which represent the Product Selection Groups, by using the Message and the Dictionary of Trait IDs to Funcs.
        /// </summary>
        /// <typeparam name="T">The type of Message object.</typeparam>
        /// <param name="message">The Message to generate Product Selection Groups for.</param>
        /// <param name="traitIdToMessageTraitValues">The Dictionary of Trait IDs to Funcs.</param>
        /// <returns>The GroupTypeTypes which represent the Product Selection Groups for the message parameter.</returns>
        private List<Contracts.GroupTypeType> CreatePsgElementsForTraits<T>(T message, Dictionary<int, Func<T, string>> traitIdToMessageTraitValues)
        {
            List<Contracts.GroupTypeType> groupTypes = new List<Contracts.GroupTypeType>();

            foreach (var traitIdToTraitValue in traitIdToMessageTraitValues)
            {
                var traitValue = traitIdToTraitValue.Value(message);
                var productSelectionGroups = traitProductSelectionGroups.Where(p => p.TraitId == traitIdToTraitValue.Key);

                // An item in R10 cannot be associated to conflicting PSGs. An example is that an item cannot be both in the Restrict 18 and Restrict 21, it must one or the other. 
                // So we need to generate delete PSGs elements for other PSGs with the same trait ID if the message's trait value does not equal the PSG's trait value.
                foreach (var productSelectionGroup in productSelectionGroups)
                {
                    var groupType = CreateMessageElementFromPsg(productSelectionGroup, productSelectionGroup.TraitValue == traitValue);
                    groupTypes.Add(groupType);
                }
            }

            return groupTypes;
        }

        /// <summary>
        /// Converts a ProductSelectionGroup object into a GroupTypeType object. Also sets the GroupTypeType's Action to AddOrUpdate or Delete based on the addOrUpdate bool.
        /// </summary>
        /// <param name="psg">The ProductSelectionGroup to convert into a GroupTypeType.</param>
        /// <param name="addOrUpdate">The bool to specify whether the Action should be AddOrUpdate or Delete.</param>
        /// <returns>The GroupTypeType which represents a Product Selection Group in the ESB.</returns>
        private Contracts.GroupTypeType CreateMessageElementFromPsg(ProductSelectionGroup psg, bool addOrUpdate)
        {
            return new Contracts.GroupTypeType
            {
                Action = addOrUpdate ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                ActionSpecified = true,
                id = psg.ProductSelectionGroupName,
                name = psg.ProductSelectionGroupName,
                type = psg.ProductSelectionGroupType.ProductSelectionGroupTypeName
            };
        }
    }
}
