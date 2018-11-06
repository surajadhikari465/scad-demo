 /****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetAllFTPConfigData]    Script Date: 08/23/2006 16:33:09 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSPush_GetAllFTPConfigData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_POSPush_GetAllFTPConfigData]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetAllFTPConfigData]    Script Date: 08/23/2006 16:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

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
	ORDER BY FileWriterType, Store_No	
END
GO

 