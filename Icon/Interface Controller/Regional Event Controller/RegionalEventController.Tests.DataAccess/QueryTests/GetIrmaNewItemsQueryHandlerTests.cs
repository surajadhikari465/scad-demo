using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.Entity;
using RegionalEventController.DataAccess.Queries;
using Irma.Framework;
using System.Collections.Generic;
using Icon.Testing.Builders;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Models;
using Icon.Framework;
using Irma.Testing.Builders;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetIrmaNewItemsQueryHandlerTests
    {
        private IrmaContext context;
        private IconContext iconContext;
        private GetIrmaNewItemsQuery query;
        private GetIrmaNewItemsQueryHandler handler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            Cache.brandNameToBrandAbbreviationMap?.Clear();
            Cache.defaultCertificationAgencies?.Clear();
            Cache.itemSbTeamEventEnabledRegions?.Clear();
            Cache.nationalClassCodeToNationalClassId?.Clear();
            Cache.taxCodeToTaxId?.Clear();

            this.iconContext = new IconContext();
            this.context = new IrmaContext();
            this.query = new GetIrmaNewItemsQuery();
            this.handler = new GetIrmaNewItemsQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetIrmaNewItems_ChangeQueueHasItems_ReturnsIrmaNewItemObjectList()
        {
            // Given
            string region = this.context.Region.First().RegionCode;
            StartupOptions.Instance = 20;
            int numberOfEvents = AddIconItemChangeQueue(StartupOptions.Instance);
            int defaultOrganicAgencyId = this.iconContext.HierarchyClassTrait
                                        .Where(hct => hct.Trait.traitCode == TraitCodes.DefaultCertificationAgency
                                            && hct.traitValue == CertificationAgencyType.Organic)
                                        .Select(hct => hct.hierarchyClassID).FirstOrDefault();
            Cache.defaultCertificationAgencies.Add(CertificationAgencyType.Organic, defaultOrganicAgencyId);

            List<IrmaNewItem> expected = (from c in this.context.IconItemChangeQueue
                                          join i in this.context.Item on c.Item_Key equals i.Item_Key
                                          join ii in this.context.ItemIdentifier on c.Identifier equals ii.Identifier
                                          join u in context.ItemUnit on i.Package_Unit_ID equals u.Unit_ID
                                          join s in context.SubTeam on i.SubTeam_No equals s.SubTeam_No
                                          from nc in context.NatItemClass
                                                  .Where(nic => nic.ClassID == i.ClassID)
                                                  .DefaultIfEmpty()
                                          where
                                                (c.InProcessBy == StartupOptions.Instance.ToString()
                                                && ii.Deleted_Identifier == 0
                                                && ii.Remove_Identifier == 0
                                                && !c.Item.Deleted_Item
                                                && c.Item.Remove_Item == 0
                                                && i.Item_Key == ii.Item_Key)
                                          select new IrmaNewItem
                                          {
                                              RegionCode = region,
                                              IrmaTaxClass = i.TaxClass.TaxClassDesc,
                                              IrmaNationalClass = nc != null ? nc.ClassID : 99999,
                                              QueueId = c.QID,
                                              IrmaItemKey = c.Item_Key,
                                              Identifier = c.Identifier,
                                              IrmaItem = new IRMAItem
                                              {
                                                  regioncode = region,
                                                  identifier = ii.Identifier,
                                                  defaultIdentifier = ii.Default_Identifier == 1,
                                                  brandName = i.ItemBrand.Brand_Name.Trim(),
                                                  itemDescription = i.Item_Description.ToUpper(),
                                                  posDescription = i.POS_Description.ToUpper(),
                                                  packageUnit = (int)i.Package_Desc1,
                                                  retailSize = i.Package_Desc2,
                                                  retailUom = u.Unit_Abbreviation,
                                                  foodStamp = i.Food_Stamps,
                                                  posScaleTare = 0.0M,
                                                  giftCard = false,
                                                  departmentSale = false,
                                                  insertDate = DateTime.Now,
                                                  nationalClassID = nc.ClassID,
                                                  OrganicAgencyId = i.Organic ? defaultOrganicAgencyId : (int?)null
                                              }
                                          }).ToList();

            // When
            List<IrmaNewItem> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(numberOfEvents, expected.Count);
            Assert.AreEqual(numberOfEvents, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].RegionCode, actual[i].RegionCode, "The RegionCode does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaTaxClass, actual[i].IrmaTaxClass, "The IrmaTaxClass does not match the expected value.");
                Assert.AreEqual(expected[i].QueueId, actual[i].QueueId, "The QueueId does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItemKey, actual[i].IrmaItemKey, "The IrmaItemKey does not match the expected value.");
                Assert.AreEqual(expected[i].Identifier, actual[i].Identifier, "The Identifier does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.regioncode, actual[i].IrmaItem.regioncode, "The IrmaItem.regioncode does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.identifier, actual[i].IrmaItem.identifier, "The IrmaItem.identifier does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.defaultIdentifier, actual[i].IrmaItem.defaultIdentifier, "The IrmaItem.defaultIdentifier does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.brandName, actual[i].IrmaItem.brandName, "The IrmaItem.brandName does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.itemDescription, actual[i].IrmaItem.itemDescription, "The IrmaItem.itemDescription does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.posDescription, actual[i].IrmaItem.posDescription, "The IrmaItem.posDescription does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.packageUnit, actual[i].IrmaItem.packageUnit, "The IrmaItem.packageUnit does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.retailSize, actual[i].IrmaItem.retailSize, "The IrmaItem.retailSize does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.retailUom, actual[i].IrmaItem.retailUom, "The IrmaItem.retailUom does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.foodStamp, actual[i].IrmaItem.foodStamp, "The IrmaItem.foodStamp does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.posScaleTare, actual[i].IrmaItem.posScaleTare, "The IrmaItem.posScaleTare does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.giftCard, actual[i].IrmaItem.giftCard, "The IrmaItem.giftCard does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.departmentSale, actual[i].IrmaItem.departmentSale, "The IrmaItem.departmentSale does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.nationalClassID, actual[i].IrmaNationalClass, "The NationalClassName does not match the expected value.");
                Assert.AreEqual(expected[i].IrmaItem.OrganicAgencyId, actual[i].IrmaItem.OrganicAgencyId, "The IrmaItem.OrganicAgencyId does not match the expected value.");
            }
        }

        private int AddIconItemChangeQueue(int instance)
        {
            //Union results with Items that have null ClassIDs in order to test getting null NationalClasses
            var changeQueues = this.context
                   .ItemIdentifier
                   .Where(ii => ii.Deleted_Identifier == 0 
                       && ii.Remove_Identifier == 0
                       && ii.Item.Remove_Item == 0
                       && !ii.Item.Deleted_Item
                       && ii.Item.ClassID != null
                       && ii.Item.Organic == true)
                   .Take(3)
                   .Union(this.context
                        .ItemIdentifier
                        .Where(ii => ii.Deleted_Identifier == 0
                            && ii.Remove_Identifier == 0
                            && ii.Item.Remove_Item == 0
                            && !ii.Item.Deleted_Item
                            && ii.Item.ClassID == null
                            && ii.Item.Organic == false)
                        .Take(3))
                   .ToList()
                   .Select(ii => new TestIconItemChangeQueueBuilder()
                                   .WithItemKey(ii.Item_Key)
                                   .WithIdentifier(ii.Identifier)
                                   .WithInProcessBy(instance.ToString())
                                   .Build());

            this.context.IconItemChangeQueue.AddRange(changeQueues);
            this.context.SaveChanges();

            return changeQueues.Count();
        }
    }
}
