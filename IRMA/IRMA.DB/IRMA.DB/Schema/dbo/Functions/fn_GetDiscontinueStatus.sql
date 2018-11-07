CREATE FUNCTION [dbo].[fn_GetDiscontinueStatus]
(
	@Item_Key int,
	@Store_No int,
	@Vendor_ID int
)
RETURNS bit

--**************************************************************************
-- Function: fn_GetDiscontinueStatus
--    Author: Benjamin Sims
--      Date: 12/21/2012
--
-- Description: This function is called in various places in ItemCatalog.
--				It's purpose is to check whether or not the Item is marked
--				discontinued for all stores and/or all vendors in 
--				StoreItemVendor table.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/21/2012	BS		8755	Created.
-- 02/20/2012	BS		11081	Combined it all to just one query, and added
--								siv.DeleteDate IS NULL to filter out any
--								"deleted" rows from siv.
-- 2013-05-08	KM		12186	Use MIN instead of ALL because of some undesirable
--								behavior when empty result sets are returned;
-- 2013-05-08	KM		11081	Check WFM_Store and Mega_Store values when determining disco status;
--**************************************************************************

AS

BEGIN

	DECLARE @IsDiscontinue bit
	
	--********************************************************
	-- NOTE: @Store_No and @Vendor_ID can be NULL		
	--********************************************************
	SELECT @IsDiscontinue = MIN(CAST(siv.DiscontinueItem AS int))
								FROM
									StoreItemVendor siv (nolock)
									JOIN Store		s	(nolock) ON siv.Store_No = S.Store_No
								WHERE	
									siv.Item_Key = @Item_Key
									AND	(@Store_No	IS NULL OR siv.Store_No	= @Store_No)
									AND	(@Vendor_ID IS NULL OR siv.Vendor_ID = @Vendor_ID)
									AND	siv.DeleteDate IS NULL
									AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	
	RETURN ISNULL(@IsDiscontinue, 0)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetDiscontinueStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetDiscontinueStatus] TO [IRMAReportsRole]
    AS [dbo];

