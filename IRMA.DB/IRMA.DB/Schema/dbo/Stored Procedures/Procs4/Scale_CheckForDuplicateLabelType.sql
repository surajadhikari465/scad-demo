CREATE PROCEDURE dbo.Scale_CheckForDuplicateLabelType 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_LabelType
WHERE 
	Description = @Description 
	AND Scale_LabelType_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateLabelType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateLabelType] TO [IRMAClientRole]
    AS [dbo];

