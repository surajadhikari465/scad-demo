if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_InsertUpdateRandomWeightType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_InsertUpdateRandomWeightType]
GO


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

