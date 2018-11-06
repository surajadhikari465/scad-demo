IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_TagPush_DeleteStoreShelfTagConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_TagPush_DeleteStoreShelfTagConfig]
GO

set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Administration_TagPush_DeleteStoreShelfTagConfig]
@Store_No int
AS
-- Delete the entry in the StoreShelfTagConfig table, which is used for the
-- ShelfTag process.
BEGIN
   delete from StoreShelfTagConfig  
   where Store_No = @Store_No
END
GO

