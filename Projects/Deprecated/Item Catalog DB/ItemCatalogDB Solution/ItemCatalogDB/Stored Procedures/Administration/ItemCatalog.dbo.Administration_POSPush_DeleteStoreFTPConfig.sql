 /****** Object:  StoredProcedure [dbo].[Administration_POSPush_DeleteStoreFTPConfig]    Script Date: 08/28/2006 16:03:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_DeleteStoreFTPConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_DeleteStoreFTPConfig]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_DeleteStoreFTPConfig]    Script Date: 08/28/2006 16:03:50 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Administration_POSPush_DeleteStoreFTPConfig
	@Store_No int, 
	@FileWriterType varchar(10)
AS
-- DELETEs configuration record in the StoreFTPConfig table for the POS Push process.
BEGIN
   
	DELETE FROM StoreFTPConfig WHERE Store_No = @Store_No AND FileWriterType = @FileWriterType
   
END
GO

  