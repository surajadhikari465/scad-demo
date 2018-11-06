if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_InsertUpdateEatBy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_InsertUpdateEatBy]
GO


CREATE PROCEDURE dbo.Scale_InsertUpdateEatBy
	@ID int,
	@Description varchar(50)
AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_EatBy 
			SET 
				Description = @Description
			WHERE 
				Scale_EatBy_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_EatBy
				(Description)
			VALUES 
				(@Description)
		END
END
GO

