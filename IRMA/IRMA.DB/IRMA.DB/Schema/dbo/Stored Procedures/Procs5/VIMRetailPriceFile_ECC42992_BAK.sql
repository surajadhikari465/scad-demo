





CREATE PROCEDURE [dbo].[VIMRetailPriceFile_ECC42992_BAK]
AS 
BEGIN
    SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF object_id('dbo.VIMRetailPriceFileLoad') is not null DROP TABLE dbo.VIMRetailPriceFileLoad

CREATE TABLE dbo.VIMRetailPriceFileLoad (
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

CREATE INDEX idxVIMRetailPriceFileLoad_NEW ON dbo.VIMRetailPriceFileLoad (UPC, PS_BU)

DECLARE @ExcludeSKUIdentifiers bit
DECLARE @Region varchar(5)
     
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('VIM_ExcludeSKUIdentifiers', NULL), 0)

SET @Region = (SELECT TOP 1 RunMode FROM Conversion_RunMode)                

INSERT INTO dbo.VIMRetailPriceFileLoad
	SELECT
		SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier			AS UPC,
		--(select primaryregioncode from instancedata) AS REGION,
		@Region																		AS REGION,
		Store.BusinessUnit_ID														AS PS_BU,
		Store.Zone_ID																AS PRICEZONE,
		--LEFT(Item.Subteam_No, 3) AS POS_DEPT,
		ISNULL(Price.ExceptionSubteam_No, Item.Subteam_No)							AS POS_DEPT,
		Price.Price																	AS REG_PRICE,
		Price.Multiple																AS REG_MULTIPLE,
		ROUND(CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeID) = 1 THEN
				CASE Price.PricingMethod_ID
					WHEN 0 THEN Price.Sale_Price
					WHEN 1 THEN Price.Sale_Price
					WHEN 2 THEN Price.Price
					WHEN 4 THEN Price.Price
					ELSE Price.Price END
				ELSE Price.Price END, 2)											AS EFF_PRICE,
		--dbo.fn_Price(Price.PriceChgTypeID, Price.Multiple, Price.Price, Price.PricingMethod_ID, Price.Sale_Multiple, Price.Sale_Price) AS EFF_PRICE,
		(CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeID) = 0 
			THEN Price.Multiple 
			ELSE Price.Sale_Multiple END)											AS EFF_MULTIPLE,
		dbo.fn_GetPriceType(Price.PriceChgTypeID)									AS EFF_PRICETYPE,
		CONVERT(varchar(10), ISNULL([START_DATE], CASE WHEN PCT.On_Sale = 1 
													THEN Price.Sale_Start_Date 
													ELSE '01/01/1900' END), 101)	AS START_DATE,
		CONVERT(varchar(10), ISNULL([END_DATE], CASE WHEN PCT.On_Sale = 1
													THEN Price.Sale_End_Date
													ELSE '06/06/2079' END), 101)	AS END_DATE,
		CONVERT(varchar(10), ISNULL([START_DATE], CASE WHEN PCT.On_Sale = 1 
													THEN Price.Sale_Start_Date 
													ELSE '01/01/1900' END), 101)	AS START_DATE,
		'NULL'																		AS PROMO_CODE
	FROM		Price						(NOLOCK)
	INNER JOIN	ItemIdentifier Identifier	(NOLOCK)	ON Price.Item_Key = Identifier.Item_Key	AND Identifier.Deleted_Identifier = 0	AND Identifier.Remove_Identifier = 0
	INNER JOIN	Item						(NOLOCK)	ON Identifier.Item_Key = Item.Item_Key
	INNER JOIN	Store						(NOLOCK)	ON Store.Store_No = Price.Store_No
	INNER JOIN	PriceChgType PCT			(NOLOCK)	ON PCT.PriceChgTypeID = Price.PriceChgTypeID
	LEFT JOIN
		(SELECT 			
			P.Item_Key,
			P.Store_No,
			(SELECT TOP 1 PBD.StartDate
					FROM PriceBatchDetail PBD	(NOLOCK)
					INNER JOIN PriceBatchHeader PBH	(NOLOCK) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
					WHERE PBH.PriceBatchStatusID = 6
					AND PBH.PriceChgTypeID IS NOT NULL
					AND PBD.Expired = 0
					AND PBD.Item_Key = P.Item_Key
					AND PBD.Store_No = P.Store_No
					ORDER BY
						PBD.StartDate DESC
			 )	AS [START_DATE],
			(SELECT TOP 1 PBD.Sale_End_Date
					FROM PriceBatchDetail PBD	(NOLOCK)
					INNER JOIN PriceBatchHeader PBH	(NOLOCK) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
					WHERE PBH.PriceBatchStatusID = 6
					AND PBH.PriceChgTypeID IS NOT NULL
					AND PBD.Expired = 0
					AND PBD.Item_Key = P.Item_Key
					AND PBD.Store_No = P.Store_No
					ORDER BY
						PBD.StartDate DESC
			 )	AS [END_DATE]
		FROM Price P		(NOLOCK)
		) AS EffDate
	ON EffDate.Item_Key = Price.Item_Key AND EffDate.Store_No = Price.Store_No

	WHERE (MEGA_STORE = 1 OR WFM_STORE = 1) -- AND IP_Address <> 'NONE'))
	AND Remove_Item = 0 AND Deleted_Item = 0
	AND Identifier NOT IN (SELECT Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0 GROUP BY Identifier HAVING COUNT(*) > 1)
	AND LEFT(Identifier, 1) <> '0'
	AND Store.Store_No in (SELECT Store_No FROM StoreRegionMapping WHERE region_code = @Region)
	AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND Identifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS
	ORDER BY BusinessUnit_ID, Identifier

DECLARE @VIMSamePrice table(UPC varchar(13) Primary Key)

DECLARE @VIMDifferentPrice table(UPC varchar(13) Primary Key)


INSERT INTO @VIMSamePrice
	SELECT UPC FROM
		(
			SELECT DISTINCT UPC, REG_PRICE, EFF_PRICE, REG_MULTIPLE, EFF_MULTIPLE, START_DATE, END_DATE FROM dbo.VIMRetailPriceFileLoad
		) AS T1
	GROUP BY UPC
	HAVING COUNT (*) = 1


INSERT INTO @VIMDifferentPrice
	SELECT UPC FROM
		(
			SELECT DISTINCT UPC, REG_PRICE, EFF_PRICE, REG_MULTIPLE, EFF_MULTIPLE, START_DATE, END_DATE FROM dbo.VIMRetailPriceFileLoad
		) AS T1
	GROUP BY UPC
	HAVING COUNT (*) > 1

SELECT VRPL.[UPC], [REGION], 0 AS [PS_BU], 0 AS [PRICEZONE], [POS_DEPT], [REG_PRICE], [REG_MULTIPLE], [EFF_PRICE], [EFF_MULTIPLE], [EFF_PRICETYPE], [START_DATE], [END_DATE], [EFF_DATE], [PROMO_CODE]
FROM @VIMSamePrice SP
LEFT JOIN dbo.VIMRetailPriceFileLoad VRPL ON VRPL.UPC = SP.UPC

UNION

SELECT VRPL.[UPC], [REGION], [PS_BU], [PRICEZONE], [POS_DEPT], [REG_PRICE], [REG_MULTIPLE], [EFF_PRICE], [EFF_MULTIPLE], [EFF_PRICETYPE], [START_DATE], [END_DATE], [EFF_DATE], [PROMO_CODE]
FROM @VIMDifferentPrice SP
LEFT JOIN dbo.VIMRetailPriceFileLoad VRPL ON VRPL.UPC = SP.UPC


    /* DROP TABLE dbo.VIMRetailPriceFileLoad
        cleanup now occurs at beginning of stored procedure, for SSIS reasons */

SET NOCOUNT OFF

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRetailPriceFile_ECC42992_BAK] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRetailPriceFile_ECC42992_BAK] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRetailPriceFile_ECC42992_BAK] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRetailPriceFile_ECC42992_BAK] TO [IRMAReportsRole]
    AS [dbo];

