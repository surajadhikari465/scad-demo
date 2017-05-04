CREATE PROCEDURE dbo.Scale_GetLabelFormats AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_LabelFormat_ID, 
		Description 
	FROM 
		Scale_LabelFormat
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelFormats] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelFormats] TO [IRMAClientRole]
    AS [dbo];

