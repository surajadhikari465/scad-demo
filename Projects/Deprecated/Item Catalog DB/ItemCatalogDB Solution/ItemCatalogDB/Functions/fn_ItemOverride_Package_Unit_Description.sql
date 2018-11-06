/****** Object:  UserDefinedFunction [dbo].[fn_ItemOverride_Package_Unit_Description]    Script Date: 09/20/2007 14:59:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_ItemOverride_Package_Unit_Description')
    DROP FUNCTION fn_ItemOverride_Package_Unit_Description
GO

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




GO