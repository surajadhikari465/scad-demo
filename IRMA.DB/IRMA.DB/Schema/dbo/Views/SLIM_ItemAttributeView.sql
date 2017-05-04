--------------------------------------------------
-- The Fuax ItemIdentifier Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_ItemAttributeView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS item_key ,
            hasingredients AS check_box_2 ,
            CAST(commoditycode AS VARCHAR(50)) AS text_1 ,
            CAST(discountterms AS VARCHAR(50)) AS text_3 ,
            CAST(golocal AS VARCHAR(50)) AS text_4 ,
            CAST(misc AS VARCHAR(50)) AS text_5 ,
            CAST(esrscki AS VARCHAR(50)) AS text_6 ,
            CAST(NULL AS BIT) AS Check_Box_1 ,
            CAST(NULL AS BIT) AS Check_Box_3 ,
            CAST(NULL AS BIT) AS Check_Box_4 ,
            CAST(NULL AS BIT) AS Check_Box_5 ,
            CAST(NULL AS BIT) AS Check_Box_6 ,
            CAST(NULL AS BIT) AS Check_Box_7 ,
            CAST(NULL AS BIT) AS Check_Box_8 ,
            CAST(NULL AS BIT) AS Check_Box_9 ,
            CAST(NULL AS BIT) AS Check_Box_10 ,
            CAST(NULL AS BIT) AS Check_Box_11 ,
            CAST(NULL AS BIT) AS Check_Box_12 ,
            CAST(NULL AS BIT) AS Check_Box_13 ,
            CAST(NULL AS BIT) AS Check_Box_14 ,
            CAST(NULL AS BIT) AS Check_Box_15 ,
            CAST(NULL AS BIT) AS Check_Box_16 ,
            CAST(NULL AS BIT) AS Check_Box_17 ,
            CAST(NULL AS BIT) AS Check_Box_18 ,
            CAST(NULL AS BIT) AS Check_Box_19 ,
            CAST(NULL AS BIT) AS Check_Box_20 ,
            CAST(NULL AS VARCHAR(50)) AS Text_2 ,
            CAST(NULL AS VARCHAR(50)) AS Text_7 ,
            CAST(NULL AS VARCHAR(50)) AS Text_8 ,
            CAST(NULL AS VARCHAR(50)) AS Text_9 ,
            CAST(NULL AS VARCHAR(50)) AS Text_10 ,
            CAST(NULL AS DATETIME) AS Date_Time_1 ,
            CAST(NULL AS DATETIME) AS Date_Time_2 ,
            CAST(NULL AS DATETIME) AS Date_Time_3 ,
            CAST(NULL AS DATETIME) AS Date_Time_4 ,
            CAST(NULL AS DATETIME) AS Date_Time_5 ,
            CAST(NULL AS DATETIME) AS Date_Time_6 ,
            CAST(NULL AS DATETIME) AS Date_Time_7 ,
            CAST(NULL AS DATETIME) AS Date_Time_8 ,
            CAST(NULL AS DATETIME) AS Date_Time_9 ,
            CAST(NULL AS DATETIME) AS Date_Time_10
    FROM    ItemRequest (NOLOCK)
    WHERE   ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_ItemAttributeView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_ItemAttributeView] TO [IRMAReportsRole]
    AS [dbo];

