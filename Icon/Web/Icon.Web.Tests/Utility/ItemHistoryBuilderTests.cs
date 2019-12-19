using Icon.Common;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Utility
{
    [TestClass]
    public class ItemHistoryBuilderTests
    {
        private List<ItemHistoryModel> itemHistory = new List<ItemHistoryModel>();
        private ItemHierarchyClassHistoryAllModel hierarchyHistory = new ItemHierarchyClassHistoryAllModel();
        private List<AttributeDisplayModel> attributes;

        [TestInitialize]
        public void Initialize()
        {
            this.attributes = new List<AttributeDisplayModel>()
            {
                new AttributeDisplayModel()
                {
                    AttributeName = "TestAttribute",
                    DisplayName = "Test Attribute",
                },
                new AttributeDisplayModel()
                {
                    AttributeName = "Test2Attribute",
                    DisplayName = "Test2 Attribute",
                },
                new AttributeDisplayModel()
                {
                    AttributeName = Constants.Attributes.ModifiedBy,
                    DisplayName = "Modified By",
                },
                new AttributeDisplayModel()
                {
                    AttributeName = Constants.Attributes.ModifiedDateTimeUtc,
                    DisplayName = "Modified Date",
                }
            };

            this.itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01").ToString()},
                    { "TestAttribute" , "A"},
                    { "Test2Attribute" ,"1"},
                }
            });

            this.itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:10"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser2"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01 00:00:10").ToString()},
                    { "TestAttribute" , "B"},
                    { "Test2Attribute" ,"2"},
                }
            });

            this.itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:20"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser3"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01 00:00:20").ToString()},
                    { "TestAttribute" , "C"},
                    { "Test2Attribute" ,"3"},
                }
            });

            this.hierarchyHistory.BrandHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Brands",
                    HierarchyLineage="Brands | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                },
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Brands",
                    HierarchyLineage="Brands | Coke",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:20"),
                }
            };

            this.hierarchyHistory.MerchHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Merchandise",
                    HierarchyLineage="Merchandise | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                },
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Merchandise",
                    HierarchyLineage="Merchandise | Beer",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:20"),
                }
            };

            this.hierarchyHistory.TaxHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Tax",
                    HierarchyLineage="Tax | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                },
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Tax",
                    HierarchyLineage="Tax | Local",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:20"),
                }
            };

            this.hierarchyHistory.FinancialHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Financial",
                    HierarchyLineage="Financial | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                },
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Financial",
                    HierarchyLineage="Financial | Money",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:20"),
                }
            };

            this.hierarchyHistory.NationalHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="National",
                    HierarchyLineage="National | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                },
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="National",
                    HierarchyLineage="National | Texas",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:20"),
                }
            };

            this.hierarchyHistory.ManufacturerHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Manufacturer",
                    HierarchyLineage="DOLE",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                },
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Manufacturer",
                    HierarchyLineage="Farms",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-02 00:00:20"),
                }
            };
        }

        [TestMethod]
        public void BuildItemHistory_AllHierarchiesHaveCorrectLineageAndUserAndDatesAreCorrectAndShouldOrderedDateDescending()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(this.itemHistory, this.hierarchyHistory, this.attributes, null);

            // Then.
            Assert.AreEqual(3, response.RevisionsByDate.Count);

            // this block of asserts corresponds to the last item record inserted and so it becomes the 
            // first item in the history because history is sorted by date descending
            Assert.AreEqual(DateTime.Parse("2001-01-01 00:00:20"), response.RevisionsByDate[0].Date);
            Assert.AreEqual("TestUser3", response.RevisionsByDate[0].User);
            Assert.AreEqual("Brands | Coke", response.RevisionsByDate[0].Values["Brands"]);
            Assert.AreEqual("Merchandise | Beer", response.RevisionsByDate[0].Values["Merchandise"]);
            Assert.AreEqual("Tax | Local", response.RevisionsByDate[0].Values["Tax"]);
            Assert.AreEqual("Financial | Money", response.RevisionsByDate[0].Values["Financial"]);
            Assert.AreEqual("National | Texas", response.RevisionsByDate[0].Values["National"]);

            // this block of asserts corresponds to the first item record inserted and it's the last item in the history.
            Assert.AreEqual(DateTime.Parse("2001-01-01 00:00:00"), response.RevisionsByDate[2].Date);
            Assert.AreEqual("TestUser", response.RevisionsByDate[2].User);
            Assert.AreEqual("Brands | TEST", response.RevisionsByDate[2].Values["Brands"]);
            Assert.AreEqual("Merchandise | TEST", response.RevisionsByDate[2].Values["Merchandise"]);
            Assert.AreEqual("Tax | TEST", response.RevisionsByDate[2].Values["Tax"]);
            Assert.AreEqual("Financial | TEST", response.RevisionsByDate[2].Values["Financial"]);
            Assert.AreEqual("National | TEST", response.RevisionsByDate[2].Values["National"]);
            Assert.AreEqual("DOLE", response.RevisionsByDate[2].Values["Manufacturer"]);
        }

        [TestMethod]
        public void AddHierarchyHistoryToResponse_ItemRevisionWithin5Seconds_RevisionShouldBeAssociated()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var itemHistory = new List<ItemHistoryModel>();
            var hierarchyHistory = new ItemHierarchyClassHistoryAllModel();

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01 00:00:00").ToString()},
                    { "TestAttribute" , "A"},
                    { "Test2Attribute" ,"1"},
                }
            });

            hierarchyHistory.BrandHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Brands",
                    HierarchyLineage="Brands | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:05"),
                }
            };

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, hierarchyHistory, this.attributes, null);

            // Then.
            Assert.AreEqual("Brands | TEST", response.RevisionsByDate.First().Values["Brands"], "The brands hierarchy was created within 5 seconds after the item and should be associated.");
        }

        [TestMethod]
        public void AddHierarchyHistoryToResponse_ItemRevisionNotWithin5Seconds_ShouldUseCurrentItemRecord()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var itemHistory = new List<ItemHistoryModel>();
            var hierarchyHistory = new ItemHierarchyClassHistoryAllModel();

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01 00:00:00").ToString()},
                    { "TestAttribute" , "A"},
                    { "Test2Attribute" ,"1"},
                }
            });

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-01-01"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-01-01 00:00:00").ToString()},
                    { "TestAttribute" , "B"},
                    { "Test2Attribute" ,"2"},
                }
            });

            hierarchyHistory.BrandHierarchy = new List<ItemHierarchyClassHistoryModel>()
            {
                new ItemHierarchyClassHistoryModel()
                {
                    HierarchyName="Brands",
                    HierarchyLineage="Brands | TEST",
                    ItemId=1,
                    SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:06"),
                }
            };

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, hierarchyHistory, this.attributes, null);

            // Then.
            Assert.IsTrue(response.RevisionsByDate.First().Values.ContainsKey("Brands"), "The brands hierarchy was created 6 seconds after the item and should be associated to the latest item record.");
            Assert.AreEqual(DateTime.Parse("2002-01-01 00:00:00"), response.RevisionsByDate.First().Date);
        }

        [TestMethod]
        public void RemoveAttributesFromResponseThatShouldBeHidden_ModifiedDateTime_ShouldBeRemovedFromDateAndAttributecollections()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            // When.
            var response = builder.BuildItemHistory(this.itemHistory, this.hierarchyHistory, this.attributes, null);

            // Then.
            Assert.IsFalse(response.RevisionsByDate.Any(x => x.Values.Any(value => value.Key == Constants.Attributes.ModifiedDateTimeUtc)));
            Assert.IsFalse(response.RevisionsByAttribute.ContainsKey(Constants.Attributes.ModifiedDateTimeUtc));
            Assert.IsFalse(response.RevisionsByDate.Any(x => x.Values.Any(value => value.Key == Constants.Attributes.ModifiedBy)));
            Assert.IsFalse(response.RevisionsByAttribute.ContainsKey(Constants.Attributes.ModifiedBy));
        }

        /// <summary>
        /// If an item is saved and nothing changed the only real changes are the ModifiedBy and ModifiedDatetime attributes.
        /// These should not be displayed as history. 
        /// </summary>
        [TestMethod]
        public void RemoveEmptyRevisions_HasEmptyRevisions_RevisionShouldNotBeReturned()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var itemHistory = new List<ItemHistoryModel>();
            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                ItemTypeCode="RTL",
                SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01 00:00:00").ToString()}
                }
            });

            // When.
            var response = builder.BuildItemHistory(itemHistory, new ItemHierarchyClassHistoryAllModel(), this.attributes, null);

            // Then.
            Assert.IsTrue(response.RevisionsByDate.Count == 0);
            Assert.IsTrue(response.RevisionsByAttribute.Count == 0);
        }

        [TestMethod]
        public void OrderResponseRevisionHistory_RevisionsByDate_ShouldBeSortedByDateDescendingAndAttributesShouldBeSortedAlphabetically()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var response = new ItemHistoryViewModel()
            {
                RevisionsByDate = new List<RevisionByDate>()
                {
                    new RevisionByDate()
                    {
                        Date = DateTime.Parse("2002-01-01 00:00:00"),
                        Id = Guid.Parse("7f84475d-8189-4a5c-8894-2c97f6cc26d0"),
                        User = "A",
                        Values = new Dictionary<string, string>()
                        {
                            { "C","Value" },
                            { "B","Value" },
                            { "A","Value" }
                        }
                    },
                    new RevisionByDate()
                    {
                        Date = DateTime.Parse("2002-01-01 00:00:05"),
                        Id = Guid.Parse("5a1b8d10-f723-4c87-a7e4-840bd2c32402"),
                        User = "A",
                        Values = new Dictionary<string, string>()
                    },
                    new RevisionByDate()
                    {
                        Date = DateTime.Parse("2002-01-01 00:00:10"),
                        Id = Guid.Parse("66da5e5f-314d-41eb-aa6a-dcc9822f6dcc"),
                        User = "A",
                        Values = new Dictionary<string, string>()
                    }
                }
            };

            // When.
            builder.OrderResponseRevisionHistory(response);

            // Then.
            Assert.AreEqual(Guid.Parse("66da5e5f-314d-41eb-aa6a-dcc9822f6dcc"), response.RevisionsByDate[0].Id);
            Assert.AreEqual(Guid.Parse("5a1b8d10-f723-4c87-a7e4-840bd2c32402"), response.RevisionsByDate[1].Id);
            Assert.AreEqual(Guid.Parse("7f84475d-8189-4a5c-8894-2c97f6cc26d0"), response.RevisionsByDate[2].Id);

            // this should be the last record (2) from the collection because the records were sorted by date
            // and the first RevisionByDate became the last.
            Assert.AreEqual("A", response.RevisionsByDate[2].Values.Keys.ToList()[0]);
            Assert.AreEqual("B", response.RevisionsByDate[2].Values.Keys.ToList()[1]);
            Assert.AreEqual("C", response.RevisionsByDate[2].Values.Keys.ToList()[2]);
        }

        [TestMethod]
        public void OrderResponseRevisionHistory_RevisionsByAttribute_ShouldBeSortedByNumberOfRevisions()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var response = new ItemHistoryViewModel()
            {
                RevisionsByAttribute = new Dictionary<string, List<RevisionByAttribute>>()
                {
                     {
                       "AttributeNameB", new List<RevisionByAttribute>()
                       {
                           new RevisionByAttribute()
                           {
                               Date=DateTime.Parse("2002-01-01 00:00:00"),
                               Id = Guid.Parse("776bcc55-b880-472b-8b0b-288528d8bfdc"),
                               User="User",
                               NewValue="ValueChangedTo1"
                           },
                            new RevisionByAttribute()
                            {
                                Date=DateTime.Parse("2001-01-01 00:00:00"),
                                Id = Guid.Parse("66da5e5f-314d-41eb-aa6a-dcc9822f6dcc"),
                                User="User",
                                NewValue="ValueChanged"
                            }
                       }
                   },
                    {
                       "AttributeNameA", new List<RevisionByAttribute>()
                       {
                           new RevisionByAttribute()
                           {
                               Date=DateTime.Parse("2002-01-01 00:00:00"),
                               Id = Guid.Parse("eddf4c16-2ef3-47c7-8535-9e6eb43bd17b"),
                               User="User",
                               NewValue="ValueChangedAgain"
                           }
                       }
                   }
                }
            };

            // When.
            builder.OrderResponseRevisionHistory(response);

            // Then.
            List<string> keys = response.RevisionsByAttribute.Keys.ToList();
            Assert.AreEqual("AttributeNameB", keys[0], "AttributeNameB should be first because it has 2 revisions");
            Assert.AreEqual("AttributeNameA", keys[1], "AttributeNameA should be second because it has 1 revision");
        }

        [TestMethod]
        public void AddRevisionsByAttributeToResponse_AttributeRevisionsAreCreated()
        {
            // Given.
            var response = new ItemHistoryViewModel()
            {
                RevisionsByDate = new List<RevisionByDate>()
                {
                    new RevisionByDate()
                    {
                        Date = DateTime.Parse("2002-01-01 00:00:00"),
                        Id = Guid.Parse("7f84475d-8189-4a5c-8894-2c97f6cc26d0"),
                        User = "A",
                        Values = new Dictionary<string, string>()
                        {
                            { "AttributeA","ValueA"}
                        }
                    },
                    new RevisionByDate()
                    {
                        Date = DateTime.Parse("2002-01-01 00:00:05"),
                        Id = Guid.Parse("5a1b8d10-f723-4c87-a7e4-840bd2c32402"),
                        User = "A",
                        Values = new Dictionary<string, string>()
                        {
                            { "AttributeB","ValueB"}
                        }
                    },
                    new RevisionByDate()
                    {
                        Date = DateTime.Parse("2002-01-01 00:00:10"),
                        Id = Guid.Parse("66da5e5f-314d-41eb-aa6a-dcc9822f6dcc"),
                        User = "A",
                        Values = new Dictionary<string, string>()
                        {
                            { "AttributeA","ValueC"}
                        }
                    }
                }
            };
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            // When.
            builder.AddRevisionsByAttributeToResponse(response);

            // Then.
            Assert.AreEqual(2, response.RevisionsByAttribute.Count, "There should be two revisions by attribute");
            Assert.AreEqual(2, response.RevisionsByAttribute["AttributeA"].Count, "There should be two revisions for AttributeA");
            Assert.AreEqual(1, response.RevisionsByAttribute["AttributeB"].Count, "There should be one revisions for AttributeB");
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_RevisionsAreCreated()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(this.itemHistory, this.hierarchyHistory, this.attributes, null);

            // Then.
            Assert.AreEqual(3, response.RevisionsByDate.Count);
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_SingleHistoryRecord_RevisionIsCreated()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var itemHistory = new List<ItemHistoryModel>
            {
                new ItemHistoryModel()
                {
                    ItemId = 1,
                    ItemTypeCode = "RTL",
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                    ItemAttributes = new Dictionary<string, string>()
                    {
                        { Constants.Attributes.ModifiedBy, "TestUser"},
                        { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01").ToString()},
                        { "TestAttribute" , "A"},
                        { "Test2Attribute" ,"1"},
                    }
                }
            };

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, new ItemHierarchyClassHistoryAllModel(), this.attributes, null);

            // Then.
            Assert.AreEqual(1, response.RevisionsByDate.Count);
            Assert.AreEqual(3, response.RevisionsByAttribute.Count);
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_SingleHistoryRecord_ItemTypeIsSet()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var itemHistory = new List<ItemHistoryModel>
            {
                new ItemHistoryModel()
                {
                    ItemId = 1,
                    ItemTypeCode = "RTL",
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                    ItemAttributes = new Dictionary<string, string>()
                    {
                        { Constants.Attributes.ModifiedBy, "TestUser"},
                        { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01").ToString()},
                        { "TestAttribute" , "A"},
                        { "Test2Attribute" ,"1"},
                    }
                }
            };

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, new ItemHierarchyClassHistoryAllModel(), this.attributes, null);

            // Then.
            Assert.AreEqual("RTL", response.RevisionsByDate[0].Values[Constants.Attributes.ItemTypeCode]);
            Assert.AreEqual("RTL", response.RevisionsByAttribute[Constants.Attributes.ItemTypeCode].First().NewValue);
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_SingleHistoryRecord_ItemTypeIsNotSetShouldNotCreateItemTypeRevision()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            var itemHistory = new List<ItemHistoryModel>
            {
                new ItemHistoryModel()
                {
                    ItemId = 1,
                    ItemTypeCode = "",
                    SysStartTimeUtc = DateTime.Parse("2001-01-01"),
                    ItemAttributes = new Dictionary<string, string>()
                    {
                        { Constants.Attributes.ModifiedBy, "TestUser"},
                        { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2001-01-01").ToString()},
                        { "TestAttribute" , "A"},
                        { "Test2Attribute" ,"1"},
                    }
                }
            };

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, new ItemHierarchyClassHistoryAllModel(), this.attributes, null);

            // Then.
            Assert.IsFalse(response.RevisionsByDate[0].Values.ContainsKey(Constants.Attributes.ItemTypeCode));
        }

        [TestMethod]
        public void Diff_AttributeIsRemoved_AttributeRevisionIsSetToRemoved()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            ItemHistoryModel previous = new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:00"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    { "AttributeA","Value"},
                    { Constants.Attributes.ModifiedBy,"User" }
                }
            };

            ItemHistoryModel next = new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:05"),
                ItemAttributes = new Dictionary<string, string>()
                {
                      {Constants.Attributes.ModifiedBy,"User" }
                }
            };

            // When.
            RevisionByDate response = builder.Diff(previous, next);

            // Then.
            Assert.AreEqual(DateTime.Parse("2001-01-01 00:00:05"), response.Date, "Date should equal the latest change");
            Assert.AreEqual(1, response.Values.Keys.Count, "There should only be a single attribute in this test");
            Assert.AreEqual("REMOVED", response.Values["AttributeA"], "AttributeA had its value removed in the seconds revision so it should be set to REMOVED");
        }

        [TestMethod]
        public void Diff_AttributeIsAdded_RevisionIsCreated()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            ItemHistoryModel previous = new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:00"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    {Constants.Attributes.ModifiedBy,"User" }
                }
            };

            ItemHistoryModel next = new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:05"),
                ItemAttributes = new Dictionary<string, string>()
                {
                     { "AttributeA","Value"},
                     {Constants.Attributes.ModifiedBy,"User" }
                }
            };

            // When.
            RevisionByDate response = builder.Diff(previous, next);

            // Then.
            Assert.AreEqual("Value", response.Values["AttributeA"], "AttributeA had its value set to Value in the second revision so it should be Value");
        }

        [TestMethod]
        public void Diff_ItemTypeChanged_RevisionIsCreated()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            ItemHistoryModel previous = new ItemHistoryModel()
            {
                ItemId = 1,
                ItemTypeCode="RTL",
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:00"),
                ItemAttributes = new Dictionary<string, string>()
                {
                    {Constants.Attributes.ModifiedBy,"User" }
                }
            };

            ItemHistoryModel next = new ItemHistoryModel()
            {
                ItemId = 1,
                ItemTypeCode = "NTL",
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:05"),
                ItemAttributes = new Dictionary<string, string>()
                {
                     {Constants.Attributes.ModifiedBy,"User" }
                }
            };

            // When.
            RevisionByDate response = builder.Diff(previous, next);

            // Then.
            Assert.AreEqual("NTL", response.Values[Constants.Attributes.ItemTypeCode], "ItemTypeCode had its value set to NTL in the second revision so it should be NTL");
        }

        [TestMethod]
        public void Diff_AttributeIsModified_RevisionIsCreated()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();

            ItemHistoryModel previous = new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:00"),
                ItemAttributes = new Dictionary<string, string>()
                {
                     { "AttributeA","Value"},
                     {Constants.Attributes.ModifiedBy,"User" }
                }
            };

            ItemHistoryModel next = new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2001-01-01 00:00:05"),
                ItemAttributes = new Dictionary<string, string>()
                {
                     { "AttributeA","ValueChanged"},
                     { Constants.Attributes.ModifiedBy,"User" }
                }
            };

            // When.
            RevisionByDate response = builder.Diff(previous, next);

            // Then.
            Assert.AreEqual("ValueChanged", response.Values["AttributeA"], "AttributeA had its value changed to ValueChanged in the second revision so it should be ValueChanged");
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_SingleHistoryRecord_ShouldCreateManufacturerRevision()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();
            var viewModel = new ItemViewModel(){ ItemId = 1, ManufacturerHierarchyClassId = 1 };

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-02"),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-02 00:00:00").ToString()},
                }
            });

            var history = new ItemHierarchyClassHistoryAllModel();
            history.ManufacturerHierarchy.Add(new ItemHierarchyClassHistoryModel()
                {
                    HierarchyId = 8,
                    HierarchyClassId = 100,
                    HierarchyName = "Manufacturer",
                    HierarchyLineage = "Test",
                    SysStartTimeUtc = DateTime.Parse("2002-02-02"),
                    SysEndTimeUtc = DateTime.Parse("2002-02-03")
                });


            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, history, this.attributes, viewModel);

            // Then.
            Assert.IsTrue(response.RevisionsByDate[0].Values.ContainsKey("Manufacturer"));
            Assert.AreEqual(1, response.RevisionsByDate.SelectMany(x => x.Values).Where(x => x.Key == "Manufacturer").Count());
            Assert.AreEqual("Test", response.RevisionsByDate[0].Values["Manufacturer"]);
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_MultipleHistoryRecords_ShouldCreateManufacturerRevision()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();
            var viewModel = new ItemViewModel(){ ItemId = 1, ManufacturerHierarchyClassId = 1 };

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-02"),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-02 00:00:00").ToString()},
                }
            });

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-03"),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-03 00:00:00").ToString()},
                }
            });

            var history = new ItemHierarchyClassHistoryAllModel();
            history.ManufacturerHierarchy.Add(new ItemHierarchyClassHistoryModel()
                {
                    HierarchyId = 8,
                    HierarchyClassId = 100,
                    HierarchyName = "Manufacturer",
                    HierarchyLineage = "Test",
                    SysStartTimeUtc = DateTime.Parse("2002-02-02"),
                    SysEndTimeUtc = DateTime.Parse("2002-02-03")
                });

            history.ManufacturerHierarchy.Add(new ItemHierarchyClassHistoryModel()
            {
                HierarchyId = 8,
                HierarchyClassId = 101,
                HierarchyName = "Manufacturer",
                HierarchyLineage = "Test1",
                SysStartTimeUtc = DateTime.Parse("2002-02-03"),
                SysEndTimeUtc = DateTime.MaxValue
            });


            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, history, this.attributes, viewModel);

            // Then.
            Assert.IsTrue(response.RevisionsByDate[0].Values.ContainsKey("Manufacturer"));
            Assert.AreEqual(2, response.RevisionsByDate.SelectMany(x => x.Values).Where(x => x.Key == "Manufacturer").Count());
            Assert.AreEqual("Test1", response.RevisionsByDate[0].Values["Manufacturer"]);
            Assert.AreEqual("Test", response.RevisionsByDate[1].Values["Manufacturer"]);
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_SingleRecord_ShouldCreateManufacturerRevisionWithRemovedRecord()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();
            var viewModel = new ItemViewModel(){ ItemId = 1, ManufacturerHierarchyClassId = 0 };

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-02"),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-02 00:00:00").ToString()},
                }
            });

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-03"),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-03 00:00:00").ToString()},
                }
            });

            var history = new ItemHierarchyClassHistoryAllModel();
            history.ManufacturerHierarchy.Add(new ItemHierarchyClassHistoryModel()
                {
                    HierarchyId = 8,
                    HierarchyClassId = 100,
                    HierarchyName = "Manufacturer",
                    HierarchyLineage = "Test",
                    SysStartTimeUtc = DateTime.Parse("2002-02-02"),
                    SysEndTimeUtc = DateTime.Parse("2002-02-03")
                });

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, history, this.attributes, viewModel);

            // Then.
            Assert.AreEqual(2, response.RevisionsByDate.SelectMany(x => x.Values).Where(x => x.Key == "Manufacturer").Count());
            Assert.AreEqual("REMOVED", response.RevisionsByDate[0].Values["Manufacturer"]);
            Assert.AreEqual("Test", response.RevisionsByDate[1].Values["Manufacturer"]);
        }

        [TestMethod]
        public void CreateItemRevisionsByDate_MultipleHistoryRecords_ShouldCreateManufacturerRevisionWithRemovedRecord()
        {
            // Given.
            IItemHistoryBuilder builder = new ItemHistoryBuilder();
            var viewModel = new ItemViewModel(){ ItemId = 1, ManufacturerHierarchyClassId = 1 };

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-02"),
                SysEndTimeUtc = DateTime.Parse("2002-02-02 8:00:00"),
                
                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-02 00:00:00").ToString()},
                }
            });

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-02 8:01:00"),
                SysEndTimeUtc = DateTime.Parse("2002-02-02 23:50:00 "),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-02 8:01:00").ToString()},
                }
            });

            itemHistory.Add(new ItemHistoryModel()
            {
                ItemId = 1,
                SysStartTimeUtc = DateTime.Parse("2002-02-03"),
                SysEndTimeUtc = DateTime.Parse("2002-02-04"),

                ItemAttributes = new Dictionary<string, string>()
                {
                    { Constants.Attributes.ModifiedBy, "TestUser"},
                    { Constants.Attributes.ModifiedDateTimeUtc , DateTime.Parse("2002-02-03 00:00:00").ToString()},
                }
            });

            var history = new ItemHierarchyClassHistoryAllModel();
            history.ManufacturerHierarchy.Add(new ItemHierarchyClassHistoryModel()
                {
                    HierarchyId = 8,
                    HierarchyClassId = 100,
                    HierarchyName = "Manufacturer",
                    HierarchyLineage = "Test",
                    SysStartTimeUtc = DateTime.Parse("2002-02-02"),
                    SysEndTimeUtc = DateTime.Parse("2002-02-02 8:00:00")
                });

            history.ManufacturerHierarchy.Add(new ItemHierarchyClassHistoryModel()
                {
                    HierarchyId = 8,
                    HierarchyClassId = 100,
                    HierarchyName = "Manufacturer",
                    HierarchyLineage = "Test1",
                    SysStartTimeUtc = DateTime.Parse("2002-02-03"),
                    SysEndTimeUtc = DateTime.MaxValue
                });

            // When.
            ItemHistoryViewModel response = builder.BuildItemHistory(itemHistory, history, this.attributes, viewModel);

            // Then.
            Assert.AreEqual(3, response.RevisionsByDate.SelectMany(x => x.Values).Where(x => x.Key == "Manufacturer").Count());
            Assert.AreEqual("Test1", response.RevisionsByDate[0].Values["Manufacturer"]);
            Assert.AreEqual("REMOVED", response.RevisionsByDate[1].Values["Manufacturer"]);
            Assert.AreEqual("Test", response.RevisionsByDate[2].Values["Manufacturer"]);
        }
    }
}