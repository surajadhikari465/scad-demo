CREATE PROCEDURE dbo.DeleteItemUpload
@ItemUploadHeader_ID int
AS 

DELETE 
FROM 
	ItemUploadDetail
WHERE 
	ItemUploadHeader_ID = @ItemUploadHeader_ID

DELETE 
FROM 
	ItemUploadHeader
WHERE 
	ItemUploadHeader_ID = @ItemUploadHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemUpload] TO [IRMAClientRole]
    AS [dbo];

