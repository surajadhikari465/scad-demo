/****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetFileWriterClass]    Script Date: 07/20/2006 16:03:05 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetFileWriterClass]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_GetFileWriterClass]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetFileWriterClass]    Script Date: 07/20/2006 16:03:05 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_POSPush_GetFileWriterClass(
	@FileWriterType varchar(10)
)
 AS 
-- Queries the FileWriterClass table to retrieve all of the available entries for POSWriters OR ScaleWriters based on bit flag @POSWriter
BEGIN
	
	SELECT FileWriterClass, FileWriterType
	FROM FileWriterClass 
	WHERE FileWriterType = @FileWriterType
	ORDER BY FileWriterClass

END
GO

 