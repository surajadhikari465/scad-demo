CREATE PROCEDURE dbo.Scale_DeleteTare
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_Tare 
	WHERE 
		Scale_Tare_ID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteTare] TO [IRMAClientRole]
    AS [dbo];

