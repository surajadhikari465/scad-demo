CREATE PROCEDURE dbo.Scale_GetLabelStyles AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_LabelStyle_ID, 
		Description 
	FROM 
		Scale_LabelStyle
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelStyles] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelStyles] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelStyles] TO [IRMAClientRole]
    AS [dbo];

