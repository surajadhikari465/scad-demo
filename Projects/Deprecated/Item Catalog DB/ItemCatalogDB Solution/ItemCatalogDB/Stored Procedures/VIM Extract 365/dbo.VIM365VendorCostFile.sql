IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID(N'dbo.VIM365VendorCostFile'))
	EXEC('CREATE PROCEDURE [dbo].[VIM365VendorCostFile] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[VIM365VendorCostFile]
WITH RECOMPILE
AS 
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	if object_id('dbo.VIM365VendorCostFileLoad') is not null DROP TABLE dbo.VIM365VendorCostFileLoad
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo.VIM365VendorCostFileLoad]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.VIM365VendorCostFileLoad

CREATE TABLE dbo.VIM365VendorCostFileLoad (
	[UPC] [varchar] (26) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[REGION] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PS_BU] [int] NULL ,
	[VEND_ITEM_NUM] [varchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[REG_VEND_NUM_CZ] [int] NULL ,
	[REG_COST] [smallmoney] NULL ,
	[EFF_COST] [smallmoney] NULL ,
	[COST_ADJUSTMENTS] [int] NULL ,
	[CASE_SIZE] [decimal](9, 4) NULL ,
	[ITEM_UOM] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[REG_COST_DATE] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]


   /**************************************************************************
	Find out if SKU identifiers should be excluded from the upload
   ***************************************************************************/
	DECLARE @ExcludeSKUIdentifiers bit
	SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

	DECLARE @CurrDate datetime
	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101));

	DECLARE @365RegionCode varchar(2) = 'TS';

	INSERT INTO dbo.VIM365VendorCostFileLoad    
	SELECT 
		SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier AS UPC,
		@365RegionCode						AS REGION,
		Store.BusinessUnit_ID				AS PS_BU, 
		(CASE WHEN Item_ID IS NULL 
			  THEN '000000000000' 
			  ELSE SUBSTRING('000000000000', 1, 12 - LEN(LEFT(Item_ID, 12))) +  LEFT(Item_ID, 12) END) AS VEND_ITEM_NUM,
		Vendor.Vendor_ID					AS REG_VEND_NUM_CZ,
		SIV.UnitCost						AS REG_COST,
		SIV.NetCost							AS EFF_COST,
		NULL								AS COST_ADJUSTMENTS,
		CASE_SIZE = 
			CASE WHEN ISNULL(ItemVendor.IgnoreCasePack, 0) = 1
			THEN ItemVendor.RetailCasePack
			ELSE ISNULL(SIV.Package_Desc1, Item.Package_Desc1) 
			END, 
		Unit_Name							AS ITEM_UOM,
		CONVERT(varchar(10), StartDate, 101) AS REG_COST_DATE
	FROM
		ItemVendor (nolock)
	LEFT JOIN 
		dbo.fn_VendorCostAll(@CurrDate) SIV
		ON SIV.Item_Key = ItemVendor.Item_Key AND SIV.Vendor_ID = ItemVendor.Vendor_ID
   INNER JOIN
		StoreItem (nolock)
		ON StoreItem.Store_No = SIV.Store_No AND StoreItem.Item_Key = SIV.Item_Key AND StoreItem.Authorized = 1    
	INNER JOIN
		Item (NOLOCK) 
		ON Item.Item_Key = ItemVendor.Item_Key AND Deleted_Item = 0
	INNER JOIN
		ItemIdentifier (NOLOCK) 
		ON Item.Item_Key = ItemIdentifier.Item_Key 
	LEFT JOIN 
		ItemUnit (NOLOCK) 
		ON (Item.Vendor_Unit_Id = ItemUnit.Unit_Id)
	INNER JOIN 
		Store (NOLOCK) 
		ON Store.Store_no = SIV.Store_No 
	INNER JOIN 
		Vendor (NOLOCK) 
		ON Vendor.Vendor_Id = SIV.Vendor_Id
	WHERE ISNULL(ItemVendor.DeleteDate, DATEADD(day, 1, @CurrDate)) > @CurrDate
	AND (MEGA_STORE = 1 AND WFM_STORE = 0) 
	AND Store.Store_No in (select store_no from storeregionmapping where region_code = @365RegionCode)
	AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND ItemIdentifier.IdentifierType <> 'S'))

--Set up table variables to get the distinct records needed for the final file
	DECLARE @VIMSameVendorCost table(UPC varchar(13), REG_VEND_NUM_CZ int,  Primary Key(UPC, REG_VEND_NUM_CZ))

	DECLARE @VIMDifferentVendorCost table(UPC varchar(13), REG_VEND_NUM_CZ int,  Primary Key(UPC, REG_VEND_NUM_CZ))


	INSERT INTO @VIMSameVendorCost
	SELECT UPC, REG_VEND_NUM_CZ FROM 
		(SELECT DISTINCT UPC, EFF_COST, REG_VEND_NUM_CZ, REG_COST_DATE
		FROM dbo.VIM365VendorCostFileLoad WHERE REG_COST_DATE IS NOT NULL
		) T1
	GROUP BY UPC, REG_VEND_NUM_CZ
	HAVING COUNT (REG_VEND_NUM_CZ)  = 1


	INSERT INTO @VIMDifferentVendorCost
	SELECT UPC, REG_VEND_NUM_CZ FROM 
		(SELECT DISTINCT UPC, EFF_COST, REG_VEND_NUM_CZ, REG_COST_DATE
		FROM dbo.VIM365VendorCostFileLoad WHERE REG_COST_DATE IS NOT NULL
		) T1
	GROUP BY UPC, REG_VEND_NUM_CZ
	HAVING COUNT (REG_VEND_NUM_CZ)  > 1

	SELECT DISTINCT VCFL.[UPC], [REGION], 0 AS [PS_BU], [VEND_ITEM_NUM], VCFL.[REG_VEND_NUM_CZ], REG_COST AS 'REG_COST', EFF_COST AS 'EFF_COST', [COST_ADJUSTMENTS], [CASE_SIZE], [ITEM_UOM], CONVERT(varchar(10), REG_COST_DATE, 101) AS REG_COST_DATE 
	FROM @VIMSameVendorCost SVC
	LEFT JOIN dbo.VIM365VendorCostFileLoad VCFL ON VCFL.UPC = SVC.UPC AND VCFL.REG_VEND_NUM_CZ = SVC.REG_VEND_NUM_CZ
	WHERE REG_COST_DATE IS NOT NULL AND EFF_COST > 0

	UNION

	SELECT VCFL.[UPC], [REGION], [PS_BU], [VEND_ITEM_NUM], VCFL.[REG_VEND_NUM_CZ], REG_COST AS 'REG_COST', EFF_COST AS 'EFF_COST', [COST_ADJUSTMENTS], [CASE_SIZE], [ITEM_UOM], CONVERT(varchar(10), REG_COST_DATE, 101) AS REG_COST_DATE 
	FROM @VIMDifferentVendorCost DVC
	LEFT JOIN dbo.VIM365VendorCostFileLoad VCFL ON VCFL.UPC = DVC.UPC AND VCFL.REG_VEND_NUM_CZ = DVC.REG_VEND_NUM_CZ
	WHERE REG_COST_DATE IS NOT NULL AND EFF_COST > 0     

	SET NOCOUNT OFF

	/* drop table dbo.VIM365VendorCostFileLoad
	cleanup now occurs at beginning of stored procedure, for SSIS column-mapping reasons */


END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

