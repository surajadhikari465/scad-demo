IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_TagPush_InsertStoreElectronicShelfTagConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_TagPush_InsertStoreElectronicShelfTagConfig]
GO

set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Administration_TagPush_InsertStoreElectronicShelfTagConfig]
@Store_No int, 
@POSFileWriterKey int, 
@ConfigType varchar(20)

AS
-- Insert a new configuration record into the StoreShelfTagConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO StoreElectronicShelfTagConfig (Store_No, POSFileWriterKey, ConfigType) 
   VALUES (@Store_No, @POSFileWriterKey, @ConfigType)
END
GO

