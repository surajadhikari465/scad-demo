if exists( select 'TRUE' from sys.objects o where o.object_id = OBJECT_ID(N'LabelType_Delete') )
begin
    drop procedure dbo.LabelType_Delete
end
go


create procedure dbo.LabelType_Delete( @LabelTypeID int )
as
begin
    /* The following tables store a LabelType_ID value and must be checked before allowing a delete.  (Need to verify list.)
            Item
            item_prep
            Item_Temp
            ItemChangeHistory
            LabelType
            NewItemsLoad
            PriceBatchDetail
            ShelfTagAttribute
            ShelfTagRule
    */
    if  exists( select 'TRUE' from Item with (NOLOCK) where LabelType_ID = @LabelTypeID )
        or  exists( select 'TRUE' from item_prep with (NOLOCK) where LabelType_ID = @LabelTypeID )
        or  exists( select 'TRUE' from Item_Temp with (NOLOCK) where LabelTypeID = @LabelTypeID )
        or  exists( select 'TRUE' from ItemChangeHistory with (NOLOCK) where LabelType_ID = @LabelTypeID )
        or  exists( select 'TRUE' from NewItemsLoad with (NOLOCK) where LabelType_ID = @LabelTypeID )
-- This table takes too long to read.  Probably need an index to perform this check.        
--        or  exists( select 'TRUE' from PriceBatchDetail with (NOLOCK) where LabelType_ID = @LabelTypeID )
        or  exists( select 'TRUE' from ShelfTagAttribute with (NOLOCK) where LabelTypeID = @LabelTypeID )
        or  exists( select 'TRUE' from ShelfTagRule with (NOLOCK) where LabelType_ID = @LabelTypeID )
    begin
        raiserror( N'This Label Type cannot be deleted due to items being associated to the Label Type', 18, -1 )
    end
    else
    begin
        delete LabelType
        where  LabelType_ID = @LabelTypeID
    end
end
go

