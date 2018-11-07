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
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUploadHeader] TO [IRMAClientRole]
    AS [dbo];

