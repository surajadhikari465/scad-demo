CREATE PROCEDURE dbo.Administration_POSPush_DeletePOSWriterFileConfig
@POSFileWriterKey int,
@POSChangeTypeKey int,
@RowOrder int, 
@ColumnOrder int
AS
-- Delete the entry in the POSWriterFileConfig table, which is used for the
-- POS Push process.
-- The entries for all the of columns after this record are re-ordered by this
-- procedure.
BEGIN
   delete from POSWriterFileConfig  
   where POSFileWriterKey = @POSFileWriterKey and
   POSChangeTypeKey=@POSChangeTypeKey and 
   RowOrder=@RowOrder and 
   ColumnOrder=@ColumnOrder

   -- Create  temporary table to store all of the columns that appear AFTER the
   -- column being deleted.
   DECLARE @WriterConfigs TABLE (ColumnOrder int)

   INSERT INTO @WriterConfigs
   SELECT ColumnOrder
   FROM POSWriterFileConfig
   WHERE POSFileWriterKey = @POSFileWriterKey and
   POSChangeTypeKey=@POSChangeTypeKey and 
   RowOrder=@RowOrder and 
   ColumnOrder>@ColumnOrder

   -- Process each record in the temporary table, decreasing the ColumnOrder by 1
  DECLARE writerCursor CURSOR
  READ_ONLY
  FOR     
      SELECT ColumnOrder FROM @WriterConfigs

  DECLARE @ColOrder int
  OPEN writerCursor
    
  FETCH NEXT FROM writerCursor INTO @ColOrder
  WHILE (@@fetch_status <> -1)
  BEGIN
    IF (@@fetch_status <> -2)
    BEGIN
      UPDATE POSWriterFileConfig set ColumnOrder=@ColOrder - 1
      where POSFileWriterKey = @POSFileWriterKey and
      POSChangeTypeKey=@POSChangeTypeKey and 
      RowOrder=@RowOrder and 
      ColumnOrder=@ColOrder
    END
    FETCH NEXT FROM writerCursor INTO @ColOrder
  END
    
  CLOSE writerCursor
  DEALLOCATE writerCursor
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriterFileConfig] TO [IRMAReportsRole]
    AS [dbo];

