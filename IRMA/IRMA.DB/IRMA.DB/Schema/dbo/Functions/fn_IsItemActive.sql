CREATE FUNCTION [dbo].[fn_IsItemActive]
(
	@Item_Key int
	,@Identifier varchar(13)
)
RETURNS bit
AS

/*
	This function returns whether or not an item is "active".
	We return TRUE unless the item is pending delete.


    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------
    8/18/10             Tom Lux               TFS 13138        Added for v4.0 to support restore-deleted-item validation.
*/

BEGIN
	declare @retVal bit

	if exists
		 (
			select top 1 i.Item_Key
			FROM Item i (nolock)
			join ItemIdentifier ii (nolock)
				on i.item_key = ii.item_key
			WHERE
				ii.Identifier = isnull(@Identifier, ii.identifier)
				and i.item_key = isnull(@Item_key, i.item_key)
				and i.remove_item = 0
				and i.deleted_item = 0
				and ii.remove_identifier = 0
				and ii.deleted_identifier = 0
		)
		select @retVal = 1
	else
		select @retVal = 0
	
	return @retVal
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemActive] TO [IRMAPromoRole]
    AS [dbo];

