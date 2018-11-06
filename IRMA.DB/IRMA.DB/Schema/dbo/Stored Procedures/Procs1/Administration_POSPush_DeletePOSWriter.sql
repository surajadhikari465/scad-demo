CREATE PROCEDURE dbo.Administration_POSPush_DeletePOSWriter
@POSFileWriterKey int
AS
-- Disable the entry in the POSWriter table, which is used for the
-- POS Push process.
-- Disabling the writer also deletes the records in the StorePOSConfig table.
-- The associated POSWriterFileConfig records are not deleted.

-- NOTE: The admin UI does not currently provide a way to enable a writer
-- that has been disabled.  This must be done directly in the database.
BEGIN
  -- Disable the writer
  update POSWriter set Disabled=1 where POSFileWriterKey=@POSFileWriterKey

  -- Delete the associated StorePOSConifg records
  delete from StorePOSConfig where POSFileWriterKey=@POSFileWriterKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriter] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeletePOSWriter] TO [IRMAReportsRole]
    AS [dbo];

