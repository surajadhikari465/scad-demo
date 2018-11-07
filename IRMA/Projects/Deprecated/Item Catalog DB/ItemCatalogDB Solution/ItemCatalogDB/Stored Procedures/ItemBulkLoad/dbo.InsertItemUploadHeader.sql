IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertItemUploadHeader]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertItemUploadHeader]
GO

CREATE PROCEDURE dbo.InsertItemUploadHeader
	@ItemUploadType_ID as int,
	@EmailToAddress as varchar(2000),
	@User_ID as int, 
	@ItemUploadHeader_ID as int output
AS
BEGIN

	INSERT INTO dbo.ItemUploadHeader (	
		ItemUploadType_ID, 
		EmailToAddress, 
		[User_ID], 
		UploadDateTime)
	VALUES (
		@ItemUploadType_ID, 
		@EmailToAddress, 
		@User_ID, 
		GETDATE())

	SELECT @ItemUploadHeader_ID = SCOPE_IDENTITY()
END
GO

