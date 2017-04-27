SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POSGetStoreInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[POSGetStoreInfo]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

