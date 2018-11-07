CREATE PROCEDURE dbo.Scale_CheckForDuplicateGrade 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_Grade
WHERE 
	Description = @Description 
	AND Scale_Grade_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateGrade] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateGrade] TO [IRMAClientRole]
    AS [dbo];

