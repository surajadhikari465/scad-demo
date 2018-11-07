CREATE PROCEDURE dbo.Scale_CheckForDuplicateEatBy 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_EatBy
WHERE 
	Description = @Description 
	AND Scale_EatBy_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateEatBy] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateEatBy] TO [IRMAClientRole]
    AS [dbo];

