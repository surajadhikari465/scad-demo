IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_UpdateStoreFTPPassword]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_UpdateStoreFTPPassword]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Administration_POSPush_UpdateStoreFTPPassword
	@IP_Address varchar(15),
	@FTP_Password varchar(25)
AS
BEGIN
   UPDATE StoreFTPConfig SET		
		FTP_Password = @FTP_Password
   WHERE IP_Address = @IP_Address 
END
GO

