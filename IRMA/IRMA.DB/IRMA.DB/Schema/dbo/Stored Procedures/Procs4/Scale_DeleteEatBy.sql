CREATE PROCEDURE dbo.Scale_DeleteEatBy
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_EatBy 
	WHERE 
		Scale_EatBy_ID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteEatBy] TO [IRMAClientRole]
    AS [dbo];

