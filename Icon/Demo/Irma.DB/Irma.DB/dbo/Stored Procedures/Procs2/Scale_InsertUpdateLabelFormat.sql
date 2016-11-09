CREATE PROCEDURE dbo.Scale_InsertUpdateLabelFormat
	@ID int,
	@Description varchar(50)
AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_LabelFormat 
			SET 
				Description = @Description
			WHERE 
				Scale_LabelFormat_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_LabelFormat
				(Description)
			VALUES 
				(@Description)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateLabelFormat] TO [IRMAClientRole]
    AS [dbo];

