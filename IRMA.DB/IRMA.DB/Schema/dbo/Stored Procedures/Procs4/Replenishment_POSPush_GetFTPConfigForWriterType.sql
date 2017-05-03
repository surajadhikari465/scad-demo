CREATE PROCEDURE dbo.Replenishment_POSPush_GetFTPConfigForWriterType(
	@FileWriterType varchar(10)
)
AS 

BEGIN


	-- GETS ALL DATA FROM StoreFTPConfig FOR A SPECIFIC FILE WRITER TYPE
	SELECT 
		SFC.Store_No, 
		S.Store_Name,
		S.StoreAbbr,
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
	WHERE 
		FileWriterType = @FileWriterType
	ORDER BY 
		Store_No

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForWriterType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForWriterType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForWriterType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetFTPConfigForWriterType] TO [IRMARSTRole]
    AS [dbo];

