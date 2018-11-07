CREATE PROCEDURE dbo.Scale_DeleteLabelFormat
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_LabelFormat 
	WHERE 
		Scale_LabelFormat_ID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteLabelFormat] TO [IRMAClientRole]
    AS [dbo];

