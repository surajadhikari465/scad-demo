CREATE PROCEDURE dbo.Scale_AddStorageDataToItem
	@Item_Key INT,
	@StorageDataDescription VARCHAR(50),
	@StorageData VARCHAR(1024)
AS
BEGIN
	DECLARE @storageDataId INT

	INSERT INTO Scale_StorageData(Description,  StorageData)
	VALUES(@StorageDataDescription, @StorageData)

	SET @storageDataId = SCOPE_IDENTITY()

	UPDATE ItemScale
	SET Scale_StorageData_ID = @storageDataId
	WHERE Item_Key = @Item_Key
END
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_AddstorageDataToItem] TO [IRMAClientRole]
    AS [dbo];
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_AddstorageDataToItem] TO [IRSUser]
    AS [dbo];
