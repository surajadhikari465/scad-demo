-- =============================================
-- Author:		Hussain Hashim
-- Create date: 9/19/2007
-- Description:	Returns Item Override Description based on Item Key and Store No
-- =============================================
CREATE FUNCTION [dbo].[fn_ItemOverride_Package_Desc1]
(
	@Item_Key	INT,
	@Store_No	INT
)
RETURNS decimal(9,4)
AS
BEGIN
	
	Declare @DefaultJurisdictionID		INT
	Declare @StoreJurisdictionID		INT
	Declare @Package_Desc1				decimal(9,4)

	SELECT     @DefaultJurisdictionID = StoreJurisdictionID
	FROM         dbo.Item
	WHERE     (Item_Key = @Item_Key)

	SELECT     @StoreJurisdictionID = StoreJurisdictionID
	FROM         dbo.Store
	WHERE     (Store_No = @Store_No)


	IF @DefaultJurisdictionID = @StoreJurisdictionID
		BEGIN
			SELECT     @Package_Desc1 = Package_Desc1
			FROM         dbo.Item
			WHERE     (Item_Key = @Item_Key)
		END
	ELSE
		BEGIN
			SELECT     @Package_Desc1 = dbo.ItemOverride.Package_Desc1
			FROM         dbo.ItemOverride INNER JOIN
								  dbo.Store ON dbo.ItemOverride.StoreJurisdictionID = dbo.Store.StoreJurisdictionID
			WHERE     (dbo.ItemOverride.Item_Key = @Item_Key) AND (dbo.Store.Store_No = @Store_No)
		END

	-- Return the result of the function
	RETURN @Package_Desc1

END