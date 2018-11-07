CREATE PROCEDURE dbo.GetVendorItems
    @Vendor_ID int,
    @Store_No int,
    @IncludeDisco bit
AS 

--**************************************************************************
-- Function: GetVendorItems
--    Author: n/a
--      Date: n/a
--
-- Description: This function is called by VendorItems.vb in IRMA client
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/04/2013	BS		8755	Coding standards. Updated extension.
--								Updated Discontinue filter to account for
--								schema change.
--**************************************************************************

BEGIN
	DECLARE @CurrDate datetime
	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
	if @Store_No is null
	  BEGIN
		SELECT 
			i.Item_Key,
			i.Item_Description,
			ii.Identifier,
			iv.Item_ID, 
			ic.Category_Name,
			ic.Category_ID,
			i.Brand_ID, 
			ISNULL(i.DistSubTeam_No, i.SubTeam_No) as SubTeam_No,
			NULL as Store_No

		FROM
			ItemVendor					iv	(nolock)
			INNER JOIN Item				i	(nolock) ON i.Item_Key					= iv.Item_Key
			INNER JOIN ItemIdentifier	ii	(nolock) ON ii.Item_Key		= i.Item_Key
														AND ii.Default_Identifier	= 1
			LEFT JOIN ItemCategory		ic	(nolock) ON i.Category_ID				= ic.Category_ID

		WHERE
			Vendor_ID		= @Vendor_ID 
			AND @CurrDate	< ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
			AND
				(dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, @Vendor_ID)		= 0
				or dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, @Vendor_ID)	= @includeDisco)

		ORDER BY
			Item_Description
	  END
	Else
	  BEGIN
		SELECT 
			i.Item_Key,
			i.Item_Description,
			ii.Identifier,
			iv.Item_ID, 
			ic.Category_Name,
			ic.Category_ID,
			i.Brand_ID, 
			ISNULL(i.DistSubTeam_No, i.SubTeam_No) as SubTeam_No,
			SIV.Store_No

		FROM
			ItemVendor					iv	(nolock)
			INNER JOIN StoreItemVendor SIV	(nolock) ON	SIV.Item_Key		= iv.Item_Key
														AND SIV.Store_No	= @Store_No            
														AND SIV.Vendor_ID	= iv.Vendor_ID
			INNER JOIN Item				i	(nolock) ON i.Item_Key		= iv.Item_Key
			INNER JOIN ItemIdentifier	ii	(nolock) ON ii.Item_Key = i.Item_Key
														AND ii.Default_Identifier = 1
			LEFT JOIN ItemCategory		ic	(nolock) ON i.Category_ID = ic.Category_ID

		WHERE
			iv.Vendor_ID		= @Vendor_ID 
			AND SIV.Store_No	= @Store_No
			AND @CurrDate		< ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
			AND @CurrDate		< ISNULL(iv.DeleteDate, DATEADD(day, 1, @CurrDate))
			AND
				(SIV.DiscontinueItem	= 0 
				or SIV.DiscontinueItem	= @includeDisco)

		ORDER BY
			Item_Description
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItems] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItems] TO [IRMAExcelRole]
    AS [dbo];

