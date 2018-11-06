CREATE PROCEDURE dbo.Scale_GetLabelTypes AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_LabelType_ID, 
		Description,
		LinesPerLabel,
		Characters
	FROM 
		Scale_LabelType
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetLabelTypes] TO [IRMAClientRole]
    AS [dbo];

