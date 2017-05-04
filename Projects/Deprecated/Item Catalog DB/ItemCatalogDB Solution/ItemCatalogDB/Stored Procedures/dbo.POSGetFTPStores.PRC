SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POSGetFTPStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[POSGetFTPStores]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


