if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_InsertUpdateLabelStyle]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_InsertUpdateLabelStyle]
GO


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

