--------------------------------------------------
-- The Fuax Scale_ExtraText Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_Scale_ExtraTextView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS Scale_ExtraText_ID ,
            extratextlabeltype AS scale_labeltype_id ,
            ingredients AS extratext
    FROM    ItemRequest (NOLOCK)
            JOIN ItemScaleRequest (NOLOCK) ON ItemScaleRequest.ItemRequest_ID = ItemRequest.ItemRequest_ID
    WHERE   ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_Scale_ExtraTextView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_Scale_ExtraTextView] TO [IRMAReportsRole]
    AS [dbo];

