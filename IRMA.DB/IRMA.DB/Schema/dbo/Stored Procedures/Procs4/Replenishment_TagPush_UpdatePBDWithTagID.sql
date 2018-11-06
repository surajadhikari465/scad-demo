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
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_UpdatePBDWithTagID] TO [IRMAClientRole]
    AS [dbo];

