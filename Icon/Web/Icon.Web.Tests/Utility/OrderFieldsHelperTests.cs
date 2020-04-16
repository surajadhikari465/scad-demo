using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Utility
{
    [TestClass]
    public class OrderFieldsHelperTests
    {
        private OrderFieldsHelper orderFieldsHelper;
        private List<AttributeViewModel> attributeViewModels;
        Dictionary<float, string> defaultFields;
        Dictionary<string, string> expectedOrderOfFields;

        [TestInitialize]
        public void Initialize()
        {
            orderFieldsHelper = new OrderFieldsHelper();
            defaultFields = new Dictionary<float, string>();
            defaultFields.Add((float)0.5, "ItemId");
            defaultFields.Add((float)1.5, "BarcodeType");
            defaultFields.Add((float)2.1, "ItemType");
            defaultFields.Add((float)2.2, "ScanCode");
            defaultFields.Add((float)2.5, "Brand");
            defaultFields.Add((float)8.2, "Merchandise");
            defaultFields.Add((float)8.1, "Financial");
            defaultFields.Add((float)8.5, "National");
            defaultFields.Add((float)8.8, "Tax");
            defaultFields.Add((float)83.1, "Manufacturer");

        }

        [TestMethod]
        public void ValidateOrderAllFields_ReturnsDictionaryWithCorrectOrder()
        {
            //given
            attributeViewModels = new List<AttributeViewModel>()
            {
                new AttributeViewModel(){AttributeId = 1, AttributeName="365Eligible", DisplayOrder =1 },
                new AttributeViewModel(){AttributeId = 2, AttributeName="RequestNumber", DisplayOrder =2 },
                new AttributeViewModel(){AttributeId = 3, AttributeName="Inactive", DisplayOrder =5 },
                new AttributeViewModel(){AttributeId = 4, AttributeName="POSDescription", DisplayOrder =10 },
                new AttributeViewModel(){AttributeId = 5, AttributeName="ItemPack", DisplayOrder =99 },
                new AttributeViewModel(){AttributeId = 6, AttributeName="VitaminK", DisplayOrder =100 },
            };

            expectedOrderOfFields = new Dictionary<string, string>() {
                                        {"ItemId","F" },
                                        {"365Eligible","A" },
                                        {"BarcodeType","F" },
                                        {"RequestNumber","A" },
                                        {"ItemType","F" },
                                        {"ScanCode","F" },
                                        {"Brand","F" },
                                        {"Inactive","A" },
                                        {"Financial","F" },
                                        {"Merchandise","F" },
                                         {"National","F" },
                                        {"Tax","F" },
                                        {"POSDescription","A" },
                                        {"Manufacturer","F" },
                                        {"ItemPack","A" },
                                        {"VitaminK","A" }

                };
            //when
            Dictionary<string, string> orderedFields = orderFieldsHelper.OrderAllFields(attributeViewModels);

            //then
            Assert.IsTrue(orderedFields.SequenceEqual(expectedOrderOfFields));
        }    
    }
}
