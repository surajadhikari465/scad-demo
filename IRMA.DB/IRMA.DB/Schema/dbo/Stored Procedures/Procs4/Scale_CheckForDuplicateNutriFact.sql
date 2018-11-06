CREATE PROCEDURE dbo.Scale_CheckForDuplicateNutriFact 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	NutriFacts
WHERE 
	Description = @Description 
	AND NutriFactsID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateNutriFact] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateNutriFact] TO [IRMAClientRole]
    AS [dbo];

