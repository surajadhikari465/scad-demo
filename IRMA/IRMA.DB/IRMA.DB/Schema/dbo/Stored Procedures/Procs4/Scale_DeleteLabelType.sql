CREATE PROCEDURE dbo.Scale_DeleteLabelType
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_LabelType 
	WHERE 
		Scale_LabelType_ID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteLabelType] TO [IRMAClientRole]
    AS [dbo];

