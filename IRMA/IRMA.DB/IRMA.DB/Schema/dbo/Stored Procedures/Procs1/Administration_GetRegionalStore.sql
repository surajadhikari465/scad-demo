CREATE PROCEDURE dbo.Administration_GetRegionalStore
AS 

-- Returns the store information for the regional office

BEGIN

SELECT [Store_No]
      ,[Store_Name]
      ,[Phone_Number]
      ,[Mega_Store]
      ,[Distribution_Center]
      ,[Manufacturer]
      ,[WFM_Store]
      ,[Internal]
      ,[TelnetUser]
      ,[TelnetPassword]
      ,[BatchID]
      ,[BatchRecords]
      ,[BusinessUnit_ID]
      ,[Zone_ID]
      ,[UNFI_Store]
      ,[LastRecvLogDate]
      ,[LastRecvLog_No]
      ,[RecvLogUser_ID]
      ,[EXEWarehouse]
      ,[Regional]
      ,[LastSalesUpdateDate]
      ,[StoreAbbr]
      ,[PLUMStoreNo]
      ,[TaxJurisdictionID]
FROM Store 
WHERE Regional = 1
 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetRegionalStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetRegionalStore] TO [IRMAClientRole]
    AS [dbo];

