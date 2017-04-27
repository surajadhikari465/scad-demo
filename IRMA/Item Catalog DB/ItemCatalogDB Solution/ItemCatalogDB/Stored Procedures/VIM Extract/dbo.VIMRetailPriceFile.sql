SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

alter PROCEDURE [dbo].[VIMRetailPriceFile]
AS 
BEGIN
--**************************************************************************
-- Procedure: VIMRetailPriceFile()
--    Author: N/A
--      Date: N/A
--
-- Description:
-- This procedure is called by the IRMA to VIM Extract SSIS job from the vim-vm-prd server
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 06/19/2013	BS		xxxxx	Added Change History.  Updated file to reflect the peformance changes that Chip Vimond (MA MAC)
--								made at the beginning of the year.
-- 09/18/2013	TL		11549	Changed to ALTER instead of drop/create because this proc is used for replication services and cannot be dropped (currently setup for SP region).
--**************************************************************************


    SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF object_id('dbo.VIMRetailPriceFileLoad') IS NOT NULL 
DROP TABLE dbo.VIMRetailPriceFileLoad

IF EXISTS (SELECT name FROM dbo.sysindexes WHERE name = N'[dbo].[idxVIMRetailPriceFileLoad]')
DROP INDEX [dbo].[idxVIMRetailPriceFileLoad]

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
		SubTeam.Dept_No																AS POS_DEPT,
		Price.Price																	AS REG_PRICE,
		Price.Multiple																AS REG_MULTIPLE,
		ROUND(CASE WHEN PCT.On_Sale = 1 THEN
				CASE Price.PricingMethod_ID
					WHEN 0 THEN Price.Sale_Price
					WHEN 1 THEN Price.Sale_Price
					WHEN 2 THEN Price.Price
					WHEN 4 THEN Price.Price
					ELSE Price.Price END
				ELSE Price.Price END, 2)											AS EFF_PRICE,
		--dbo.fn_Price(Price.PriceChgTypeID, Price.Multiple, Price.Price, Price.PricingMethod_ID, Price.Sale_Multiple, Price.Sale_Price) AS EFF_PRICE,
		(CASE WHEN PCT.On_Sale = 0 
			THEN Price.Multiple 
			ELSE Price.Sale_Multiple END)											AS EFF_MULTIPLE,
		PCT.PriceChgTypeDesc														AS EFF_PRICETYPE,
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
	INNER JOIN	SubTeam						(NOLOCK)	ON Item.SubTeam_No = SubTeam.SubTeam_No
	INNER JOIN
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

	WHERE (MEGA_STORE = 0 AND WFM_STORE = 1) -- AND IP_Address <> 'NONE'))
	AND Remove_Item = 0 AND Deleted_Item = 0
	AND Identifier NOT IN (SELECT Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0 GROUP BY Identifier HAVING COUNT(*) > 1)
	AND LEFT(Identifier, 1) <> '0'
	AND Store.Store_No IN (SELECT Store_No FROM StoreRegionMapping WHERE region_code = @Region)
	AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND Identifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS

CREATE CLUSTERED INDEX idxVIMRetailPriceFileLoad ON dbo.VIMRetailPriceFileLoad
(
	[UPC] ASC,
	[REGION] ASC,
	[POS_DEPT] ASC,
	[REG_PRICE] ASC,
	[REG_MULTIPLE] ASC,
	[EFF_PRICE] ASC,
	[EFF_MULTIPLE] ASC,
	[EFF_PRICETYPE] ASC,
	[START_DATE] ASC,
	[END_DATE] ASC,
	[EFF_DATE] ASC,
	[PROMO_CODE] ASC
) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

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
INNER JOIN dbo.VIMRetailPriceFileLoad VRPL ON VRPL.UPC = SP.UPC

UNION

SELECT VRPL.[UPC], [REGION], [PS_BU], [PRICEZONE], [POS_DEPT], [REG_PRICE], [REG_MULTIPLE], [EFF_PRICE], [EFF_MULTIPLE], [EFF_PRICETYPE], [START_DATE], [END_DATE], [EFF_DATE], [PROMO_CODE]
FROM @VIMDifferentPrice SP
INNER JOIN dbo.VIMRetailPriceFileLoad VRPL ON VRPL.UPC = SP.UPC

SET NOCOUNT OFF

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO