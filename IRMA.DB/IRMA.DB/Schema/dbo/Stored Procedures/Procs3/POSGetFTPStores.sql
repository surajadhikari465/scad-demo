CREATE PROCEDURE dbo.POSGetFTPStores
AS 

SELECT Mega_Store, WFM_Store, 
       CASE WHEN (IP_Address = 'NONE') OR (RTRIM(IP_Address) = '') 
            THEN NULL ELSE IP_Address END AS IP_Address, 
		Store.Store_No
FROM Store 
LEFT JOIN
	StoreFTPConfig
	ON StoreFTPConfig.Store_No = Store.Store_No 
WHERE FileWriterType = 'POS'
	AND BatchRecords > 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetFTPStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetFTPStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetFTPStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSGetFTPStores] TO [IRMAReportsRole]
    AS [dbo];

