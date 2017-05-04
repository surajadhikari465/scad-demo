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
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfigRow] TO [IRMAReportsRole]
    AS [dbo];

