
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteItemUpload]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteItemUpload]
GO


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


