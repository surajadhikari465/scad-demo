CREATE PROCEDURE dbo.Scale_GetEatBy AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_EatBy_ID, 
		Description 
	FROM 
		Scale_EatBy
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetEatBy] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetEatBy] TO [IRMAClientRole]
    AS [dbo];

