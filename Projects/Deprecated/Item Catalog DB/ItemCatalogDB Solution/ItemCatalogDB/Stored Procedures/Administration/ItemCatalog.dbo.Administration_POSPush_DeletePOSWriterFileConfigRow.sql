/****** Object:  StoredProcedure [dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow]    Script Date: 05/19/2006 16:02:57 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow]    Script Date: 05/19/2006 16:02:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Administration_POSPush_DeletePOSWriterFileConfigRow
	@POSFileWriterKey int,
	@POSChangeTypeKey int,
	@RowOrder int
AS
-- Delete entries in the POSWriterFileConfig table, which is used for the POS Push process, for the given row param
BEGIN
   DELETE FROM POSWriterFileConfig  
   WHERE POSFileWriterKey = @POSFileWriterKey
		AND POSChangeTypeKey = @POSChangeTypeKey
		AND RowOrder = @RowOrder

	-- Create  temporary table to store all of the rows that appear AFTER the row being deleted.
	DECLARE @WriterConfigs TABLE (RowOrder int)

	INSERT INTO @WriterConfigs
		SELECT DISTINCT RowOrder
		FROM POSWriterFileConfig
		WHERE POSFileWriterKey = @POSFileWriterKey
			AND POSChangeTypeKey = @POSChangeTypeKey
			AND RowOrder > @RowOrder
		ORDER BY RowOrder ASC

	-- Process each record in the temporary table, decreasing the RowOrder by 1
	DECLARE writerCursor CURSOR
	READ_ONLY
	FOR     
		SELECT RowOrder FROM @WriterConfigs

		DECLARE @RowOrderTemp int
		OPEN writerCursor
    
		FETCH NEXT FROM writerCursor INTO @RowOrderTemp
		WHILE (@@fetch_status <> -1)
		BEGIN
			IF (@@fetch_status <> -2)
			BEGIN
				UPDATE POSWriterFileConfig SET RowOrder = @RowOrderTemp - 1
				WHERE POSFileWriterKey = @POSFileWriterKey
					AND POSChangeTypeKey = @POSChangeTypeKey
					AND RowOrder = @RowOrderTemp
			END
		FETCH NEXT FROM writerCursor INTO @RowOrderTemp
	END
    
  CLOSE writerCursor
  DEALLOCATE writerCursor
END
GO

