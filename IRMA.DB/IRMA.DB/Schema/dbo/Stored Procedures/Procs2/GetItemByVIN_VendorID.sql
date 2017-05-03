CREATE PROCEDURE dbo.GetItemByVIN_VendorID
	
	(
	  @Item_ID varchar(12),
      @Vendor_ID int
	)
	
AS
	/* SET NOCOUNT ON */ 
	
	SELECT
        ItemVendor.Item_Key, ItemIdentifier.Identifier
        
FROM
        ItemVendor (nolock)
        inner join ItemIdentifier (nolock)
        on ItemVendor.item_key = ItemIdentifier.item_key
     
WHERE
        ItemVendor.Item_ID = @Item_ID
        AND ItemVendor.Vendor_ID = @Vendor_ID
        AND ItemVendor.DeleteDate is Null
        AND ItemIdentifier.Default_Identifier = 1
        AND ItemIdentifier.Deleted_Identifier = 0
        
	
	RETURN
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemByVIN_VendorID] TO [IRMAClientRole]
    AS [dbo];

