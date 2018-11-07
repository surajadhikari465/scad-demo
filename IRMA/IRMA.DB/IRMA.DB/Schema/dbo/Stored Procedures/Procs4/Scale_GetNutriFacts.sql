CREATE PROCEDURE dbo.Scale_GetNutriFacts AS

BEGIN

	SELECT 
		NutrifactsID, 
		Description 
	FROM 
		NutriFacts
	ORDER BY 
		Description
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetNutriFacts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetNutriFacts] TO [IRMAClientRole]
    AS [dbo];

