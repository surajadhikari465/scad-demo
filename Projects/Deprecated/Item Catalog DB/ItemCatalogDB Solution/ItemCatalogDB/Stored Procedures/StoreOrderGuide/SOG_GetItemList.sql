SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetItemList')
	BEGIN
		DROP Procedure [dbo].SOG_GetItemList
	END
GO

CREATE PROCEDURE dbo.SOG_GetItemList
	@CatalogID			int,
	@Identifier			varchar(13),
	@Description		varchar(max),
	@SubTeamID			int,
	@Level3ID			int,
	@BrandID			int,
	@ClassID			int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetItemList()
--    Author: Billy Blackerby
--      Date: 3/23/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of items based upon filters
--
-- Modification History:
-- Date			Init	Comment
-- 03/23/2009	BBB		Creation
-- 04/08/2009	BBB		Removed Discontinue_Item from WHERE clause; converted
--						NatItemClass to Left Join for MA
-- 04/09/2009	BBB		Treated NatItemClass in WHERE clause for MA
-- 04/11/2009	BBB		Added in join to CatalogItem to remove items already
--						in the catalog from the data output; converted Level3
--						and Brand to ID; added ClassID and VendorID
-- 04/13/2009	BBB		Removed ClassDescription
-- 04/16/2009	BBB		Converted from NatItemClass to ItemCategory
-- 04/23/2009	BBB		Added in join to StoreItemVendor so that only items with
--						matching Store/ItemManager relationships are returned;
--						removed VendorID search criteria and joins for efficiency
-- 06/04/2009	BBB		Added in calls to AvgCostHistory and a CASE statement
--						to handle the different cost calls for Warehouse/Kitchen
-- 06/05/2009	BBB		Added in multiplier to AvgCost against PackSize
-- 06/10/2009	BBB		Updated call to AvgCost to utilize vendorID instead of StoreNo
-- 06/10/2009	RDS		Updated call to AvgCost to utilize Vendor's StoreNo instead of vendorID
-- 01/11/2013	DN		Using the function dbo.fn_GetDiscontinueStatus instead 
--						of the Discontinue_Item field in the Item table.
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Treat Variables
	--**************************************************************************
	IF @Identifier = ''
		SET @Identifier = NULL
		
	IF @Description = ''
		SET @Description = NULL
	
	IF @Level3ID = 0
		SET @Level3ID = NULL
		
	IF @BrandID = 0
		SET @BrandID = NULL
	
	IF @SubTeamID = 0
		SET @SubTeamID = NULL
		
	IF @ClassID = 0
		SET @ClassID = NULL
	
	--**************************************************************************
	--Select calculated values based upon calls from inner query
	--**************************************************************************
	SELECT
		ItemKey,
		Description,
		Identifier,
		Class,
		Level3,
		RetailUnit,
		DistributionUnit,
		Discontinued,
		NotAvailable,
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
		SELECT TOP 100
			[ItemKey]			= i.Item_Key,
			[Description]		= i.Item_Description,
			[Identifier]		= ii.Identifier,
			[Class]				= ic.Category_Name,
			[Level3]			= lv3.Description,
			[RetailUnit]		= iur.Unit_Name,
			[DistributionUnit]	= iud.Unit_Name,
			[Discontinued]		= dbo.fn_GetDiscontinueStatus(i.Item_Key,NULL,NULL),
			[NotAvailable]		= i.Not_Available,
			[Cost]				= vch.NetCost,
			[ManagedBy]			= im.Value,
			[CaseUpchargePct]	= vch.CaseUpchargePct,
			[AvgCostHistory]	= vch.AvgCostHistory,
			[CaseUpchargeAmt]	= vch.CaseUpchargeAmt,
			[PackSize]			= vch.PackSize		
		FROM
			Item							(nolock) i		
			INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key					= ii.Item_Key
															AND	ii.Default_Identifier		= 1
			INNER JOIN	ItemBrand			(nolock) ib		ON	i.Brand_ID					= ib.Brand_ID
			INNER JOIN	ItemUnit			(nolock) iur	ON	i.Retail_Unit_ID			= iur.Unit_ID
			INNER JOIN	ItemUnit			(nolock) iud	ON	i.Distribution_Unit_ID		= iud.Unit_ID
			INNER JOIN	Catalog				(nolock) c		ON	c.CatalogID					= @CatalogID
			INNER JOIN	ItemManager			(nolock) im		ON	i.Manager_ID				= im.Manager_ID
			LEFT JOIN	CatalogItem			(nolock) ci		ON  c.CatalogID					= ci.CatalogID
															AND	ci.ItemKey					= i.Item_Key
			LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
			LEFT JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	i.ProdHierarchyLevel4_ID	= lv4.ProdHierarchyLevel4_ID
			LEFT JOIN	ProdHierarchyLevel3	(nolock) lv3	ON	lv4.ProdHierarchyLevel3_ID	= lv3.ProdHierarchyLevel3_ID
			CROSS APPLY
							(
								--**************************************************************************
								-- Select the latest NetCost and Average Cost for the item
								--**************************************************************************
								SELECT TOP 1 
									[NetCost]			=	(ISNULL(UnitCost, 0) + ISNULL(UnitFreight, 0)),
									[PackSize]			=	vch2.Package_Desc1,
									[AvgCostHistory]	=	IsNull(dbo.fn_AvgCostHistory(i.Item_Key, v.Store_No, i.SubTeam_No, GETDATE()), 0),
									[CaseUpchargePct]	=	IsNull(dbo.fn_CaseUpchargePct(i.SubTeam_No), 0),
									[CaseUpchargeAmt]	=	CASE IsNull(iv.CaseDistHandlingChargeOverride, 0)
																WHEN 0 THEN
																	IsNull(v.CaseDistHandlingCharge, 0)
																ELSE
																	iv.CaseDistHandlingChargeOverride
															END
								FROM
									VendorCostHistory			(nolock) vch2
									INNER JOIN StoreItemVendor	(nolock) siv2	ON	siv2.StoreItemVendorID	= vch2.StoreItemVendorID
																				AND	siv2.Item_Key			= i.Item_Key
																				AND siv2.Vendor_ID			= im.Vendor_ID
																				AND siv2.Store_No			IN (SELECT StoreNo FROM CatalogStore WHERE CatalogID = @CatalogID)
									INNER JOIN Vendor			(nolock) v		ON  v.Vendor_ID				= siv2.Vendor_ID
									INNER JOIN ItemVendor		(nolock) iv		ON	iv.Vendor_ID			= siv2.Vendor_ID
																				AND	iv.Item_Key				= siv2.Item_Key
								WHERE 
									StartDate			<=	GETDATE()
									AND EndDate			>=	GETDATE()
									AND siv2.DeleteDate IS	NULL
								ORDER BY 
									VendorCostHistoryID DESC
							) vch
		WHERE
			i.Remove_Item					=		0
			AND i.Deleted_Item				=		0
			AND ii.Deleted_Identifier		=		0
			AND	ci.CatalogItemID			IS		NULL
			AND ii.Identifier				=		ISNULL(@Identifier, ii.Identifier)
			AND i.SubTeam_No				=		ISNULL(@SubTeamID, i.SubTeam_No)
			AND i.Item_Description			LIKE	'%' + ISNULL(@Description,'%') + '%'
			AND ib.Brand_ID					=		ISNULL(@BrandID, ib.Brand_ID)
			AND i.Manager_ID				=		c.ManagedByID
			AND
				(
				ic.Category_ID				=		ISNULL(@ClassID, ic.Category_ID)
				OR
				ic.Category_ID				IS		NULL
				)
			AND 
				(
				lv3.ProdHierarchyLevel3_ID	=		ISNULL(@Level3ID, lv3.ProdHierarchyLevel3_ID)
				OR
				lv3.ProdHierarchyLevel3_ID	IS		NULL
				)
		) AS iq
					
    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 