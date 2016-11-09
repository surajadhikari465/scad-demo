using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using GlobalEventController.Common;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetItemsByScanCodeQueryHandlerTests
    {
        private IrmaContext context;
        private GetItemsByScanCodeQuery queryParameters;
        private GetItemsByScanCodeQueryHandler queryHandler;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IrmaContext();
            this.queryParameters = new GetItemsByScanCodeQuery();
            this.queryHandler = new GetItemsByScanCodeQueryHandler(this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetItemsByScanCodeQuery_ValidListOfScanCodes_ReturnsListOfIrmaItemModel()
        {
            // Given
            List<string> existingScanCodes = this.context.ItemIdentifier
                .Where(ii => ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0 && !ii.Item.Deleted_Item)
                .Take(3)
                .Select(ii => ii.Identifier)
                .ToList();

            var expectedIrmaItems = this.context.ItemIdentifier
                .Where(ii => existingScanCodes.Contains(ii.Identifier)
                    && ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0)
                .Select(ii => new IrmaItemModel
                {
                    Item_Key = ii.Item_Key,
                    Identifier = ii.Identifier,
                    Description = ii.Item.Item_Description,
                    SubTeamName = ii.Item.SubTeam.SubTeam_Name,
                    SubTeamNo = ii.Item.SubTeam_No,
                    RetailPack = ii.Item.Package_Desc1,
                    RetailSize = ii.Item.Package_Desc2,
                    RetailUomAbbreviation = ii.Item.ItemUnit3.Unit_Abbreviation,
                    RetailUnitAbbreviation = ii.Item.ItemUnit4.Unit_Abbreviation,
                    IsDefaultIdentifier = ii.Default_Identifier == 1
                }).ToList();

            queryParameters.ScanCodes = existingScanCodes;

            // When
            List<IrmaItemModel> actualItems = this.queryHandler.Handle(queryParameters);

            // Then
            for (int i = 0; i < expectedIrmaItems.Count; i++)
            {
                Assert.AreEqual(expectedIrmaItems[i].Item_Key, actualItems[i].Item_Key, "Item_Key does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].Identifier, actualItems[i].Identifier, "Identifier does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].Description, actualItems[i].Description, "Description does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].SubTeamName, actualItems[i].SubTeamName, "SubTeamName does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].SubTeamNo, actualItems[i].SubTeamNo, "SubTeamNo does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].RetailPack, actualItems[i].RetailPack, "RetailPack does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].RetailSize, actualItems[i].RetailSize, "RetailSize does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].RetailUomAbbreviation, actualItems[i].RetailUomAbbreviation, "RetailUomAbbreviation does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].RetailUnitAbbreviation, actualItems[i].RetailUnitAbbreviation, "RetailUnitAbbreviation does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].RetailUomIsWeightedUnit, actualItems[i].RetailUomIsWeightedUnit, "RetailUomIsWeightedUnit does not match expected.");
                Assert.AreEqual(expectedIrmaItems[i].IsDefaultIdentifier, actualItems[i].IsDefaultIdentifier, "IsDefaultIdentifier does not match expected.");
            }
        }
    }
}
