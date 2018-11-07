-- =============================================
-- Author:		Hussain Hashim
-- Create date: 9/19/2007
-- Description:	Returns Item Override Description based on Item Key and Store No
-- =============================================
CREATE FUNCTION [dbo].[fn_ItemOverride_Package_Unit_Description]
(
	@Item_Key	INT,
	@Store_No	INT
)
RETURNS VARCHAR(60)
AS
BEGIN
	
	Declare @DefaultJurisdictionID		INT
	Declare @StoreJurisdictionID		INT
	Declare @Package_Unit_ID			INT
    Declare @Package_Unit_Description   VARCHAR(60)


	SELECT     @DefaultJurisdictionID = StoreJurisdictionID
	FROM         dbo.Item
	WHERE     (Item_Key = @Item_Key)

	SELECT     @StoreJurisdictionID = StoreJurisdictionID
	FROM         dbo.Store
	WHERE     (Store_No = @Store_No)


	IF @DefaultJurisdictionID = @StoreJurisdictionID
		BEGIN
			SELECT     @Package_Unit_ID = Package_Unit_ID
			FROM         dbo.Item
			WHERE     (Item_Key = @Item_Key)
		END
	ELSE
		BEGIN
			SELECT     @Package_Unit_ID = dbo.ItemOverride.Package_Unit_ID
			FROM         dbo.ItemOverride INNER JOIN
								  dbo.Store ON dbo.ItemOverride.StoreJurisdictionID = dbo.Store.StoreJurisdictionID
			WHERE     (dbo.ItemOverride.Item_Key = @Item_Key) AND (dbo.Store.Store_No = @Store_No)
		END

   SELECT @Package_Unit_Description = dbo.ItemUnit.Unit_Name 
    		FROM dbo.ItemUnit WHERE (Unit_ID = @Package_Unit_ID)

	-- Return the result of the function
	RETURN @Package_Unit_Description


END