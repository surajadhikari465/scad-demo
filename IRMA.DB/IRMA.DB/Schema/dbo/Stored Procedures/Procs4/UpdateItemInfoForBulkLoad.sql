CREATE PROCEDURE dbo.[UpdateItemInfoForBulkLoad]
    @Identifier varchar(13),
    @POS_Description varchar(26), 
    @Item_Description varchar(60),
    @Discontinue_Item bit,
    @Discountable bit, 
    @Food_Stamps bit,
    @NatClassID int,
    @TaxClassID int,
	@Restricted_Hours bit,
	@ItemKey int output
AS 
BEGIN
    SET NOCOUNT ON
	
	SELECT @ItemKey = Item_Key FROM ItemIdentifier WHERE Identifier  = @Identifier and Deleted_Identifier = 0

    UPDATE Item
    SET 
        POS_Description = ISNULL(@POS_Description, POS_Description),
        Item_Description = ISNULL(@Item_Description, Item_Description),
        Discountable = ISNULL(@Discountable, Discountable),
        Food_Stamps = ISNULL(@Food_Stamps, Food_Stamps),
        ClassID = ISNULL(@NatClassID,  ClassID),
        TaxClassID = ISNULL(@TaxClassID, TaxClassID)
    FROM Item (rowlock)
    WHERE Item_Key = @ItemKey

	UPDATE Price
	SET Restricted_Hours = ISNULL(@Restricted_Hours,Restricted_Hours)
	FROM Price
	INNER JOIN Store on Price.Store_no = Store.Store_no and
	(Store.Mega_Store = 1 or Store.WFM_store = 1)
	WHERE Item_Key = @ItemKey
    
	UPDATE StoreItemVendor
	SET DiscontinueItem = @Discontinue_Item
	WHERE Item_Key = @ItemKey

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemInfoForBulkLoad] TO [IRMAClientRole]
    AS [dbo];

