CREATE PROCEDURE dbo.Replenishment_POSPush_GetAllFTPConfigData
AS 

BEGIN

	-- GETS ALL DATA FROM StoreFTPConfig
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
	ORDER BY FileWriterType, SFC.Store_No	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetAllFTPConfigData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetAllFTPConfigData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetAllFTPConfigData] TO [IRMASchedJobsRole]
    AS [dbo];

