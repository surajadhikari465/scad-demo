-- =============================================
-- Author:		Hussain Hashim
-- Create date: 9/19/2007
-- Description:	Returns Item Override Description based on Item Key and Store No
-- =============================================
CREATE FUNCTION [dbo].[fn_ItemOverride_Package_Desc2]
(
	@Item_Key	INT,
	@Store_No	INT
)
RETURNS Decimal(9,4)
AS
BEGIN
	
	Declare @DefaultJurisdictionID		INT
	Declare @StoreJurisdictionID		INT
	Declare @Package_Desc2				decimal(9,4)


	SELECT     @DefaultJurisdictionID = StoreJurisdictionID
	FROM         dbo.Item
	WHERE     (Item_Key = @Item_Key)

	SELECT     @StoreJurisdictionID = StoreJurisdictionID
	FROM         dbo.Store
	WHERE     (Store_No = @Store_No)


	IF @DefaultJurisdictionID = @StoreJurisdictionID
		BEGIN
			SELECT     @Package_Desc2 = Package_Desc2
			FROM         dbo.Item
			WHERE     (Item_Key = @Item_Key)
		END
	ELSE
		BEGIN
			SELECT     @Package_Desc2 = dbo.ItemOverride.Package_Desc2
			FROM         dbo.ItemOverride INNER JOIN
								  dbo.Store ON dbo.ItemOverride.StoreJurisdictionID = dbo.Store.StoreJurisdictionID
			WHERE     (dbo.ItemOverride.Item_Key = @Item_Key) AND (dbo.Store.Store_No = @Store_No)
		END

	-- Return the result of the function
	RETURN @Package_Desc2


END