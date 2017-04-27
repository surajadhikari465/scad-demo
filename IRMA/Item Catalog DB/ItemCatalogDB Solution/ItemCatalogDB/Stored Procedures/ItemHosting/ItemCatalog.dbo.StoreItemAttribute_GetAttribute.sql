IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreItemAttribute_GetAttribute]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[StoreItemAttribute_GetAttribute]
GO

CREATE PROCEDURE dbo.StoreItemAttribute_GetAttribute 
	@Store_No as int,
	@Item_Key as int 
	
AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		StoreItemAttribute_ID,
		Exempt 
	FROM 
		StoreItemAttribute
	WHERE 
		Store_No = @Store_No
		AND Item_Key = @Item_Key
    
    SET NOCOUNT OFF
END

GO

