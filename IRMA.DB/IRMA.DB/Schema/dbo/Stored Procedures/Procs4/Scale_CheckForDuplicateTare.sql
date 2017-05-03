CREATE PROCEDURE dbo.Scale_CheckForDuplicateTare 
@ID int, 
@Description varchar(50) 
AS 

SELECT 
	COUNT(*) AS DuplicateCount 
FROM 
	Scale_Tare
WHERE 
	Description = @Description 
	AND Scale_Tare_ID <> @ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateTare] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_CheckForDuplicateTare] TO [IRMAClientRole]
    AS [dbo];

