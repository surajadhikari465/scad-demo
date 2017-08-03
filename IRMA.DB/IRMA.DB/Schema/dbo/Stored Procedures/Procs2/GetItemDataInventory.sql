CREATE PROCEDURE dbo.GetItemDataInventory
    @Item_Key int, 
    @Store_No int
AS 


-- **************************************************************************************************
-- Procedure: GetItemDataInventory
--    Author: N/A
--      Date: N/A
--
-- Description: This stored procedure is called by the ItemDAO.vb in the IRMA Client code
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/19/2012	BAS		8755	Added Discontinue field. It checks all rows of StoreItemVendor 
--								to make sure all vendors have it marked discontinue.
--								This code change was done to account for future Discontinue
--								functionality which will allow a user to discontinue at the Vendor 
--								level and not just the Store level, which is the 4.8 requirement
-- 10/22/2013	DN		13402	Added ECommerce field in the result.
-- 1/14/2016	MZ/MU	13104	Added Item UOM Override fields.
-- **************************************************************************************************

BEGIN
	SET NOCOUNT ON

	SELECT
		p.CompFlag,
		p.GrillPrint,
		p.Restricted_Hours,
		p.IBM_Discount,
		p.NotAuthorizedForSale,
		p.PosTare,
		p.AgeCode,
		p.VisualVerify,
		p.SrCitizenDiscount, 
		p.POSLinkCode,
		p.LinkedItem,
		[LinkedItemIdentifier]			= (	SELECT TOP 1 Identifier 
											FROM ItemIdentifier (nolock)
											WHERE ItemIdentifier.Item_Key = p.LinkedItem 
											ORDER BY Default_Identifier DESC),
		p.ExceptionSubTeam_No as storeSubTeam_No,
		i.SubTeam_No as itemSubTeam_No,
		p.KitchenRoute_ID,
		p.Routing_Priority,
		p.Consolidate_Price_To_Prev_Item,
		p.Print_Condiment_On_Receipt,
		p.Age_Restrict,
		[Authorized]					= ISNULL(SI.Authorized, 0),
		p.MixMatch,
		p.Discountable,
		[LastScannedUserName_DTS]		= UsersDTS.FullName,
		[LastScannedUserName_NonDTS]	= UsersNonDTS.FullName,
		p.LastScannedDate_DTS,
		p.LastScannedDate_NonDTS,
		si.Refresh,
		p.LocalItem,
		p.ItemSurcharge,
		p.ElectronicShelfTag,
		[Discontinue]					= dbo.fn_GetDiscontinueStatus (@Item_Key, @Store_No, NULL),
		[ECommerce]						= SI.ECommerce,
		iuo.Retail_Unit_ID,
		iuo.Scale_ScaleUomUnit_ID,
		iuo.Scale_FixedWeight,
		iuo.Scale_ByCount,
		sie.ItemStatusCode
	FROM 
		Price				(nolock) p
		INNER JOIN Item		(nolock) i				ON	p.Item_Key		= i.Item_Key
		LEFT JOIN StoreItem (nolock) si				ON	p.Item_Key		= si.Item_Key
														AND p.Store_No	= si.Store_No
		LEFT JOIN Users		(nolock) UsersDTS		ON	p.LastScannedUserId_DTS		= UsersDTS.User_Id
		LEFT JOIN Users		(nolock) UsersNonDTS	ON	p.LastScannedUserId_NonDTS	= UsersNonDTS.User_Id
		LEFT JOIN ItemUomOverride (nolock) iuo		ON i.Item_Key      = iuo.Item_key
														AND si.Store_No     = iuo.Store_No 
		LEFT JOIN StoreItemExtended (nolock) sie	ON p.Item_Key		= sie.Item_Key
														AND p.Store_No	= sie.Store_No
	WHERE
		p.Item_Key		= @Item_Key 
		AND p.Store_No	= @Store_No
END

SET QUOTED_IDENTIFIER OFF 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemDataInventory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemDataInventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemDataInventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemDataInventory] TO [IRMAReportsRole]
    AS [dbo];

