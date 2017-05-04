CREATE PROCEDURE dbo.Administration_POSPush_UpdateStoreFTPConfig
	@Store_No int, 
	@FileWriterType varchar(10), 
	@IP_Address varchar(15),
	@FTP_User varchar(25),
	@FTP_Password varchar(25),
	@ChangeDirectory varchar(100),
	@Port int,
	@IsSecureTransfer bit
AS
-- Update an existing configuration record in the StoreFTPConfig table for the POS Push process.
BEGIN
   UPDATE StoreFTPConfig SET		
		IP_Address = @IP_Address,
		FTP_User = @FTP_User,
		FTP_Password = @FTP_Password,
		ChangeDirectory = @ChangeDirectory,
		Port = @Port,
		IsSecureTransfer = @IsSecureTransfer
   WHERE Store_No = @Store_No 
		AND FileWriterType = @FileWriterType
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStoreFTPConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStoreFTPConfig] TO [IRMAClientRole]
    AS [dbo];

