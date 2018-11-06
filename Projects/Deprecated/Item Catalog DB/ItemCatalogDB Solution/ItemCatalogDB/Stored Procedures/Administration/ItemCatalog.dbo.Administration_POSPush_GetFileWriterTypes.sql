 /****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetFileWriterTypes]    Script Date: 08/21/2006 16:03:18 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetFileWriterTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_GetFileWriterTypes]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetFileWriterTypes]    Script Date: 08/21/2006 16:03:18 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_POSPush_GetFileWriterTypes(
	@IsConfigWriter bit
)
 AS 

BEGIN
	-- Queries the FileWriterType table to retrieve all of the available FileWriterType entries.
	-- If @IsConfigWriter = 1 then data is limited to those wehre IsConfigWriter = 1
	SELECT FileWriterType  
	FROM FileWriterType 
	WHERE ((@IsConfigWriter = 1 AND IsConfigWriter = 1) OR (@IsConfigWriter = 0)) 
	ORDER BY FileWriterType
END
GO

