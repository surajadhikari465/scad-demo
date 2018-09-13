using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AmazonLoad.Common;
using Dapper;
using Icon.Common;
using Icon.Framework;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.IconItemLocale
{
    public class IconItemLocalePsgMapper
    {
        private string iconConnectionString;

        internal protected virtual List<ProductSelectionGroupModel> TraitProductSelectionGroups { get; set; }
        internal protected virtual Dictionary<int, Func<ItemLocaleModelForWormhole, string>> TraitIdToItemLocaleMessageTraitValues { get; set; }

        public IconItemLocalePsgMapper() { }

        public IconItemLocalePsgMapper(string iconConnectionString) : this()
        {
            this.iconConnectionString = iconConnectionString;
        }

        /// <summary>
        /// Loads the list of PSGs and the Trait ID to message trait value Dictionaries.
        /// </summary>
        public void LoadProductSelectionGroups()
        {
            var productSelectionGroups = QueryForProductSelectionGroups();
            TraitProductSelectionGroups = FilterProductSelectionGroups(productSelectionGroups);
            TraitIdToItemLocaleMessageTraitValues = LoadTraitIdToItemLocaleMessageTraitValues(TraitProductSelectionGroups);
        }

        internal protected virtual List<ProductSelectionGroupModel> QueryForProductSelectionGroups()
        {
            List<ProductSelectionGroupModel> productSelectionGroups = null;
            using (var sqlConnection = new SqlConnection(this.iconConnectionString))
            {
                productSelectionGroups = sqlConnection
                    .Query<ProductSelectionGroupModel>(SqlQueries.ProductSelectionGroupSql)
                    .ToList();
            }
            return productSelectionGroups;
        }

        internal protected virtual List<ProductSelectionGroupModel> FilterProductSelectionGroups(
            IList<ProductSelectionGroupModel> psgs)
        {
            return psgs.Where(psg => psg.TraitId != null && psg.TraitValue != null)
                .ToList();
        }

        internal protected virtual Dictionary<int, Func<ItemLocaleModelForWormhole, string>> LoadTraitIdToItemLocaleMessageTraitValues(
            List<ProductSelectionGroupModel> traitProductSelectionGroups)
        {
            var traitIdToItemLocaleMessageTraitValues = new Dictionary<int, Func<ItemLocaleModelForWormhole, string>>();

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

            var dictionaryItemsNotInPSGList = traitIdToItemLocaleMessageTraitValues
                .Where((kvp) => !traitProductSelectionGroups.Any(psg => psg.TraitId == kvp.Key))
                .ToList();
            foreach (var traitIdToTraitValue in dictionaryItemsNotInPSGList)
            {
                traitIdToItemLocaleMessageTraitValues.Remove(traitIdToTraitValue.Key);
            }
            return traitIdToItemLocaleMessageTraitValues;
        }

        public List<Contracts.GroupTypeType> CreatePsgElementsForTraits(ItemLocaleModelForWormhole itemLocale)
        {
            return CreatePsgElementsForTraits(itemLocale, this.TraitIdToItemLocaleMessageTraitValues);
        }

        /// <summary>
        /// Creates list of GroupTypeTypes, which represent the Product Selection Groups, by using the Message and the Dictionary of Trait IDs to Funcs.
        /// </summary>
        /// <typeparam name="T">The type of Message object.</typeparam>
        /// <param name="message">The Message to generate Product Selection Groups for.</param>
        /// <param name="traitIdToMessageTraitValues">The Dictionary of Trait IDs to Funcs.</param>
        /// <returns>The GroupTypeTypes which represent the Product Selection Groups for the message parameter.</returns>
        private List<Contracts.GroupTypeType> CreatePsgElementsForTraits<T>(
            T message, Dictionary<int, Func<T, string>> traitIdToMessageTraitValues)
        {
            List<Contracts.GroupTypeType> groupTypes = new List<Contracts.GroupTypeType>();

            foreach (var traitIdToTraitValue in traitIdToMessageTraitValues)
            {
                var traitValue = traitIdToTraitValue.Value(message);
                var productSelectionGroups = TraitProductSelectionGroups.Where(p => p.TraitId == traitIdToTraitValue.Key);

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
        private Contracts.GroupTypeType CreateMessageElementFromPsg(
            ProductSelectionGroupModel psg, bool addOrUpdate)
        {
            return new Contracts.GroupTypeType
            {
                Action = addOrUpdate ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                ActionSpecified = true,
                id = psg.ProductSelectionGroupName,
                name = psg.ProductSelectionGroupName,
                type = psg.ProductSelectionGroupTypeName
            };
        }
    }
}