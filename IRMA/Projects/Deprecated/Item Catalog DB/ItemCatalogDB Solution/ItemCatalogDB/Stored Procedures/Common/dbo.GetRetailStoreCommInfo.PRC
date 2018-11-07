SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRetailStoreCommInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRetailStoreCommInfo]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
