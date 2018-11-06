--------------------------------------------------
-- The Fuax StoreItemVendor Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_StoreItemVendorView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS StoreItemVendorID ,
            ItemRequest.ItemRequest_ID AS item_key ,
            vendornumber AS vendor_id ,
            user_store AS store_no ,
            CAST(1 AS BIT) AS primaryvendor
    FROM    ItemRequest (NOLOCK)
    WHERE   ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_StoreItemVendorView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_StoreItemVendorView] TO [IRMAReportsRole]
    AS [dbo];

