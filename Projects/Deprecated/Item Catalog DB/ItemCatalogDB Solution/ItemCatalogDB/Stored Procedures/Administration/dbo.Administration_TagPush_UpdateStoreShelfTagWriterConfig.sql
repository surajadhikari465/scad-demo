IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_TagPush_UpdateStoreShelfTagWriterConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_TagPush_UpdateStoreShelfTagWriterConfig]
GO

set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Administration_TagPush_UpdateStoreShelfTagWriterConfig]
@Store_No int, 
@POSFileWriterKey int, 
@Writer_Type varchar(20)
AS
-- Update an existing configuration record in the StoreShelfTagConfig table for the
-- ShelfTag printing.
BEGIN

update STTagConfig 
set STTagConfig.posfileWriterKey=@POSFileWriterKey 
From storeShelfTagConfig STTagConfig 
	INNER JOIN
		Store ST
		ON ST.Store_No = STTagConfig.Store_No
	INNER JOIN
		POSWriter POSW
		ON STTagConfig.POSFileWriterKey = POSW.POSFileWriterKey
where POSW.Disabled = 0 
	AND ((@Store_No IS NULL) OR (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
	AND ((@Writer_Type IS NULL) OR (@Writer_Type IS NOT NULL AND POSW.fileWriterType = @Writer_Type))

END
GO