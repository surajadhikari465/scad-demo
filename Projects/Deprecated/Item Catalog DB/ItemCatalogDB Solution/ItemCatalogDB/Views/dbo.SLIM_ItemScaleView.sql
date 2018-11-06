

IF EXISTS ( SELECT  *
            FROM    sysobjects
            WHERE   name = 'SLIM_ItemScaleView'
                    AND xtype = 'v' ) 
    DROP VIEW [dbo].[SLIM_ItemScaleView]
GO
--------------------------------------------------
-- The Fuax ItemScale Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_ItemScaleView]
AS
    SELECT  ItemScaleRequest.ItemRequest_ID AS itemScale_id ,
            ItemScaleRequest.ItemRequest_ID AS item_key ,
            ItemScaleRequest.ItemRequest_ID AS scale_extraText_id ,
            bycount AS scale_bycount ,
            fixedweight AS scale_fixedweight ,
            forcetare AS forcetare ,
            labelstyle AS scale_labelstyle_id ,
            scaleuomunit_id AS scale_scaleuomunit_id ,
            shelflife AS shelflife_length ,
            scale_tare_id AS scale_tare_id ,
            ItemScaleRequest.scaledescription1 AS scale_description1 ,
            ItemScaleRequest.scaledescription2 AS scale_description2 ,
            ItemScaleRequest.scaledescription3 AS scale_description3 ,
            ItemScaleRequest.scaledescription4 AS scale_description4 ,
            CAST(NULL AS BIT) AS printblankeatby ,
            CAST(NULL AS BIT) AS printblankpackdate ,
            CAST(NULL AS BIT) AS printblankshelflife ,
            CAST(NULL AS BIT) AS printblanktotalprice ,
            CAST(NULL AS BIT) AS printblankunitprice ,
            CAST(NULL AS BIT) AS printblankweight ,
            CAST(NULL AS INT) AS scale_alternate_tare_id ,
            CAST(NULL AS INT) AS scale_eatby_id ,
            CAST(NULL AS INT) AS scale_grade_id ,
            CAST(NULL AS INT) AS scale_randomweighttype_id
    FROM    ItemScaleRequest (NOLOCK)
            JOIN ItemRequest (NOLOCK) ON ItemRequest.ItemRequest_ID = ItemScaleRequest.ItemRequest_ID
    WHERE   ItemStatus_ID = 2

GO

