if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_InsertUpdateLabelType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_InsertUpdateLabelType]
GO


CREATE PROCEDURE dbo.Scale_InsertUpdateLabelType
	@ID int,
	@Description varchar(50),
	@LinesPerLabel int,
	@Characters int
AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_LabelType 
			SET 
				Description = @Description,
				LinesPerLabel = @LinesPerLabel,
				Characters = @Characters

			WHERE 
				Scale_LabelType_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_LabelType
				(Description, LinesPerLabel, Characters)
			VALUES 
				(@Description, @LinesPerLabel, @Characters)
		END
END
GO

