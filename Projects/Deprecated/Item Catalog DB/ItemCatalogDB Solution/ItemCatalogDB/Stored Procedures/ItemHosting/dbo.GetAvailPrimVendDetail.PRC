SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvailPrimVendDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvailPrimVendDetail]
GO


CREATE PROCEDURE dbo.GetAvailPrimVendDetail
@SourceVendorID int,
@TargetVendorID int,
@Item_Key int
AS
BEGIN
    SET NOCOUNT ON

    SELECT Item.Item_Key, Item.Item_Description, ItemIdentifier.Identifier, ItemVendor.Item_ID, ItemCategory.Category_Name, ItemCategory.Category_ID, Item.Brand_ID, Item.SubTeam_No, NULL as Store_No
    FROM Vendor
        INNER JOIN
            StoreItemVendor Target (NOLOCK)
            ON Vendor.Vendor_ID = Target.Vendor_ID
        INNER JOIN
            StoreItemVendor Source (NOLOCK)
            on Source.Item_Key = Target.Item_Key and Source.Store_No = Target.Store_no
               and Source.Vendor_ID = @SourceVendorID and Source.Vendor_ID <> Target.Vendor_ID
        INNER JOIN
            Item (NOLOCK)
            ON Item.Item_Key = Source.Item_Key
        INNER JOIN
            ItemVendor (NOLOCK)
            on ItemVendor.Item_Key = Item.Item_Key
        INNER JOIN
            ItemIdentifier (nolock)
            ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
        LEFT JOIN
            ItemCategory (nolock)
            ON Item.Category_ID = ItemCategory.Category_ID
    WHERE Vendor.Vendor_ID = @TargetVendorID and 
         (source.DeleteDate is null or source.DeleteDate > getdate())
    GROUP BY Item.Item_Key, Item.Item_Description, ItemIdentifier.Identifier, ItemVendor.Item_ID, ItemCategory.Category_Name, ItemCategory.Category_ID, Item.Brand_ID, Item.SubTeam_No
    ORDER BY Item_Description DESC
   
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

