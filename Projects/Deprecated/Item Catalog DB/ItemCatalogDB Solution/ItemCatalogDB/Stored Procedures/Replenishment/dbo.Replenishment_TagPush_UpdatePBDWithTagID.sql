IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_TagPush_UpdatePBDWithTagID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_TagPush_UpdatePBDWithTagID]
GO

set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Replenishment_TagPush_UpdatePBDWithTagID]
@pbdID int, 
@itemKey int, 
@tagID int,
@tagID2 int
AS
-- Update an existing configuration record in the StoreShelfTagConfig table for the
-- ShelfTag printing.
BEGIN
update dbo.PriceBatchDetail
set TagTypeID=@tagID, TagTypeID2=@tagID2
where PriceBatchDetailID=@pbdID and
	  Item_Key = @itemKey

END
GO