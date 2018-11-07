CREATE PROCEDURE dbo.Scale_CheckForDuplicateLabelStyle 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_LabelStyle
WHERE 
	Description = @Description 
	AND Scale_LabelStyle_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateLabelStyle] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateLabelStyle] TO [IRMAClientRole]
    AS [dbo];

