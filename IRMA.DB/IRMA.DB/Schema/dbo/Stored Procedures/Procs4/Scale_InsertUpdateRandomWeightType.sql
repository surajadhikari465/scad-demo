CREATE PROCEDURE dbo.Scale_InsertUpdateRandomWeightType
	@ID int,
	@Description varchar(50)
AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_RandomWeightType
			SET 
				Description = @Description
			WHERE 
				Scale_RandomWeightType_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_RandomWeightType
				(Description)
			VALUES 
				(@Description)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateRandomWeightType] TO [IRMAClientRole]
    AS [dbo];

