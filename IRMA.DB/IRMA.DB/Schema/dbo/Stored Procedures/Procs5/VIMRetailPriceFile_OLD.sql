CREATE PROCEDURE dbo.VIMRetailPriceFile_OLD
AS 
BEGIN
    SET NOCOUNT ON

        /**************************************************************************
        Find out if SKU identifiers should be excluded from the upload
        ***************************************************************************/
        DECLARE @ExcludeSKUIdentifiers bit
        SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)


    if object_id('dbo.VIMRetailPriceFileLoad_OLD') is not null DROP TABLE dbo.VIMRetailPriceFileLoad_OLD

    CREATE TABLE dbo.VIMRetailPriceFileLoad_OLD (
        [UPC] [varchar] (26) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [REGION] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
        [PS_BU] [int] NULL ,
        [PRICEZONE] [int] NULL ,
        --[POS_DEPT] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [POS_DEPT] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [REG_PRICE] [smallmoney] NOT NULL ,
        [REG_MULTIPLE] [tinyint] NOT NULL ,
        [EFF_PRICE] [smallmoney] NOT NULL ,
        [EFF_MULTIPLE] [tinyint] NOT NULL ,
        [EFF_PRICETYPE] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [START_DATE] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [END_DATE] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [EFF_DATE] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
        [PROMO_CODE] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
    ) ON [PRIMARY]

                CREATE INDEX idxVIMRetailPriceFileLoad ON dbo.VIMRetailPriceFileLoad_OLD (UPC, PS_BU)

        INSERT INTO dbo.VIMRetailPriceFileLoad_OLD

        SELECT
                SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier AS UPC,
                --(select primaryregioncode from instancedata) AS REGION,
                (select top 1 runmode from conversion_runmode) AS REGION,
                Store.BusinessUnit_ID AS PS_BU,
                Store.Zone_ID AS PRICEZONE,
                --LEFT(Item.Subteam_No, 3) AS POS_DEPT,
				ISNULL(Price.ExceptionSubteam_No, Item.Subteam_No)							AS POS_DEPT,
                Price.Price AS REG_PRICE,
                Price.Multiple AS REG_MULTIPLE,
                ROUND(CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeID) = 1
                        THEN CASE Price.PricingMethod_ID WHEN 0 THEN Price.Sale_Price
                                WHEN 1 THEN Price.Sale_Price
                                WHEN 2 THEN Price.Price
                                WHEN 4 THEN Price.Price
                                ELSE Price.Price END
                        ELSE Price.Price END, 2) AS EFF_PRICE,
                --dbo.fn_Price(Price.PriceChgTypeID, Price.Multiple, Price.Price, Price.PricingMethod_ID, Price.Sale_Multiple, Price.Sale_Price) AS EFF_PRICE,
                (CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeID) = 0 THEN Price.Multiple ELSE Price.Sale_Multiple END) AS EFF_MULTIPLE,
                dbo.fn_GetPriceType(Price.PriceChgTypeID) AS EFF_PRICETYPE,
        CONVERT(varchar(10), ISNULL((SELECT TOP 1 PBD.StartDate
                                     FROM PriceBatchDetail PBD
                                     INNER JOIN PriceBatchHeader PBH ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                                     WHERE PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
                                         AND PriceBatchStatusID = 6 AND PBD.PriceChgTypeID IS NOT NULL
                                     ORDER BY ProcessedDate DESC), CASE WHEN PCT.On_Sale = 1 THEN Sale_Start_Date ELSE '01/01/1900' END), 101) AS START_DATE,
        CONVERT(varchar(10), ISNULL((SELECT TOP 1 Sale_End_Date
                                     FROM PriceBatchDetail PBD
                                     INNER JOIN PriceBatchHeader PBH ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                                     WHERE PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
                                         AND PriceBatchStatusID = 6 AND PBD.PriceChgTypeID IS NOT NULL
                                     ORDER BY ProcessedDate DESC), CASE WHEN PCT.On_Sale = 1 THEN Sale_End_Date ELSE '06/06/2079' END), 101) AS END_DATE,
        --CONVERT(varchar(10), ISNULL((SELECT TOP 1 ProcessedDate
                                    -- FROM PriceBatchDetail PBD
                                    -- INNER JOIN PriceBatchHeader PBH ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                                    -- WHERE PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
                                         --AND PriceBatchStatusID = 6 AND PBD.PriceChgTypeID IS NOT NULL
                                     --ORDER BY ProcessedDate DESC), CASE WHEN PCT.On_Sale = 1 THEN Sale_Start_Date ELSE '01/01/1900' END), 101) AS EFF_DATE,
    CONVERT(varchar(10), ISNULL((SELECT TOP 1 PBD.StartDate
                                     FROM PriceBatchDetail PBD
                                     INNER JOIN PriceBatchHeader PBH ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                                     WHERE PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
                                         AND PriceBatchStatusID = 6 AND PBD.PriceChgTypeID IS NOT NULL
ORDER BY ProcessedDate DESC), CASE WHEN PCT.On_Sale = 1 THEN Sale_Start_Date ELSE '01/01/1900' END), 101) AS START_DATE,
                'NULL' AS PROMO_CODE
        FROM  Store (NOLOCK)
        INNER JOIN
        Price (NOLOCK)
        ON Store.Store_No = Price.Store_No
        INNER JOIN
                ItemIdentifier Identifier (NOLOCK) ON Price.Item_Key = Identifier.Item_Key AND Identifier.Deleted_Identifier = 0 AND Identifier.Remove_Identifier = 0
    INNER JOIN
        Item (NOLOCK)
        ON Identifier.Item_Key = Item.Item_Key
         INNER JOIN
                PriceChgType PCT
                ON PCT.PriceChgTypeID = Price.PriceChgTypeID
        WHERE (MEGA_STORE = 1 OR WFM_STORE = 1) -- AND IP_Address <> 'NONE'))
                AND Remove_Item = 0 AND Deleted_Item = 0
                AND Identifier NOT IN (SELECT Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0 GROUP BY Identifier HAVING COUNT(*) > 1)
                AND LEFT(Identifier, 1) <> '0'
                AND Store.Store_No in (select store_no from storeregionmapping where
                                                                        region_code in (select top 1 runmode from conversion_runmode))
                AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND Identifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS


        ORDER BY BusinessUnit_ID, Identifier

        DECLARE @VIMSamePrice table(UPC varchar(13) Primary Key)

        DECLARE @VIMDifferentPrice table(UPC varchar(13) Primary Key)


        INSERT INTO @VIMSamePrice
        SELECT UPC FROM
                (SELECT DISTINCT UPC, REG_PRICE, EFF_PRICE, REG_MULTIPLE, EFF_MULTIPLE, START_DATE, END_DATE
                FROM dbo.VIMRetailPriceFileLoad_OLD) AS T1
        GROUP BY UPC
        HAVING COUNT (*) = 1


        INSERT INTO @VIMDifferentPrice
        SELECT UPC FROM
                (SELECT DISTINCT UPC, REG_PRICE, EFF_PRICE, REG_MULTIPLE, EFF_MULTIPLE, START_DATE, END_DATE
                FROM dbo.VIMRetailPriceFileLoad_OLD
                ) AS T1
        GROUP BY UPC
        HAVING COUNT (*) > 1


        SELECT VRPL.[UPC], [REGION], 0 AS [PS_BU], 0 AS [PRICEZONE], [POS_DEPT], [REG_PRICE], [REG_MULTIPLE], [EFF_PRICE], [EFF_MULTIPLE], [EFF_PRICETYPE], [START_DATE], [END_DATE], [EFF_DATE], [PROMO_CODE]
        FROM @VIMSamePrice SP
        LEFT JOIN dbo.VIMRetailPriceFileLoad_OLD VRPL ON VRPL.UPC = SP.UPC

        UNION

        SELECT VRPL.[UPC], [REGION], [PS_BU], [PRICEZONE], [POS_DEPT], [REG_PRICE], [REG_MULTIPLE], [EFF_PRICE], [EFF_MULTIPLE], [EFF_PRICETYPE], [START_DATE], [END_DATE], [EFF_DATE], [PROMO_CODE]
        FROM @VIMDifferentPrice SP
        LEFT JOIN dbo.VIMRetailPriceFileLoad_OLD VRPL ON VRPL.UPC = SP.UPC

    /* DROP TABLE dbo.VIMRetailPriceFileLoad_OLD
        cleanup now occurs at beginning of stored procedure, for SSIS reasons */

    SET NOCOUNT OFF

END