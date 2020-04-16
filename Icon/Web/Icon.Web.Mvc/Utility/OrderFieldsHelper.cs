using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Mvc.Utility
{
    public class OrderFieldsHelper : IOrderFieldsHelper
    {
        // to identify in the view if its attribute field or non attribute field we are designating attribute fields with Type A and non attribute fields with Type F
        private const string attributePrefix = "A";
        private const string nonAttributePrefix = "F";

        public Dictionary<string, string> OrderAllFields(List<AttributeViewModel> attributeViewModels)
        {
            Dictionary<float, FieldNameDisplayOrderModel> sortedFieldsDictionary = new Dictionary<float, FieldNameDisplayOrderModel>();
            Dictionary<float, string> nonAttributefieldsDictionary = ItemNonAttributeFieldsDefaultOrder.OrderForNonAttributefields();

            Dictionary<int, string> attributefieldsDictionary = attributeViewModels.ToDictionary(k => (int)k.DisplayOrder, k => k.AttributeName);

            foreach (var item in attributefieldsDictionary)
                sortedFieldsDictionary.Add(item.Key, new FieldNameDisplayOrderModel { Name = item.Value, Type = attributePrefix });

            foreach (var item in nonAttributefieldsDictionary)
                sortedFieldsDictionary.Add(item.Key, new FieldNameDisplayOrderModel { Name = item.Value, Type = nonAttributePrefix });

            return sortedFieldsDictionary.OrderBy(s => s.Key).ToDictionary(k => k.Value.Name, k => k.Value.Type);

        }
    }
}