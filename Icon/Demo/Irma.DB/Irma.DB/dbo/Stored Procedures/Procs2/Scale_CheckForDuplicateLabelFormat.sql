CREATE PROCEDURE dbo.Scale_CheckForDuplicateLabelFormat 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_LabelFormat
WHERE 
	Description = @Description 
	AND Scale_LabelFormat_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateLabelFormat] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateLabelFormat] TO [IRMAClientRole]
    AS [dbo];

