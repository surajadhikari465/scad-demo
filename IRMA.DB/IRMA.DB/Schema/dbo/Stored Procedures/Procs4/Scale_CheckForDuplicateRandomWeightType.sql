CREATE PROCEDURE dbo.Scale_CheckForDuplicateRandomWeightType 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_RandomWeightType
WHERE 
	Description = @Description 
	AND Scale_RandomWeightType_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateRandomWeightType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateRandomWeightType] TO [IRMAClientRole]
    AS [dbo];

