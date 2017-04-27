/****** Object:  StoredProcedure [dbo].[Administration_POSPush_UpdateStoreFTPConfig]    Script Date: 08/28/2006 16:03:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_UpdateStoreFTPConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_UpdateStoreFTPConfig]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_UpdateStoreFTPConfig]    Script Date: 08/28/2006 16:03:50 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Administration_POSPush_UpdateStoreFTPConfig
	@Store_No int, 
	@FileWriterType varchar(10), 
	@IP_Address varchar(15),
	@FTP_User varchar(25),
	@FTP_Password varchar(25),
	@ChangeDirectory varchar(100),
	@Port int,
	@IsSecureTransfer bit
AS
-- Update an existing configuration record in the StoreFTPConfig table for the POS Push process.
BEGIN
   UPDATE StoreFTPConfig SET		
		IP_Address = @IP_Address,
		FTP_User = @FTP_User,
		FTP_Password = @FTP_Password,
		ChangeDirectory = @ChangeDirectory,
		Port = @Port,
		IsSecureTransfer = @IsSecureTransfer
   WHERE Store_No = @Store_No 
		AND FileWriterType = @FileWriterType
END
GO

 