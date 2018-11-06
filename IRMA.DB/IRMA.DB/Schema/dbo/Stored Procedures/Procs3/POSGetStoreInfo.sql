CREATE PROCEDURE dbo.POSGetStoreInfo
@Store_No int
AS 

SELECT Store.Store_No, 
		Store_Name, 
		IP_Address, 
		ChangeDirectory AS HD_Directory, 
		FTP_User AS FTPUser, 
		FTP_Password AS FTPPassword, 
		BatchID, BatchRecords, TelnetUser, TelnetPassword
FROM Store 
LEFT JOIN
	StoreFTPConfig
	ON StoreFTPConfig.Store_No = Store.Store_No 
WHERE FileWriterType = 'POS'
	AND Store.Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetStoreInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetStoreInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetStoreInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetStoreInfo] TO [IRMAReportsRole]
    AS [dbo];

