CREATE PROCEDURE dbo.Replenishment_POSPush_GetFTPConfigForStoreAndWriterType(
	@Store_No int,
	@FileWriterType varchar(10)
)
AS 

BEGIN

	-- GETS ALL DATA FROM StoreFTPConfig FOR A SPECIFIC STORE/FILE WRITER TYPE COMBO
	SELECT 
		SFC.Store_No, 
		FileWriterType, 
		IP_Address, 
		FTP_User, 
		FTP_Password, 
		ChangeDirectory, 
		Port, 
		IsSecureTransfer,
		ISNULL(PST.POSSystemType, '') as POSSystemType,
		BusinessUnit_ID
	FROM 
		StoreFTPConfig SFC
	INNER JOIN 
		dbo.Store S ON SFC.Store_No = S.Store_No
	LEFT OUTER JOIN 
		dbo.POSSystemTypes PST ON S.POSSystemID = PST.POSSystemID
	WHERE SFC.Store_No = @Store_No
		AND FileWriterType = @FileWriterType	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForStoreAndWriterType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForStoreAndWriterType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForStoreAndWriterType] TO [IRMASchedJobsRole]
    AS [dbo];

