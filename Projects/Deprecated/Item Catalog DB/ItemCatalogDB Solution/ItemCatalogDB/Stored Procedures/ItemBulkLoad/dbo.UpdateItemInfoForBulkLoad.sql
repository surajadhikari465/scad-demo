/****** Object:  StoredProcedure [dbo].[UpdateItemInfoForBulkLoad]    Script Date: 09/14/2006 14:08:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateItemInfoForBulkLoad]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateItemInfoForBulkLoad]

/****** Object:  StoredProcedure [dbo].[UpdateItemInfoForBulkLoad]    Script Date: 09/13/2006 14:28:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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