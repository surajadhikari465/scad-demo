CREATE PROCEDURE dbo.Scale_UpdateStorageData
	@Scale_StorageData_ID INT,
	@StorageDataDescription VARCHAR(50),
	@StorageData VARCHAR(1024)
AS
BEGIN
	UPDATE Scale_StorageData
	SET Description = @StorageDataDescription,
		StorageData = @StorageData
	WHERE Scale_StorageData_ID = @Scale_StorageData_ID
END
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_UpdateStorageData] TO [IRMAClientRole]
    AS [dbo];
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_UpdateStorageData] TO [IRSUser]
    AS [dbo];