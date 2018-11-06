IF EXISTS ( SELECT  *
            FROM    sysobjects
            WHERE   name = 'SLIM_Scale_ExtraTextView'
                    AND xtype = 'v' ) 
    DROP VIEW [dbo].[SLIM_Scale_ExtraTextView]
GO
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

