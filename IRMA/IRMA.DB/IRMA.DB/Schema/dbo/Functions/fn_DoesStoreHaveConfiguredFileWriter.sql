CREATE FUNCTION dbo.fn_DoesStoreHaveConfiguredFileWriter (
	@Store_No INT
	,@FileWriterType NVARCHAR(20)
	)
RETURNS BIT

BEGIN
	DECLARE @DoesStoreHaveFileWriter BIT = 0

	IF EXISTS (
			SELECT 1
			FROM StoreFTPConfig ftp
			WHERE ftp.Store_No = @Store_No
				AND ftp.FileWriterType = @FileWriterType
				AND ISNULL(ftp.IP_Address, '') <> ''
				AND ISNULL(ftp.FTP_User, '') <> ''
				AND ISNULL(ftp.FTP_Password, '') <> ''
			)
		SET @DoesStoreHaveFileWriter = 1

	RETURN @DoesStoreHaveFileWriter
END