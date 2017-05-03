if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsItemInStoreJurisdiction]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsItemInStoreJurisdiction]
GO

Create  FUNCTION dbo.fn_IsItemInStoreJurisdiction
	(@Item_Key int,
	@Identifier varchar(13),
	@Store_No int)
RETURNS bit
AS 
BEGIN  
	DECLARE @return bit
	DECLARE @StoreJurisdictionID int

	SELECT
		@StoreJurisdictionID = StoreJurisdictionID
	FROM
		Store (nolock)
	WHERE
		Store_No = @Store_No
	
	IF EXISTS(	SELECT
			I.Item_Key
		FROM
			Item I (nolock)
			LEFT OUTER JOIN ItemOverride IOV (nolock) ON I.Item_Key = IOV.Item_Key
			LEFT OUTER JOIN ItemScaleOverride ISO (nolock) ON I.Item_Key = ISO.Item_Key
		WHERE
			I.Item_Key = @Item_Key
			AND
	--	If the Item.StoreJurisdictionID value does not match the Store.StoreJurisdictionID value...
			(	I.StoreJurisdictionID = @StoreJurisdictionID
				OR 
	--	there must be an entry in the ItemOverride table for the Item_Key and 
	--	Store.StoreJurisdictionID to include the Item-Store in the batch search results.
				(	IOV.StoreJurisdictionID = @StoreJurisdictionID
					AND
	-- If this is a scale item, there must also be an entry in the ItemScaleOverride table 
	-- for the Item_Key and Store.StoreJurisdictionID to include the Item-Store in the batch 
	-- search results.
					(	dbo.fn_IsScaleItem(@Identifier) = 0
						OR
						ISO.StoreJurisdictionID = @StoreJurisdictionID) ) ) )
		SET @return = 1
	ELSE
		SET @return = 0

	RETURN @return;
END

GO