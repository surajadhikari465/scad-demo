CREATE PROCEDURE dbo.Administration_POSPush_UpdatePOSWriterFileConfigOrder
@POSFileWriterKey int,
@POSChangeTypeKey int,
@RowOrder int, 
@ColumnOrder int,
@MoveUp bit
AS
-- Reorders an entry in the POSWriterFileConfig table, which is used for the
-- POS Push process.
-- The selected column is switched with the column directly above or below
--  it, based on the MoveUp flag.
BEGIN
  -- Set the values for the old and new column numbers
  Declare @oldColumn1 int
  Declare @oldColumn2 int
  Declare @newColumn1 int
  Declare @newColumn2 int

  if @MoveUp = 1
		SELECT @NewColumn1 = @ColumnOrder - 1	-- move up (new position of column 1)
  else
		SELECT @NewColumn1 = @ColumnOrder + 1	-- move down (new position of column 1)
		
  SELECT @OldColumn1 = @ColumnOrder	-- original position of column 1
  SELECT @OldColumn2 = @NewColumn1	-- original position of column 2
  SELECT @NewColumn2 = @OldColumn1	-- new position of column 2 
  
	-- temporarily set the column order to negative values
	UPDATE POSWriterFileConfig
	SET ColumnOrder = ColumnOrder * -1
	WHERE POSFileWriterKey = @POSFileWriterKey AND
   		POSChangeTypeKey = @POSChangeTypeKey AND 
		RowOrder = @RowOrder AND 
   		(ColumnOrder = @OldColumn1 OR ColumnOrder = @OldColumn2)

	-- set new column 1 order to original column 2 order
	UPDATE POSWriterFileConfig
	SET ColumnOrder = @NewColumn1
	WHERE POSFileWriterKey = @POSFileWriterKey AND
   		POSChangeTypeKey = @POSChangeTypeKey AND 
		RowOrder = @RowOrder AND 
   		ColumnOrder = @OldColumn1 * -1

	-- set new column 2 order to original column 1 order
	UPDATE POSWriterFileConfig
	SET ColumnOrder = @NewColumn2
	WHERE POSFileWriterKey = @POSFileWriterKey AND
   		POSChangeTypeKey = @POSChangeTypeKey AND 
		RowOrder = @RowOrder AND 
   		ColumnOrder = @OldColumn2 * -1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfigOrder] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfigOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfigOrder] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdatePOSWriterFileConfigOrder] TO [IRMAReportsRole]
    AS [dbo];

