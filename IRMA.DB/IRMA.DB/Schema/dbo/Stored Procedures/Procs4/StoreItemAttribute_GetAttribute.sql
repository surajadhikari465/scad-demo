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
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreItemAttribute_GetAttribute] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreItemAttribute_GetAttribute] TO [IRMAClientRole]
    AS [dbo];

