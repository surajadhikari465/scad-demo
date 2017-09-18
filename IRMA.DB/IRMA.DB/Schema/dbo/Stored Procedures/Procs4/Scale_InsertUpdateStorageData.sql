CREATE PROCEDURE [dbo].[Scale_InsertUpdateStorageData]
	@ID int,
	@Description varchar(50),
	@StorageData varchar(1024),
	@NEW_ID int OUTPUT
AS 
BEGIN 
	DECLARE @EnableReturnsInStorageData BIT = (SELECT FlagValue FROM InstanceDataFlags idf WHERE idf.FlagKey = 'EnableReturnsInExtraTextAndStorageData')

	IF @EnableReturnsInStorageData = 0
	BEGIN
		SET @StorageData = REPLACE(@StorageData, CHAR(9),  '')
		SET @StorageData = REPLACE(@StorageData, CHAR(10), '')
		SET @StorageData = REPLACE(@StorageData, CHAR(13), '')
	END

	IF @ID > 0 
		BEGIN
			UPDATE 
				Scale_StorageData 
			SET 
				Description = @Description,
				StorageData = @StorageData
			WHERE 
				Scale_StorageData_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO Scale_StorageData
				(Description, StorageData)
			VALUES 
				(@Description, @StorageData)
				
			SELECT @NEW_ID = SCOPE_IDENTITY()
		END
END
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateStorageData] TO [IRMAClientRole]
    AS [dbo];


GO