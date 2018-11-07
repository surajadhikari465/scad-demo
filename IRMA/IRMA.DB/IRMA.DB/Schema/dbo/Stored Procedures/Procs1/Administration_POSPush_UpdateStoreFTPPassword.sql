CREATE PROCEDURE dbo.Administration_POSPush_UpdateStoreFTPPassword
	@IP_Address varchar(15),
	@FTP_Password varchar(25)
AS
BEGIN
   UPDATE StoreFTPConfig SET		
		FTP_Password = @FTP_Password
   WHERE IP_Address = @IP_Address 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStoreFTPPassword] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStoreFTPPassword] TO [IRMAClientRole]
    AS [dbo];

