if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_InsertUpdateGrade]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_InsertUpdateGrade]
GO


CREATE PROCEDURE dbo.Scale_InsertUpdateGrade
	@ID int,
	@Description varchar(50),
	@Zone1 smallint,
	@Zone2 smallint,
	@Zone3 smallint,
	@Zone4 smallint,
	@Zone5 smallint,
	@Zone6 smallint,
	@Zone7 smallint,
	@Zone8 smallint,
	@Zone9 smallint,
	@Zone10 smallint 

AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_Grade 
			SET 
				Description = @Description,
				Zone1 = @Zone1,
				Zone2 = @Zone2,
				Zone3 = @Zone3,
				Zone4 = @Zone4,
				Zone5 = @Zone5,
				Zone6 = @Zone6,
				Zone7 = @Zone7,
				Zone8 = @Zone8,
				Zone9 = @Zone9,
				Zone10 = @Zone10 
			WHERE 
				Scale_Grade_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_Grade
				(Description, Zone1, Zone2, Zone3, Zone4, Zone5,
				Zone6, Zone7, Zone8, Zone9, Zone10)
			VALUES 
				(@Description,
				@Zone1,
				@Zone2,
				@Zone3,
				@Zone4,
				@Zone5,
				@Zone6,
				@Zone7,
				@Zone8,
				@Zone9,
				@Zone10)
		END
END
GO

