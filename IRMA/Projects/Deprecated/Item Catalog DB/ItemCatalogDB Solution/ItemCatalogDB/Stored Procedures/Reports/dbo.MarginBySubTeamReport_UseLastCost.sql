
/****** Object:  StoredProcedure dbo.MarginBySubTeamReport_UseLastCost    Script Date: 10/09/2006 21:21:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.MarginBySubTeamReport_UseLastCost') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.MarginBySubTeamReport_UseLastCost


/****** Object:  StoredProcedure dbo.MarginBySubTeamReport_UseLastCost    Script Date: 10/09/2006 21:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MarginBySubTeamReport_UseLastCost]
	@Store_No int ,
	@SubTeam_No int,
	@Minval int = 0,
	@Maxval int = 100,
	@Range bit = 1
AS

-- **************************************************************************
-- Procedure: MarginBySubTeamReport_UseLastCost()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/11/2013  BAS	Update i.Discontinue_Item filter in WHERE clause to
--					account for schema change. Renamed file to .sql. Coding Standards.
-- **************************************************************************

	BEGIN
	SET NOCOUNT ON
	if @Range = 1
		begin
			SELECT distinct *
			FROM (SELECT *, dbo.fn_GetMargin(Price, Multiple, Cost) as Margin
				FROM (	SELECT
							SubTeam_Name,
							Item_Description,
							Price.Store_No,
							Store.Store_Name,
							Price.Multiple,
							Price.Price,
							dbo.fn_GetCurrentNetCost(Item.Item_Key, Store.Store_No)/VCH.Package_Desc1 AS Cost, --ADDED /VCH.Package_Desc1 for Margin Calculation
							Brand_Name, 
							Identifier, 
							Item.Item_Key, 
							VCH.Package_Desc1
						FROM 
							Item (nolock)
							INNER JOIN SubTeam				(nolock) ON SubTeam.SubTeam_No		= Item.SubTeam_No
							LEFT JOIN ItemBrand				(nolock) ON Item.Brand_ID			= ItemBrand.Brand_ID
							INNER JOIN ItemIdentifier		(nolock) ON	Item.Item_Key			= ItemIdentifier.Item_Key
																		AND ItemIdentifier.Default_Identifier = 1
							INNER JOIN Price				(nolock) ON Item.Item_Key			= Price.Item_Key
							INNER JOIN Store				(nolock) ON Store.Store_No			= Price.Store_No
							INNER JOIN StoreItemVendor SIV	(nolock) ON	SIV.Item_Key			= Price.Item_Key
																		and siv.Store_no		= Price.Store_no
																		and siv.PrimaryVendor	= 1
							INNER JOIN ItemVendor iv		(nolock) ON	( iv.Item_Key			= ItemIdentifier.Item_Key 
																		and iv.Vendor_ID		= SIV.Vendor_ID)
							-- use function to retrieve only current cost records
							INNER JOIN dbo.fn_VendorCostAll(getdate()) vch ON	( vch.Item_Key		= iv.Item_Key
																				and vch.Vendor_ID	= iv.Vendor_ID 
																				and siv.Store_no	= vch.Store_no)
						WHERE
							Retail_Sale				= 1
							AND Deleted_Item		= 0
							AND	SIV.DiscontinueItem	= 0
							AND Price.Price			> 0
							AND Price.Store_No		= ISNULL(@Store_No, Price.Store_No)
							AND (WFM_Store = 1 OR Mega_Store = 1)
							AND Item.SubTeam_No		= ISNULL(@SubTeam_No, Item.SubTeam_No)
					)	AS inner_result
				WHERE Cost is not null ) as outer_result
			WHERE Margin >= @Minval and Margin < @Maxval
			ORDER BY SubTeam_Name, Identifier
		END
	ELSE
		begin
			SELECT distinct *
			FROM (	SELECT *,dbo.fn_GetMargin(Price, Multiple, Cost) as Margin
				FROM ( SELECT
							SubTeam_Name,
							Item_Description,
							Price.Store_No,
							Store.Store_Name,
							Price.Multiple,
							Price.Price,
							dbo.fn_GetCurrentNetCost(Item.Item_Key, Store.Store_No)/VCH.Package_Desc1 AS Cost, --ADDED /VCH.Package_Desc1 for Margin Calculation
							Brand_Name,
							Identifier,
							Item.Item_Key,
							VCH.Package_Desc1
						FROM 
							Item (nolock)
							INNER JOIN SubTeam				(nolock) ON SubTeam.SubTeam_No		= Item.SubTeam_No
							LEFT JOIN ItemBrand				(nolock) ON Item.Brand_ID			= ItemBrand.Brand_ID
							INNER JOIN ItemIdentifier		(nolock) ON Item.Item_Key			= ItemIdentifier.Item_Key
																		AND ItemIdentifier.Default_Identifier = 1
							INNER JOIN Price				(nolock) ON Item.Item_Key			= Price.Item_Key
							INNER JOIN Store				(nolock) ON Store.Store_No			= Price.Store_No
							INNER JOIN StoreItemVendor SIV	(nolock) ON SIV.Item_Key			= Price.Item_Key
																		and siv.Store_no		= Price.Store_no
																		and siv.PrimaryVendor	= 1
							INNER JOIN ItemVendor iv		(nolock) ON ( iv.Item_Key			= ItemIdentifier.Item_Key 
																	and iv.Vendor_ID			= SIV.Vendor_ID)
							-- use function to retrieve only current cost records
							INNER JOIN dbo.fn_VendorCostAll(getdate()) vch ON ( vch.Item_Key	= iv.Item_Key
																			and vch.Vendor_ID	= iv.Vendor_ID 
																			and siv.Store_no	= vch.Store_no)
						WHERE
							Retail_Sale				= 1
							AND Deleted_Item		= 0
							AND	SIV.DiscontinueItem	= 0
							AND Price.Price			> 0
							AND Price.Store_No		= ISNULL(@Store_No, Price.Store_No)
							AND (WFM_Store = 1 OR Mega_Store = 1)
							AND Item.SubTeam_No		= ISNULL(@SubTeam_No, Item.SubTeam_No)
						) AS inner_result
					WHERE Cost is not null
				) as outer_result
			WHERE Margin >= @Maxval or Margin < @Minval 
			ORDER BY SubTeam_Name, Identifier
		end

    SET NOCOUNT OFF
END
