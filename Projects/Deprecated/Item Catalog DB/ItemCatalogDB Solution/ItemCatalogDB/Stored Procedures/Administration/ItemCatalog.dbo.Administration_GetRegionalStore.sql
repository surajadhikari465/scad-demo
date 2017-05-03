 /****** Object:  StoredProcedure [dbo].[Administration_GetRegionalStore]    Script Date: 08/24/2006 16:33:09 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_GetRegionalStore]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_GetRegionalStore]
GO

/****** Object:  StoredProcedure [dbo].[Administration_GetRegionalStore]    Script Date: 08/24/2006 16:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

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

  