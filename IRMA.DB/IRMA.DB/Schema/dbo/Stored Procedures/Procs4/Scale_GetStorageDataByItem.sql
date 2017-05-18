CREATE PROCEDURE dbo.Scale_GetStorageDataByItem
	@Item_Key INT
AS
BEGIN
	SELECT TOP 1 
		   ISNULL(sd.Scale_StorageData_ID,0) AS Scale_StorageData_ID,
		   ISNULL(sd.Description,'') AS Description,
		   ISNULL(sd.StorageData,'') AS StorageData ,
		   idf.Identifier AS Identifier
	FROM ItemIdentifier idf
	LEFT JOIN ItemScale its ON idf.item_key = its.item_key
	LEFT JOIN Scale_StorageData sd ON its.Scale_StorageData_ID = sd.Scale_StorageData_ID
	WHERE  idf.Item_Key = @Item_Key
END
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetStorageDataByItem] TO [IRMAClientRole]
    AS [dbo];
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetStorageDataByItem] TO [IRSUser]
    AS [dbo];