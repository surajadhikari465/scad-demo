CREATE PROCEDURE dbo.GetRetailStoreCommInfo
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Store.Store_No, IP_Address, 
		ChangeDirectory AS HD_Directory, 
		FTP_User AS FTPUser, 
		FTP_Password AS FTPPassword, 
		BatchID, BatchRecords 
	FROM Store (NOLOCK)
	LEFT JOIN
		StoreFTPConfig
		ON StoreFTPConfig.Store_No = Store.Store_No 
	WHERE FileWriterType = 'POS' 
		AND (Mega_Store = 1 OR WFM_Store = 1)
	ORDER BY Store.Store_No
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoreCommInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoreCommInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoreCommInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoreCommInfo] TO [IRMAReportsRole]
    AS [dbo];

