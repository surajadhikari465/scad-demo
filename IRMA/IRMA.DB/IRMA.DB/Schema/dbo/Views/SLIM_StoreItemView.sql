--------------------------------------------------
-- The Fuax StoreItem Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_StoreItemView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS StoreItemAuthorizationID ,
            ItemRequest.ItemRequest_ID AS item_key ,
            user_store AS store_no ,
            CAST(1 AS BIT) AS authorized
    FROM    ItemRequest (NOLOCK)
    WHERE   ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_StoreItemView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_StoreItemView] TO [IRMAReportsRole]
    AS [dbo];

