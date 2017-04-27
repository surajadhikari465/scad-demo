
/****** Object:  StoredProcedure [dbo].[GetStoreTlogFtpInfo]    Script Date: 08/25/2006 11:11:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreTlogFtpInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreTlogFtpInfo]
GO


CREATE PROCEDURE dbo.[GetStoreTlogFtpInfo]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	s.Store_Name, 
			s.Store_No,
			s.StoreAbbr, 
			sfc.Ftp_User as TlogFtpUser,
			sfc.Ftp_Password as TlogFtpPassword, 
			sfc.IP_Address as TlogIpAddress, 
			sfc.ChangeDirectory  as TlogFtpDirectory, 
			sfc.IsSecureTransfer
	FROM	Store s INNER JOIN StoreFtpConfig sfc 
			ON s.store_no = sfc.store_no 
	WHERE	s.WFM_Store=1 and sfc.FileWriterType = 'TLOG'
	ORDER BY Store_Name
END
GO

 