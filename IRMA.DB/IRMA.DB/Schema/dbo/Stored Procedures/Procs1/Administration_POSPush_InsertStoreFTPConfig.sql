CREATE PROCEDURE dbo.Administration_POSPush_InsertStoreFTPConfig
	@Store_No int, 
	@FileWriterType varchar(10), 
	@IP_Address varchar(15),
	@FTP_User varchar(25),
	@FTP_Password varchar(25),
	@ChangeDirectory varchar(25),
	@Port int,
	@IsSecureTransfer bit
AS
-- INSERTs configuration record in the StoreFTPConfig table for the POS Push process.
BEGIN
   
   INSERT INTO StoreFTPConfig (Store_No, FileWriterType, IP_Address, FTP_User, FTP_Password, ChangeDirectory, Port, IsSecureTransfer)
   VALUES (@Store_No, @FileWriterType, @IP_Address, @FTP_User, @FTP_Password, @ChangeDirectory, @Port, @IsSecureTransfer)
   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertStoreFTPConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertStoreFTPConfig] TO [IRMAClientRole]
    AS [dbo];

