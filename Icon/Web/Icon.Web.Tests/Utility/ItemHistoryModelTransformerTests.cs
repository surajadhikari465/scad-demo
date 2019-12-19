using Icon.Common;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Icon.Web.Mvc.Utility.ItemHistory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Utility
{
    [TestClass]
    public class ItemHistoryModelTransformerTests
    {
        [TestMethod]
        public void TransformInforHistory_DataIsTransformedCorrectly()
        {
            // Given.
            IHistoryModelTransformer transformer = new HistoryModelTransformer();

            // When.
            List<ItemInforHistoryDbModel> inforHistory = new List<ItemInforHistoryDbModel>()
            {
                new ItemInforHistoryDbModel()
                {
                    ItemId=4000000,
                    JsonObject = @"{ 
                       ""ItemID"":""4000000"",
                       ""Validated"":""True"",
                       ""ScanCode"":""5500001286839"",
                       ""BarcodeType"":""UPC"",
                       ""Name"":""SANITIZER HAND UNSCENTED"",
                       ""Brand"":{ 
                          ""Brand ID"":""134441"",
                          ""Brand Name"":""EO PRODUCTS"",
                          ""Brand Abbreviation"":""EOPRO""
                       },
                       ""Tax"":{ 
                          ""Tax Hier ID"":""40424"",
                          ""Tax Class ID"":""9989000"",
                          ""Tax Class"":""9989000 NON-TAXABLE PRODUCTS AND SERVICES"",
                          ""Tax Abbreviation"":""9989000 NON-TAXABLE PRODUCTS AND SVCS"",
                          ""Tax Romance"":""9989000 NON-TAXABLE PRODUCTS AND SVCS""
                       },
                       ""Subteam"":{ 
                          ""Subteam"":""3700"",
                          ""Financial Hier ID"":""84237"",
                          ""Subteam Name"":""Body Care (3700)""
                       },
                       ""Merchandise"":{ 
                          ""Segment"":{ 
                             ""ID"":""82799"",
                             ""Name"":""Beauty/Personal Care/Hygiene""
                          },
                          ""Family"":{ 
                             ""ID"":""82856"",
                             ""Name"":""Skin Products""
                          },
                          ""Class"":{ 
                             ""ID"":""83002"",
                             ""Name"":""Skin Care""
                          },
                          ""GS1 Brick"":{ 
                             ""ID"":""83362"",
                             ""Name"":""Skin Care Other""
                          },
                          ""Sub Brick"":{ 
                             ""ID"":""84052"",
                             ""Name"":""Skin Care Other:Body Care (3700)""
                          }
                       },
                       ""National"":{ 
                          ""Family"":{ 
                             ""ID"":""121187"",
                             ""Name"":""Packaging & Supplies""
                          },
                          ""Category"":{ 
                             ""ID"":""121466"",
                             ""Name"":""Chemicals""
                          },
                          ""Sub Category"":{ 
                             ""ID"":""122152"",
                             ""Name"":""Chemicals""
                          },
                          ""Class"":{ 
                             ""ID"":""125152"",
                             ""Name"":""Chemicals""
                          }
                       },
                       ""Food Stamp Eligible"":""False"",
                       ""Item Pack"":""1"",
                       ""Item Status"":""Complete"",
                       ""POS Description"":""EOPRO SANITIZER HAND"",
                       ""POS Scale Tare"":""0"",
                       ""Product Description"":""SANITIZER HAND UNSCENTED"",
                       ""Prohibit Discount"":""False"",
                       ""Retail Size"":""6"",
                       ""UOM"":""CS"",
                       ""Created On"":""2017-03-04T20:18:01.609Z"",
                       ""Created By"":""System"",
                       ""Updated On"":""2017-03-05T08:40:22.534Z"",
                       ""Updated By"":""0902090@wholefoods.com""
                    }"
                },
                new ItemInforHistoryDbModel()
                {
                    ItemId=4000000,
                    JsonObject = @"{ 
               ""ItemID"":""4000000"",
               ""Validated"":""True"",
               ""ScanCode"":""5500001286839"",
               ""BarcodeType"":""UPC"",
               ""Name"":""SANITIZER HAND UNSCENTED"",
               ""Brand"":{ 
                  ""Brand ID"":""134441"",
                  ""Brand Name"":""EO PRODUCTS"",
                  ""Brand Abbreviation"":""EOPRO""
               },
               ""Item Type"":{ 
                  ""Non Merch Trait"":""N/A"",
                  ""Item Type"":""RTL""
               },
               ""Tax"":{ 
                  ""Tax Hier ID"":""40424"",
                  ""Tax Class ID"":""9989000"",
                  ""Tax Class"":""9989000 NON-TAXABLE PRODUCTS AND SERVICES"",
                  ""Tax Abbreviation"":""9989000 NON-TAXABLE PRODUCTS AND SVCS"",
                  ""Tax Romance"":""9989000 NON-TAXABLE PRODUCTS AND SVCS""
               },
               ""Subteam"":{ 
                  ""Subteam"":""3700"",
                  ""Financial Hier ID"":""84237"",
                  ""Subteam Name"":""Body Care (3700)""
               },
               ""Merchandise"":{ 
                  ""Segment"":{ 
                     ""ID"":""82799"",
                     ""Name"":""Beauty/Personal Care/Hygiene""
                  },
                  ""Family"":{ 
                     ""ID"":""82856"",
                     ""Name"":""Skin Products""
                  },
                  ""Class"":{ 
                     ""ID"":""83002"",
                     ""Name"":""Skin Care""
                  },
                  ""GS1 Brick"":{ 
                     ""ID"":""83362"",
                     ""Name"":""Skin Care Other""
                  },
                  ""Sub Brick"":{ 
                     ""ID"":""84052"",
                     ""Name"":""Skin Care Other:Body Care (3700) WITH A CHANGE""
                  }
               },
               ""National"":{ 
                  ""Family"":{ 
                     ""ID"":""121187"",
                     ""Name"":""Packaging & Supplies""
                  },
                  ""Category"":{ 
                     ""ID"":""121466"",
                     ""Name"":""Chemicals""
                  },
                  ""Sub Category"":{ 
                     ""ID"":""122152"",
                     ""Name"":""Chemicals""
                  },
                  ""Class"":{ 
                     ""ID"":""125152"",
                     ""Name"":""Chemicals""
                  }
               },
               ""Food Stamp Eligible"":""True"",
               ""Item Pack"":""1"",
               ""Item Status"":""Complete"",
               ""POS Description"":""EOPRO SANITIZER HAND"",
               ""POS Scale Tare"":""0"",
               ""Product Description"":""SANITIZER HAND UNSCENTED"",
               ""Prohibit Discount"":""False"",
               ""Retail Size"":""6"",
               ""UOM"":""CS"",
               ""Created On"":""2017-03-04T20:18:01.609Z"",
               ""Created By"":""System"",
               ""Updated On"":""2017-03-04T08:40:22.534Z"",
               ""Updated By"":""0902090@wholefoods.com""
            }"}
            };

            List<AttributeDisplayModel> attributes = new List<AttributeDisplayModel>()
            {
                new AttributeDisplayModel()
                {
                    AttributeName = "Food Stamp Eligible",
                    DisplayName = "Food Stamp Eligible"
                }
            };

            // Then.
            List<ItemHistoryModel> result = transformer.TransformInforHistory(inforHistory, attributes);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(4000000, result[1].ItemId);

            Assert.AreEqual(4000000, result[0].ItemId);
            Assert.AreEqual("RTL", result[0].ItemTypeCode);
            Assert.AreEqual(DateTime.Parse("2017-03-04T08:40:22.534Z").ToShortDateString(), result[0].SysStartTimeUtc.ToShortDateString());
            Assert.AreEqual("EO PRODUCTS", result[0].ItemAttributes[InforConstants.Brand]);
            Assert.AreEqual("9989000 NON-TAXABLE PRODUCTS AND SVCS", result[0].ItemAttributes[InforConstants.Tax]);
            Assert.AreEqual("Body Care (3700)", result[0].ItemAttributes[InforConstants.Financial]);
            Assert.AreEqual("Packaging & Supplies | Chemicals | Chemicals | Chemicals", result[0].ItemAttributes[InforConstants.National]);
            Assert.AreEqual("Beauty/Personal Care/Hygiene | Skin Products | Skin Care | Skin Care Other | Skin Care Other:Body Care (3700) WITH A CHANGE", result[0].ItemAttributes[InforConstants.Merchandise]);
            Assert.AreEqual("True", result[0].ItemAttributes["Food Stamp Eligible"]);

            Assert.AreEqual("", result[1].ItemTypeCode);
            Assert.AreEqual(DateTime.Parse("2017-03-05T08:40:22.534Z").ToShortDateString(), result[1].SysStartTimeUtc.ToShortDateString());
            Assert.AreEqual("EO PRODUCTS", result[1].ItemAttributes[InforConstants.Brand]);
            Assert.AreEqual("9989000 NON-TAXABLE PRODUCTS AND SVCS", result[1].ItemAttributes[InforConstants.Tax]);
            Assert.AreEqual("Body Care (3700)", result[1].ItemAttributes[InforConstants.Financial]);
            Assert.AreEqual("Packaging & Supplies | Chemicals | Chemicals | Chemicals", result[1].ItemAttributes[InforConstants.National]);
            Assert.AreEqual("Beauty/Personal Care/Hygiene | Skin Products | Skin Care | Skin Care Other | Skin Care Other:Body Care (3700)", result[1].ItemAttributes[InforConstants.Merchandise]);
            Assert.AreEqual("False", result[1].ItemAttributes["Food Stamp Eligible"]);

        }

        [TestMethod]
        public void ToViewModels_DataIsConvertedAndInDateAscendingOrder()
        {
            // Given.
            IHistoryModelTransformer transformer = new HistoryModelTransformer();

            // When.
            List<ItemHistoryModel> result = transformer.ToViewModels(new List<ItemHistoryDbModel>()
            {
                new ItemHistoryDbModel()
                {
                    ItemTypeCode = "RTL",
                    ItemId = 1,
                    ItemTypeId = 1,
                    SysStartTimeUtc = DateTime.Parse("2002-01-01"),
                    ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string,string>()
                    {
                        {"AttributeA","ValueA" }
                    })
                },
                new ItemHistoryDbModel()
                {
                    ItemTypeCode = "NRTL",
                    ItemId = 2,
                    ItemTypeId = 2,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                    ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string,string>()
                    {
                        {"AttributeB","ValueB" }
                    })
                }
            });


            // Then.
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(result[0].ItemId, 2);
            Assert.AreEqual(result[0].ItemTypeId, 2);
            Assert.AreEqual(result[0].ItemTypeCode, "NRTL");
            Assert.AreEqual(result[0].SysStartTimeUtc, DateTime.Parse("2001-01-01"));
            Assert.AreEqual(result[0].ItemAttributes["AttributeB"], "ValueB");

            Assert.AreEqual(result[1].ItemId, 1);
            Assert.AreEqual(result[1].ItemTypeId, 1);
            Assert.AreEqual(result[1].ItemTypeCode, "RTL");
            Assert.AreEqual(result[1].SysStartTimeUtc, DateTime.Parse("2002-01-01"));
            Assert.AreEqual(result[1].ItemAttributes["AttributeA"], "ValueA");
        }
    }
}