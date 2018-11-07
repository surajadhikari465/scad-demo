CREATE PROCEDURE [dbo].[SOG_PrintCatalog]
	@CatalogID		int,
	@StoreNo		int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_PrintCatalog()
--    Author: Billy Blackerby
--      Date: 4/3/2009
--
-- Description:
-- Utilized by SSRS to return data consumed by SOG_PrintCatalog.RDL
--
-- Modification History:
-- Date			Init	Comment
-- 04/03/2009	BBB		Creation
-- 04/14/2009	BBB		Added Left Join to NatItemClass for MA
-- 05/26/2009	BBB		Added Order By from SOG_GetCatalogItems and required
--						tables via join; modified Class to be from ItemCategory
-- 05/27/2009	BBB		Resolved issue with pulling VCH and not providing enough
--						parameters to return a valid cost for the item
-- 06/04/2009	BBB		Added in calls to AvgCostHistory and a CASE statement
--						to handle the different cost calls for Warehouse/Kitchen;
--						added Store parameter and its value to the VCH call
-- 06/05/2009	BBB		Added in multiplier to AvgCost against PackSize
-- 06/10/2009	BBB		Updated call to AvgCost to utilize vendorID instead of StoreNo
-- 06/10/2009	RDS		Updated call to AvgCost to utilize Vendor's StoreNo instead of vendorID
-- 07/17/2012	td		Updated to sort like SOG_GetCatalogItem.sql
-- 01/11/2013	DN		Using the function dbo.fn_GetDiscontinueStatus instead 
--						of the Discontinue_Item field in the Item table.
-- 09/13/2013   MZ      TFS #13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- **************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON    
	--**************************************************************************
	--Select calculated values based upon calls from inner query
	--**************************************************************************
	SELECT
		[CatalogID],
		[CatalogCode],
		[CatalogDesc],
		[CatalogItemID],
		[ItemKey],
		[Description],
		[Identifier],
		[SubTeam],
		[SubTeamName],
		[Class],
		[Level3],
		[RetailUnit],
		[DistributionUnit],
		[Discontinued],
		[NotAvailable],
		[ItemNote],
		[PackSize],
		[Cost]		=	CASE iq.ManagedBy
							WHEN 'Kitchen' THEN
								[Cost]
							ELSE
								CASE			
									WHEN iq.CaseUpchargePct > 0 THEN
										(PackSize * AvgCostHistory) + ((iq.AvgCostHistory * ((100 + iq.CaseUpchargePct) / 100) * iq.PackSize) - (iq.AvgCostHistory * iq.PackSize))
									ELSE
										(PackSize * AvgCostHistory) + iq.CaseUpchargeAmt
								END
						END
	FROM
		(	
		--**************************************************************************
		--Inner Query
		--**************************************************************************
		SELECT
			[CatalogID]			= c.CatalogID,
			[CatalogCode]		= c.CatalogCode,
			[CatalogDesc]		= c.Description,
			[CatalogItemID]		= ci.CatalogItemID,
			[ItemKey]			= i.Item_Key,
			[Description]		= i.Item_Description,
			[Identifier]		= ii.Identifier,
			[SubTeam]			= i.SubTeam_No,
			[SubTeamName]		= st.SubTeam_Name,
			[Class]				= ic.Category_Name,
			[Level3]			= ISNULL(lv3.Description, 'N/A'),
			[RetailUnit]		= iur.Unit_Name,
			[DistributionUnit]	= iud.Unit_Name,
			[Discontinued]		= dbo.fn_GetDiscontinueStatus(i.Item_Key,NULL,NULL),
			[NotAvailable]		= i.Not_Available,
			[ItemNote]			= ci.ItemNotes,
			[Cost]				= vch.NetCost,
			[PackSize]			= vch.PackSize,
			[CaseUpchargePct]	= vch.CaseUpchargePct,
			[CaseUpchargeAmt]	= vch.CaseUpchargeAmt,
			[AvgCostHistory]	= vch.AvgCostHistory,
			[ManagedBy]			= im.Value,
			[CategoryName]		= ic.Category_Name
		FROM
			[Catalog]						(nolock) c
			INNER JOIN	CatalogItem			(nolock) ci		ON	c.CatalogID					= ci.CatalogID
			INNER JOIN	Item				(nolock) i		ON	ci.ItemKey					= i.Item_Key
			INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key					= ii.Item_Key
															AND	ii.Default_Identifier		= 1
			INNER JOIN	ItemUnit			(nolock) iur	ON	i.Retail_Unit_ID			= iur.Unit_ID
			INNER JOIN	ItemUnit			(nolock) iud	ON	i.Distribution_Unit_ID		= iud.Unit_ID
			INNER JOIN	SubTeam				(nolock) st		ON	i.SubTeam_No				= st.SubTeam_No
			INNER JOIN	ItemManager			(nolock) im		ON	c.ManagedByID				= im.Manager_ID
			LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
			LEFT JOIN	NatItemClass		(nolock) nic	ON	i.ClassId					= nic.ClassID
			LEFT JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	i.ProdHierarchyLevel4_ID	= lv4.ProdHierarchyLevel4_ID
			LEFT JOIN	ProdHierarchyLevel3	(nolock) lv3	ON	lv4.ProdHierarchyLevel3_ID	= lv3.ProdHierarchyLevel3_ID
			OUTER APPLY
							(
							--**************************************************************************
							-- Select the latest NetCost and Average Cost for the item
							--**************************************************************************
							SELECT TOP 1 
								[NetCost]			=	(ISNULL(UnitCost, 0) + ISNULL(UnitFreight, 0)),
								[PackSize]			=	vch2.Package_Desc1,
								[AvgCostHistory]	=	IsNull(dbo.fn_AvgCostHistory(i.Item_Key, v.Store_No, st.SubTeam_No, GETDATE()), 0),
								[CaseUpchargePct]	=	IsNull(dbo.fn_CaseUpchargePct(st.SubTeam_No), 0),
								[CaseUpchargeAmt]	=	CASE IsNull(iv.CaseDistHandlingChargeOverride, 0)
															WHEN 0 THEN
																IsNull(v.CaseDistHandlingCharge, 0)
															ELSE
																iv.CaseDistHandlingChargeOverride
														END
							FROM
								VendorCostHistory			(nolock) vch2
								INNER JOIN StoreItemVendor	(nolock) siv2	ON	siv2.StoreItemVendorID	= vch2.StoreItemVendorID
																			AND siv2.Vendor_ID			= im.Vendor_ID
																			AND siv2.Store_No			= @StoreNo
								INNER JOIN Vendor			(nolock) v		ON  v.Vendor_ID				= siv2.Vendor_ID
								INNER JOIN ItemVendor		(nolock) iv		ON	iv.Vendor_ID			= siv2.Vendor_ID
																			AND	iv.Item_Key				= siv2.Item_Key
							WHERE 
								StartDate			<=	GETDATE()
								AND EndDate			>=	GETDATE()
								AND siv2.Item_Key	=	i.Item_Key
								AND siv2.DeleteDate IS	NULL
							ORDER BY 
								VendorCostHistoryID DESC
							) vch
		WHERE
			c.CatalogID	= @CatalogID
		) AS iq
	ORDER BY
		SubTeamName,
		Description

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_PrintCatalog] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_PrintCatalog] TO [IRMASLIMRole]
    AS [dbo];

