CREATE PROCEDURE dbo.Scale_GetExtraTexts AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_ExtraText_ID, 
		Scale_LabelType_ID, 
		Description,
		ExtraText
		
	FROM 
		Scale_ExtraText
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTexts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTexts] TO [IRMAClientRole]
    AS [dbo];

