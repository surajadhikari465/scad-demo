CREATE PROCEDURE dbo.Scale_CheckForDuplicateExtraText 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_ExtraText
WHERE 
	Description = @Description 
	AND Scale_ExtraText_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateExtraText] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateExtraText] TO [IRMAClientRole]
    AS [dbo];

