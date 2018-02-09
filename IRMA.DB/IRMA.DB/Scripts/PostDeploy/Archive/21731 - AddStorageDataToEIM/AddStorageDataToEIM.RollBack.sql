DECLARE @UploadAttribute_Id int
DECLARE @UploadTypeAttribute_Id int

SELECT  @UploadAttribute_Id = UploadAttribute_ID FROM UploadAttribute WHERE Name = 'Storage Data'
SELECT  @UploadTypeAttribute_Id = UploadTypeAttribute_ID FROM UploadTypeAttribute WHERE UploadType_Code = 'ITEM_MAINTENANCE' AND UploadAttribute_ID = @UploadAttribute_ID
  
BEGIN TRY 
BEGIN TRANSACTION;
DELETE UploadTypeTemplateAttribute
WHERE UploadTypeAttribute_Id = @UploadTypeAttribute_Id

DELETE UploadTypeAttribute
WHERE UploadAttribute_Id = @UploadAttribute_Id

DELETE UploadAttribute
WHERE UploadAttribute_Id = @UploadAttribute_Id

COMMIT TRANSACTION; 
END TRY

BEGIN CATCH  
ROLLBACK TRANSACTION 
END CATCH;  