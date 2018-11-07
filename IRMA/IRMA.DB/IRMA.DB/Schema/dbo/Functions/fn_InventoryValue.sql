-- =============================================
-- Author:		Rick Kelleher
-- Create date: 9/27/2007
-- Description:	Returns the core data table for the various Inventory Value reports
--				It was the old dbo.InventoryBalance SP

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
-- =============================================

CREATE FUNCTION [dbo].[fn_InventoryValue]
(
    @BusUnit int,
    @TeamNo int,
    @SubTeam_No int,
    @categoryID int,
	@Level3 int,
	@Level4 int,
	@Identifier varchar(13) --Change the datatype by sekhara to fix the bug 6109.
)
/*

grant exec on dbo.fn_InventoryValue to IRMAClientRole
grant exec on dbo.fn_InventoryValue to IRMAReportsRole
grant exec on dbo.fn_InventoryValue to IRMAExcelRole

*/
RETURNS @Table TABLE 
(
	BusUnit int NULL,
	Team int NULL,
	SubTeam int NULL,
	Item_key int NOT NULL, --To Fix 6481
	Identifier varchar(13)   NOT NULL,
	Item_Description varchar(60)   NOT NULL,
	PackSize decimal(9, 4) NOT NULL,
	Unit_Name varchar(25)   NOT NULL,
	Unit_Abbreviation varchar(5)   NULL,
	CaseCountOnHand decimal(38, 4) NOT NULL,
	CaseCountNotAvail int NOT NULL,
	LandedCaseCost decimal(20, 8) NULL,
	ExtLandedCost decimal(38, 6) NULL,
	CaseUpchargeAmt decimal(36, 16) NULL,
	CaseUpchargePct decimal(9, 4) NULL,
	LoadedCaseCost decimal(35, 16) NULL,
	ExtLoadedCost decimal(38, 6) NULL
)
AS
--**************************************************************************
-- Function: fn_InventoryValue
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
BEGIN
--    DECLARE @CaseUpchargePct decimal(9,4)
 
--    SELECT @CaseUpchargePct = Distribution_Markup 
--    FROM ZoneSupply (NOLOCK) 
--    WHERE SubTeam_No = @SubTeam_No AND
--          FromZone_ID = (SELECT Zone_ID FROM Store (NOLOCK) WHERE Store_No = @TeamNo) AND
--          ToZone_ID = (SELECT Zone_ID FROM Store (NOLOCK) WHERE Store_No = @BusUnit)

	-- By business rules (per Lawrence Priest 1/24/08) the CaseUpchargePct (Distribution_Markup)
	-- will be one amount across the region, i.e. the same for all zones, 
	-- but it is unknown at this time what (or how many) records there will
	-- be in the ZoneSupply table. So taking the MAX() (or MIN() for that matter) of Distribution_Markup
	-- will still give the correct result per these business rules.
	--			Rick Kelleher

--  Commented to fix the bug 6623.
--	SELECT @CaseUpchargePct = MAX(Distribution_Markup)
--		FROM ZoneSupply (NOLOCK) 
--		WHERE SubTeam_No = @SubTeam_No 

-- Changed the code in the following places to fix bug 6623.
-- To fetch the packsize from Vendor Cost History table.
-- Handled the code to fectch the results even in case of NULL values for subteam. 
-- Used fn_CaseUpchargePct function to fetch the CaseUpchargePct value.
-- Used Vendor packsize in caliculations.


    INSERT @Table
		SELECT DISTINCT
			@BusUnit as BusUnit,
			SubTeam.Team_No as Team,
			T2.SubTeam_No as SubTeam, 
			Item.Item_Key as Item_Key, --To Fix 6481
			ItemIdentifier.Identifier, 
			Item.Item_Description, 
			Isnull(dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@BusUnit),0) AS PackSize,  -- Changed from Iteam.package_desc1,
			ItemUnit.Unit_Name,
			ItemUnit.Unit_Abbreviation,
			ISNULL(T2.CasesOnHand, 0) as CaseCountOnHand,
			0 as CaseCountNotAvail,
			ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @BusUnit, isnull(@SubTeam_No,T2.SubTeam_No), 
						GETDATE()), 0) * Isnull(dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@BusUnit),0) AS LandedCaseCost, 
			ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @BusUnit, isnull(@SubTeam_No,T2.SubTeam_No), 
						GETDATE()), 0) * ISNULL(T2.UnitsOnHand, 0) AS ExtLandedCost,
			((((ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @BusUnit, isnull(@SubTeam_No,T2.SubTeam_No), 
						GETDATE()), 0) * (100 + Isnull(dbo.fn_CaseUpchargePct(T2.SubTeam_No),0))) / 100) * Isnull(dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@BusUnit),0))
					- 
				(ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @BusUnit, isnull(@SubTeam_No,T2.SubTeam_No), 
						GETDATE()), 0) * Isnull(dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@BusUnit),0))) as CaseUpchargeAmt,
			Isnull(dbo.fn_CaseUpchargePct(T2.SubTeam_No),0) as CaseUpchargePct,
			((ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @BusUnit, isnull(@SubTeam_No,T2.SubTeam_No), 
						GETDATE()), 0) * (100 + Isnull(dbo.fn_CaseUpchargePct(T2.SubTeam_No),0))) / 100) * Isnull(dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@BusUnit),0) 
						AS LoadedCaseCost,
			((ISNULL(T2.CasesOnHand, 0) 
					+ 
				0) -- CaseCountNotAvail) 
					* 
				((ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @BusUnit, isnull(@SubTeam_No,T2.SubTeam_No), 
						GETDATE()), 0) * (100 + Isnull(dbo.fn_CaseUpchargePct(T2.SubTeam_No),0))) / 100) * Isnull(dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@BusUnit),0)) 
				as ExtLoadedCost
		FROM Item (nolock)
		INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key 
							AND ItemIdentifier.Default_Identifier = 1
		LEFT JOIN ItemCategory (nolock) ON Item.Category_ID = ItemCategory.Category_ID
		INNER JOIN ItemUnit on item.Package_Unit_ID = ItemUnit.Unit_ID
		LEFT JOIN 
			(
			SELECT 
				ItemHistory.Item_Key, 
				ItemHistory.SubTeam_No, 
				Item.Package_Desc1,
				SUM( 
					CASE WHEN ISNULL(ItemHistory.Quantity, 0) > 0 
						THEN ItemHistory.Quantity / CASE WHEN Package_Desc1 <> 0 THEN Package_Desc1 ELSE 1 END
						ELSE ISNULL(ItemHistory.Weight, 0) / 
									CASE WHEN Package_Desc1 * Package_Desc2 <> 0 
										THEN (Package_Desc1 * Package_Desc2) ELSE 1 END 
					END * ItemAdjustment.Adjustment_Type) AS CasesOnHand,
				SUM( ISNULL(ItemHistory.Quantity, 0) 
							+ ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) AS UnitsOnHand
				FROM 
					ItemHistory (nolock) 
					INNER JOIN OnHand (nolock) ON OnHand.Item_Key = ItemHistory.Item_Key 
									AND OnHand.Store_No = ItemHistory.Store_No 
									AND OnHand.SubTeam_No = ItemHistory.SubTeam_No
					INNER JOIN	ItemAdjustment (nolock)
									ON (ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID)
					INNER JOIN Item (nolock)  ON (Item.Item_Key = ItemHistory.Item_Key)
					INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key 
									AND ItemIdentifier.Default_Identifier = 1				
			   WHERE 
					ItemHistory.Store_No = @BusUnit 
					AND ItemHistory.SubTeam_No = ISNULL(@SubTeam_No, ItemHistory.SubTeam_No)
					AND DateStamp >= OnHand.LastReset
					-- The following line changed by sekhara to fix the bug 6109.(old code as AND ItemIdentifier.Identifier_ID = @Identifier)
					AND ItemIdentifier.Identifier = ISNULL(@Identifier,ItemIdentifier.Identifier)
			   GROUP BY ItemHistory.Item_Key, ItemHistory.SubTeam_No, Item.Package_Desc1
			   HAVING SUM(ISNULL(ItemHistory.Quantity, 0) 
									+ ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) <> 0
			   ) T2 ON (Item.Item_Key = T2.Item_Key) 

		INNER JOIN SubTeam  (nolock) ON T2.SubTeam_No = SubTeam.SubTeam_No
		INNER JOIN ProdHierarchyLevel3 ON Item.Category_ID =  ProdHierarchyLevel3.Category_ID 
			AND ProdHierarchyLevel3.ProdHierarchyLevel3_ID = ISNULL(@Level3, ProdHierarchyLevel3.ProdHierarchyLevel3_ID)
		INNER JOIN ProdHierarchyLevel4 ON ProdHierarchyLevel3.ProdHierarchyLevel3_ID =  ProdHierarchyLevel4.ProdHierarchyLevel3_ID 
			AND ProdHierarchyLevel4.ProdHierarchyLevel4_ID = ISNULL(@Level4, ProdHierarchyLevel4.ProdHierarchyLevel4_ID)
		WHERE Deleted_Item = 0 AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, @BusUnit, NULL) = 0
			AND SubTeam.Team_No = ISNULL(@TeamNo, SubTeam.Team_No)
			AND Item.Category_ID = ISNULL(@CategoryID, Item.Category_ID)
			AND Item.SubTeam_No = CASE WHEN T2.SubTeam_No IS NULL 
									THEN @SubTeam_No 
									ELSE Item.SubTeam_No END  									      
	--		AND (ItemIdentifier.IdentifierType = 'S' or ItemIdentifier.IdentifierType = 's')
	RETURN 
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_InventoryValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_InventoryValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_InventoryValue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_InventoryValue] TO [IRMAExcelRole]
    AS [dbo];

