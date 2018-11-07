-- =============================================
-- Author:		Hussain Hashim
-- Create date: 9/19/2007
-- Description:	Returns Item Override Description based on Item Key and Store No
-- =============================================
CREATE FUNCTION [dbo].[fn_ItemOverride_ItemDescription]
(
	@Item_Key	INT,
	@Store_No	INT
)
RETURNS VARCHAR(60)
AS
BEGIN
	
	Declare @DefaultJurisdictionID		INT
	Declare @StoreJurisdictionID		INT
	Declare @Item_Description			VARCHAR(60)


	SELECT     @DefaultJurisdictionID = StoreJurisdictionID
	FROM         dbo.Item
	WHERE     (Item_Key = @Item_Key)

	SELECT     @StoreJurisdictionID = StoreJurisdictionID
	FROM         dbo.Store
	WHERE     (Store_No = @Store_No)


	IF @DefaultJurisdictionID = @StoreJurisdictionID
	BEGIN
		SELECT     @Item_Description = Item_Description
		FROM         dbo.Item
		WHERE     (Item_Key = @Item_Key)
	END
	ELSE
	BEGIN
		SELECT     @Item_Description = dbo.ItemOverride.Item_Description
		FROM         dbo.ItemOverride INNER JOIN
							  dbo.Store ON dbo.ItemOverride.StoreJurisdictionID = dbo.Store.StoreJurisdictionID
		WHERE     (dbo.ItemOverride.Item_Key = @Item_Key) AND (dbo.Store.Store_No = @Store_No)
	END


	-- Return the result of the function
	RETURN @Item_Description


END