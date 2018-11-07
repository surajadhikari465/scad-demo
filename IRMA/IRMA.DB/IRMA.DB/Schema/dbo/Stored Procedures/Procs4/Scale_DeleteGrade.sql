CREATE PROCEDURE dbo.Scale_DeleteGrade
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_Grade 
	WHERE 
		Scale_Grade_ID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteGrade] TO [IRMAClientRole]
    AS [dbo];

