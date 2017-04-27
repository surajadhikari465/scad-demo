  /****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore]    Script Date: 08/28/2006 16:03:18 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore]    Script Date: 08/28/2006 16:03:18 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_POSPush_GetAvailableFileWriterTypeForStore(
	@Store_No int,
	@IsConfigWriter bit
)
 AS 

BEGIN
	-- Queries the StoreFTPConfig table to retrieve all of the FileWriterType entries that have not yet been
	-- configured for that store.
	-- If @IsConfigWriter = 1 then data is limited to those where IsConfigWriter = 1
	SELECT FileWriterType  
	FROM FileWriterType
	WHERE FileWriterType NOT IN (SELECT FileWriterType FROM StoreFTPConfig WHERE Store_No = @Store_No)
		AND ((@IsConfigWriter = 1 AND IsConfigWriter = 1) OR (@IsConfigWriter = 0)) 
	ORDER BY FileWriterType
END
GO

