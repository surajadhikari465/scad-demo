using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;
using Newtonsoft.Json;
using Icon.Web.Mvc.Utility.ItemHistory;

namespace Icon.Web.Mvc.Utility
{
    public class ItemHistoryBuilder : IItemHistoryBuilder
    {

        /// <summary>
        /// Creates a ItemHistoryModel for an item. This contains a list of revisions by date and a dictionary of changes keyed by
        /// attribute. These are the two ways the revisions will be displayed to the user.
        /// </summary>
        /// <param name="itemHistory"></param>
        /// <param name="hierarchyHistory"></param>
        /// <param name="attributes"></param>c
        /// <returns></returns>
        public ItemHistoryViewModel BuildItemHistory(IEnumerable<ItemHistoryModel> itemHistory, ItemHierarchyClassHistoryAllModel hierarchyHistory, IEnumerable<AttributeDisplayModel> attributes, ItemViewModel viewModel)
        {
            ItemHistoryViewModel response = CreateItemRevisionsByDate(itemHistory.ToList());
            this.AddHierarchyHistoryToResponse(response, hierarchyHistory, viewModel);
            this.AddRevisionsByAttributeToResponse(response);
            this.RemoveAttributesFromResponseThatShouldBeHidden(response);
            this.RemoveEmptyRevisions(response);
            this.OrderResponseRevisionHistory(response);
            response.Attributes = (from attribute in attributes select new
                                   AttributeDisplayModel
                                    {
                                        AttributeName=attribute.AttributeName,
                                        DisplayName=attribute.DisplayName
                                    }).ToList();
            return response;
        }



        /// <summary>
        /// Adds ItemHierarchyClass history to the response object. Item history does not have a direct reference to ItemHierarchyClass history.
        /// Also ItemHierarchyHClass doesn't track the user who made the change which we need to display. To work around this problem we're attempting to correleate records in
        /// ItemHierarchyClassHistory to ItemHistory by looking for ItemHistory records with a SysStartTimeUtc within 5 seconds of SysStartTimeUtc on
        /// ItemHierarchyClassHistory. This should work as they are saved together and it's unlikely two saves to the same item will occur within 5 seconds.
        /// There isn't a ItemHistory record on the first create. If we can't find an ItemHistory record to compare to we will use the current Item record.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="hierarchyHistory"></param>
        public void AddHierarchyHistoryToResponse(ItemHistoryViewModel response, ItemHierarchyClassHistoryAllModel hierarchyHistory, ItemViewModel viewModel)
        {
            Action<List<ItemHierarchyClassHistoryModel>> addHierarchyChange = (history) =>
            {
                foreach (var historyEntry in history)
                {
                    // find the item revision that occurred within 5 seconds of when this Item Hierarchy class revision was made.
                    // this is an inexact lookup that could result in the wrong data being displayed.
                    var itemRevision = response.RevisionsByDate.FirstOrDefault(x => historyEntry.SysStartTimeUtc.Subtract(x.Date).TotalSeconds < 5);

                    if (itemRevision == null)
                    {
                        // when a hierarchy is changed on an item at that's the only change there is no
                        // item history record created. In those cases just use the current item
                        itemRevision = response.RevisionsByDate.OrderByDescending(x => x.Date).First();
                    }

                    itemRevision.Values[historyEntry.HierarchyName] = historyEntry.HierarchyLineage;
                }
            };

            addHierarchyChange(hierarchyHistory.MerchHierarchy);
            addHierarchyChange(hierarchyHistory.TaxHierarchy);
            addHierarchyChange(hierarchyHistory.FinancialHierarchy);
            addHierarchyChange(hierarchyHistory.NationalHierarchy);
            addHierarchyChange(hierarchyHistory.BrandHierarchy);
            addHierarchyChange(CreateManufacturerHistory(hierarchyHistory.ManufacturerHierarchy, viewModel));
        }

        /// <summary>
        /// This function removed any attributes that should not be displayed to the user.
        /// </summary>
        /// <param name="response"></param>
        public void RemoveAttributesFromResponseThatShouldBeHidden(ItemHistoryViewModel response)
        {
            for (int i = 0; i < response.RevisionsByDate.Count; i++)
            {
                // "Modified" attributes aren't displayed because the data is already in the UI
                response.RevisionsByDate[i].Values.Remove(Constants.Attributes.ModifiedBy);
                response.RevisionsByDate[i].Values.Remove(Constants.Attributes.ModifiedDateTimeUtc);
            }

            for (int i = 0; i < response.RevisionsByAttribute.Count; i++)
            {
                // "Modified" attributes aren't displayed because the data is already in the UI
                response.RevisionsByAttribute.Remove(Constants.Attributes.ModifiedBy);
                response.RevisionsByAttribute.Remove(Constants.Attributes.ModifiedDateTimeUtc);
            }
        }

        /// <summary>
        /// If two history records that exist and the only difference is the modified date there will not be anything to display
        /// in the history. So here we remove those entries.
        /// </summary>
        /// <param name="response"></param>
        public void RemoveEmptyRevisions(ItemHistoryViewModel response)
        {
            response.RevisionsByDate.RemoveAll(x => x.Values.Keys.Count == 0);
            response.RevisionsByDate.RemoveAll(x => x.Values.Count == 1 && x.Values.Keys.First() == Constants.Attributes.ItemTypeCode);

            if (response.RevisionsByAttribute.Keys.Count == 1 && response.RevisionsByAttribute.Keys.First() == Constants.Attributes.ItemTypeCode)
            {
                response.RevisionsByAttribute.Remove(Constants.Attributes.ItemTypeCode);
            }
        }

        public void OrderResponseRevisionHistory(ItemHistoryViewModel response)
        {
            // order date history most recent first
            response.RevisionsByDate = response.RevisionsByDate.OrderByDescending(x => x.Date).ToList();

            // then order each collection of attributes in alphabetical order
            for (int i=0; i < response.RevisionsByDate.Count; i++)
            {
                response.RevisionsByDate[i].Values = response.RevisionsByDate[i].Values.OrderBy(x => x.Key).ToDictionary();
            }

            // order attribute history by greatest number of changes then alphabetically
            response.RevisionsByAttribute = response.RevisionsByAttribute
                .OrderByDescending(x => x.Value.Count)
                .ThenBy(x => x.Key)
                .ToDictionary();

            // order the collections of attribute changes most recent first
            List<string> revisionsByAttributeKeys = response.RevisionsByAttribute.Keys.ToList();
            foreach (string key in revisionsByAttributeKeys)
            {
                response.RevisionsByAttribute[key] = response.RevisionsByAttribute[key].OrderByDescending(x => x.Date).ToList();
            }
        }

        /// <summary>
        /// Takes a ItemHistoryModel and groups all of the revisions by date and groups them by attribute
        /// </summary>
        /// <param name="response"></param>
        public void AddRevisionsByAttributeToResponse(ItemHistoryViewModel response)
        {
            foreach (var revision in response.RevisionsByDate)
            {
                foreach (string key in revision.Values.Keys)
                {
                    if (!response.RevisionsByAttribute.ContainsKey(key))
                    {
                        response.RevisionsByAttribute[key] = new List<RevisionByAttribute>();
                    }
                    response.RevisionsByAttribute[key].Add(new RevisionByAttribute()
                    {
                        Id = Guid.NewGuid(),
                        Date = revision.Date,
                        User = revision.User,
                        NewValue = revision.Values[key]
                    });
                }
            }
        }


     

        /// <summary>
        /// This function takes a list of sorted ItemHistoryViewModels and diffs each record with the one ahead of it
        /// and creates a list of revisions.
        /// </summary>
        /// <param name="viewModels"></param>
        /// <returns></returns>
        public ItemHistoryViewModel CreateItemRevisionsByDate(List<ItemHistoryModel> viewModels)
        {
            ItemHistoryViewModel response = new ItemHistoryViewModel();

            if (viewModels.Count  > 0)
            {
                // for the first history record we want to list all values. In this case there is only a single record.
                var singleRevision = new RevisionByDate()
                {
                    Date = viewModels[0].SysStartTimeUtc,
                    User = viewModels[0].ItemAttributes[Constants.Attributes.ModifiedBy],
                    Id = Guid.NewGuid(),
                    Values = viewModels[0].ItemAttributes
                };

                // infor history often doesn't have item type set on the first record
                if (!string.IsNullOrWhiteSpace(viewModels[0].ItemTypeCode))
                {
                    singleRevision.Values[Constants.Attributes.ItemTypeCode] = viewModels[0].ItemTypeCode;
                }
                response.RevisionsByDate.Add(singleRevision);

                // diff every record with the one a head of it. The viewModels should be sorted by start date asc.
                for (int i = 0; i < viewModels.Count - 1; i++)
                {
                    // on records after the first one diff each record with the one after it. Run this on the first and second
                    // record as well so we get the diff between those two record. The first entry is special. 
                    RevisionByDate revision = this.Diff(viewModels[i], viewModels[i + 1]);
                    if (revision != null)
                    {
                        response.RevisionsByDate.Add(revision);
                    }
                }

                response.RevisionsByDate = response.RevisionsByDate.OrderBy(x => x.Date).ToList();
            }

            return response;
        }

        /// <summary>
        /// Takes two item history records and compares the attributes and returns a revision object with a list
        /// of attributes that were changed. Assumes the previous record occurs previously in time to the next record.
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RevisionByDate Diff(ItemHistoryModel previous, ItemHistoryModel next)
        {
            if(!next.ItemAttributes.ContainsKey(Constants.Attributes.ModifiedBy))
            { 
                throw new InvalidOperationException("ModifiedBy is a required attribute on all items.");
            }

            RevisionByDate response = new RevisionByDate()
            {
                Date = next.SysStartTimeUtc,
                User = next.ItemAttributes[Constants.Attributes.ModifiedBy]
            };

            // iterate all previous assigned attributes and if any don't exist in the current record mark them as removed
            Action setRemovedAttributesToRemoved = () =>
            {
                foreach (string key in previous.ItemAttributes.Keys.Except(new List<string> { Constants.Attributes.ItemTypeCode}))
                {
                    if (!next.ItemAttributes.ContainsKey(key))
                    {
                        // the attribute was in the previous record but was removed
                        response.Values[key] = "REMOVED";
                    }
                }
            };

            Action diffAttributes = () =>
            {
                foreach (string key in next.ItemAttributes.Keys.Except(new List<string> { Constants.Attributes.ItemTypeCode }))
                {
                    if (!previous.ItemAttributes.ContainsKey(key)) // the attribute is not in the previous record
                    {
                        // then take the new value
                        response.Values[key] = next.ItemAttributes[key];
                    }
                    else if (previous.ItemAttributes[key] != next.ItemAttributes[key]) // check if the value is in the previous and current record but has changed
                    {
                        // take the newer value
                        response.Values[key] = next.ItemAttributes[key];
                    }
                }
            };


            Action diffItemType = () =>
            {
                if(next.ItemTypeCode != previous.ItemTypeCode)
                {
                    response.Values[Constants.Attributes.ItemTypeCode] = next.ItemTypeCode;
                }
            };

            setRemovedAttributesToRemoved();
            diffAttributes();
            diffItemType();

            if (response.Values.Count == 0)
            {
                return null;
            }
            else
            {
                return response;
            }
        }

        // <summary>
        /// This function takes a list of ItemHierarchyClassHistoryModel and creates REMOVED manufacturer records if needed.
        /// </summary>
        /// <param name="history"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public List<ItemHierarchyClassHistoryModel> CreateManufacturerHistory (List<ItemHierarchyClassHistoryModel> history, ItemViewModel viewModel)
        {
            if (history != null && history.Any())
            {
                var historyArray = history.OrderBy(x => x.SysStartTimeUtc).ToArray();
                for (int i = 0; i < historyArray.Length - 1; i++)
                {
                    if (historyArray[i + 1].SysStartTimeUtc > historyArray[i].SysEndTimeUtc)
                    {
                        history.Add(new ItemHierarchyClassHistoryModel()
                        {
                            ItemId = history.First().ItemId,
                            HierarchyName = "Manufacturer",
                            HierarchyLineage = "REMOVED",
                            SysStartTimeUtc = historyArray[i].SysEndTimeUtc,
                            SysEndTimeUtc = historyArray[i + 1].SysStartTimeUtc
                        });
                    }
                }

                if (viewModel != null && viewModel.ManufacturerHierarchyClassId <= 0)
                {
                    history.Add(new ItemHierarchyClassHistoryModel()
                    {
                        ItemId = viewModel.ItemId,
                        HierarchyName = "Manufacturer",
                        HierarchyLineage = "REMOVED",
                        SysStartTimeUtc = history.OrderBy(x => x.SysStartTimeUtc).Last().SysEndTimeUtc,
                        SysEndTimeUtc = DateTime.MaxValue
                    });
                }
            }

            return (history ?? new List<ItemHierarchyClassHistoryModel>()).OrderBy(x => x.SysStartTimeUtc).ToList();
        }
    }
}