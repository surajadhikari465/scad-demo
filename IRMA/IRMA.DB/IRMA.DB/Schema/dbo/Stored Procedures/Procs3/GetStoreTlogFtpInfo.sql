CREATE PROCEDURE dbo.[GetStoreTlogFtpInfo]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	s.Store_Name, 
			s.Store_No,
			s.StoreAbbr, 
			sfc.Ftp_User as TlogFtpUser,
			sfc.Ftp_Password as TlogFtpPassword, 
			sfc.IP_Address as TlogIpAddress, 
			sfc.ChangeDirectory  as TlogFtpDirectory, 
			sfc.IsSecureTransfer
	FROM	Store s INNER JOIN StoreFtpConfig sfc 
			ON s.store_no = sfc.store_no 
	WHERE	s.WFM_Store=1 and sfc.FileWriterType = 'TLOG'
	ORDER BY Store_Name
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreTlogFtpInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreTlogFtpInfo] TO [IRMAClientRole]
    AS [dbo];

