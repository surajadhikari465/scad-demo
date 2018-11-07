CREATE PROCEDURE dbo.Scale_InsertUpdateLabelStyle
	@ID int,
	@Description varchar(50)
AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_LabelStyle 
			SET 
				Description = @Description
			WHERE 
				Scale_LabelStyle_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_LabelStyle
				(Description)
			VALUES 
				(@Description)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateLabelStyle] TO [IRMAClientRole]
    AS [dbo];

