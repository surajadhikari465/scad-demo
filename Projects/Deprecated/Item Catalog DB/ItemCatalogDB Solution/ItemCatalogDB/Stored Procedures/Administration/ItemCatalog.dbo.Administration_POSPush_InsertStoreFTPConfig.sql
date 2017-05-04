/****** Object:  StoredProcedure [dbo].[Administration_POSPush_InsertStoreFTPConfig]    Script Date: 08/28/2006 16:03:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_InsertStoreFTPConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_InsertStoreFTPConfig]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_InsertStoreFTPConfig]    Script Date: 08/28/2006 16:03:50 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Administration_POSPush_InsertStoreFTPConfig
	@Store_No int, 
	@FileWriterType varchar(10), 
	@IP_Address varchar(15),
	@FTP_User varchar(25),
	@FTP_Password varchar(25),
	@ChangeDirectory varchar(25),
	@Port int,
	@IsSecureTransfer bit
AS
-- INSERTs configuration record in the StoreFTPConfig table for the POS Push process.
BEGIN
   
   INSERT INTO StoreFTPConfig (Store_No, FileWriterType, IP_Address, FTP_User, FTP_Password, ChangeDirectory, Port, IsSecureTransfer)
   VALUES (@Store_No, @FileWriterType, @IP_Address, @FTP_User, @FTP_Password, @ChangeDirectory, @Port, @IsSecureTransfer)
   
END
GO

  