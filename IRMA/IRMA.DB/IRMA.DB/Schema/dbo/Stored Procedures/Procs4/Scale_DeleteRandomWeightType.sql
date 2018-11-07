CREATE PROCEDURE dbo.Scale_DeleteRandomWeightType
	@ID int
AS 
BEGIN 
	DELETE  
		Scale_RandomWeightType 
	WHERE 
		Scale_RandomWeightType_ID = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteRandomWeightType] TO [IRMAClientRole]
    AS [dbo];

