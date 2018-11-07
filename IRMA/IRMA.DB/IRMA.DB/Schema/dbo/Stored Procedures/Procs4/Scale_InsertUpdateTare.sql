CREATE PROCEDURE dbo.Scale_InsertUpdateTare
	@ID int,
	@Description varchar(50),
	@Zone1 decimal(4,3),
	@Zone2 decimal(4,3),
	@Zone3 decimal(4,3),
	@Zone4 decimal(4,3),
	@Zone5 decimal(4,3),
	@Zone6 decimal(4,3),
	@Zone7 decimal(4,3),
	@Zone8 decimal(4,3),
	@Zone9 decimal(4,3),
	@Zone10 decimal(4,3) 

AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_Tare 
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
				Scale_Tare_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_Tare
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
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateTare] TO [IRMAClientRole]
    AS [dbo];

