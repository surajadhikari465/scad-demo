 /****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetScaleWriterTypes]    Script Date: 08/21/2006 16:03:18 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetScaleWriterTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_GetScaleWriterTypes]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetScaleWriterTypes]    Script Date: 08/21/2006 16:03:18 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_POSPush_GetScaleWriterTypes
 AS 

BEGIN
	-- Queries the ScaleWriterType table to retrieve all of the available ScaleWriterType entries.
	SELECT ScaleWriterTypeKey, ScaleWriterTypeDesc  
	FROM ScaleWriterType 
	ORDER BY ScaleWriterTypeDesc
END
GO

 