SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetCatalogItems')
	BEGIN
		DROP Procedure [dbo].SOG_GetCatalogItems
	END
GO

CREATE PROCEDURE dbo.SOG_GetCatalogItems
	@CatalogID		int,
	@CatalogItemID	int,
	@StoreNo		int,
	@Order			bit,
	@Identifier		varchar(13) = NULL,
	@Description	varchar(max) = NULL,
	@SubTeamID		int = NULL,
	@Level3ID		int = NULL,
	@BrandID		int = NULL,
	@ClassID		int = NULL	
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetCatalogItems()
--    Author: Billy Blackerby
--      Date: 3/18/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of items for a specific catalog
--
-- Modification History:
-- Date			Init	Comment
-- 03/18/2009	BBB		Creation
-- 03/27/2009	BBB		Added in StoreNo and Order parameter to control StoreItem
--						authorizations
-- 04/05/2009	BBB		Added CasePack and SubTeam to output
-- 04/09/2009	BBB		Converted NatItemClass to Left Join for MA
-- 04/13/2009	BBB		Added in catch to VCH to return a false for Authorized
--						it no cost file could be found
-- 04/14/2009	BBB		Added CAST to Authorized output so it could be consumed
--						in .NET correctly
-- 04/16/2009	BBB		Converted from NatItemClass to ItemCategory
-- 04/27/2009	BBB		Added in SWITCH for N/A items to be passed back as Auth
--						value; added text output for NotAvailable column to 
--						indicate failed Authorized reasons
-- 05/11/2009	BBB		Added ELSE statements to NotAvailable column to deal with
--						items only meeting some criteria; added additional CASE
--						statment to Authorized to deal with i.Not_Available
-- 05/14/2009	BBB		Added OrderBy
-- 05/27/2009	BBB		Resolved issue with pulling VCH and not providing enough
--						parameters to return a valid cost for the item
-- 06/04/2009	BBB		Added in calls to AvgCostHistory and a CASE statement
--						to handle the different cost calls for Warehouse/Kitchen
-- 06/05/2009	BBB		Added in multiplier to AvgCost against PackSize
-- 06/10/2009	BBB		Updated call to AvgCost to utilize vendorID instead of StoreNo
-- 06/10/2009	RDS		Updated call to AvgCost to utilize Vendor's StoreNo instead of vendorID
-- 05/02/2012   MZ      Modified the Order By clause to sort the results by subteam and item description
-- 01/11/2013	DN		Using the function dbo.fn_GetDiscontinueStatus instead 
--						of the Discontinue_Item field in the Item table.
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Treat Variables
	--**************************************************************************
	IF @CatalogID = 0
		SET @CatalogID = NULL

	IF @CatalogItemID = 0
		SET @CatalogItemID = NULL
		
	IF @StoreNo = 0
		SET @StoreNo = NULL
		
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
		[CatalogID],
		[CatalogItemID],
		[ItemKey],
		[Description],
		[Identifier],
		[Class],
		[RetailUnit],
		[DistributionUnit],
		[Discontinued],
		[Level3],
		[ItemNote],
		[CasePack],
		[SubTeam],
		[SubTeamName],
		[NotAvailable],
		[Authorized],
		[Cost]		=	CASE iq.ManagedBy
							WHEN 'Kitchen' THEN
								[Cost]
							ELSE
								CASE			
									WHEN iq.CaseUpchargePct > 0 THEN
										(CasePack * AvgCostHistory) + ((iq.AvgCostHistory * ((100 + iq.CaseUpchargePct) / 100) * iq.CasePack) - (iq.AvgCostHistory * iq.CasePack))
									ELSE
										(CasePack * AvgCostHistory) + iq.CaseUpchargeAmt
								END
						END
	FROM
		(
		--**************************************************************************
		--Inner Query
		--**************************************************************************
		SELECT
			[CatalogID]			= c.CatalogID,
			[CatalogItemID]		= ci.CatalogItemID,
			[ItemKey]			= i.Item_Key,
			[Description]		= i.Item_Description,
			[Identifier]		= ii.Identifier,
			[Class]				= ic.Category_Name,
			[RetailUnit]		= iur.Unit_Name,
			[DistributionUnit]	= iud.Unit_Name,
			[Discontinued]		= dbo.fn_GetDiscontinueStatus(i.Item_Key,NULL,NULL),
			[Level3]			= ISNULL(lv3.Description, 'N/A'),
			[ItemNote]			= ISNULL(i.Not_AvailableNote,ci.ItemNotes),
			[Cost]				= vch.NetCost,
			[CasePack]			= vch.PackSize,
			[SubTeam]			= i.SubTeam_No,
			[SubTeamName]		= st.SubTeam_Name,
			[CategoryName]		= ic.Category_Name,
			[ManagedBy]			= im.Value,
			[CaseUpchargePct]	= vch.CaseUpchargePct,
			[AvgCostHistory]	= vch.AvgCostHistory,
			[CaseUpchargeAmt]	= vch.CaseUpchargeAmt,
			[NotAvailable]		= CASE 
									WHEN ISNULL(si.Authorized, 0) = 0 THEN
										'A'
									ELSE
										''
								  END
								  +
								  CASE 
									WHEN ISNULL(vch.NetCost, 0) = 0 THEN
										'C'
									ELSE
										''
								  END
								  +
								  CASE 
									WHEN ISNULL(i.Not_Available, 0) = 1 THEN
										'V'
									ELSE
										''
								  END,
			[Authorized]		= CASE 
									WHEN ISNULL(si.Authorized, 0) = 0 THEN
										CAST(0 AS bit)
									ELSE
										CASE 
											WHEN ISNULL(vch.NetCost, 0) = 0 THEN
												CAST(0 AS bit)
											ELSE
												CASE
													WHEN ISNULL(i.Not_Available, 0) = 1 THEN
														CAST(0 AS bit)
													ELSE
														CAST(1 AS bit)
												END
										END
								  END
		FROM
			[Catalog]						(nolock) c
			INNER JOIN	CatalogItem			(nolock) ci		ON	c.CatalogID					= ci.CatalogID
			INNER JOIN	Item				(nolock) i		ON	ci.ItemKey					= i.Item_Key
			INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key					= ii.Item_Key
															AND	ii.Default_Identifier		= 1
			INNER JOIN	ItemBrand			(nolock) ib		ON	i.Brand_ID					= ib.Brand_ID
			INNER JOIN	ItemUnit			(nolock) iur	ON	i.Retail_Unit_ID			= iur.Unit_ID
			INNER JOIN	ItemUnit			(nolock) iud	ON	i.Distribution_Unit_ID		= iud.Unit_ID
			INNER JOIN	SubTeam				(nolock) st		ON	i.SubTeam_No				= st.SubTeam_No
			INNER JOIN	ItemManager			(nolock) im		ON	c.ManagedByID				= im.Manager_ID
			LEFT JOIN	ItemCategory		(nolock) icat	ON	i.Category_ID				= icat.Category_ID
			LEFT JOIN	StoreItem			(nolock) si		ON	i.Item_Key					= si.Item_Key
															AND	si.Store_No					= @StoreNo
			LEFT JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	i.ProdHierarchyLevel4_ID	= lv4.ProdHierarchyLevel4_ID
			LEFT JOIN	ProdHierarchyLevel3	(nolock) lv3	ON	lv4.ProdHierarchyLevel3_ID	= lv3.ProdHierarchyLevel3_ID
			LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID			
			
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
									INNER JOIN StoreItemVendor	(nolock) siv2	ON siv2.StoreItemVendorID	= vch2.StoreItemVendorID
																				AND Store_No				= @StoreNo
																				AND Vendor_ID				= im.Vendor_ID
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
			AND ii.Identifier				LIKE	'%' + ISNULL(@Identifier, ii.Identifier) + '%'
			AND i.SubTeam_No				=		ISNULL(@SubTeamID, i.SubTeam_No)
			AND i.Item_Description			LIKE	'%' + ISNULL(@Description,'%') + '%'
			AND ib.Brand_ID					=		ISNULL(@BrandID, ib.Brand_ID)
			AND
				(
				icat.Category_ID			=		ISNULL(@ClassID, ic.Category_ID)
				OR
				icat.Category_ID			IS		NULL
				)
			AND 
				(
				lv3.ProdHierarchyLevel3_ID	=		ISNULL(@Level3ID, lv3.ProdHierarchyLevel3_ID)
				OR
				lv3.ProdHierarchyLevel3_ID	IS		NULL
				)			
		) AS iq
			
	ORDER BY
		SubTeamName,
		--CategoryName,
		--Level3,
		Description
		
    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 