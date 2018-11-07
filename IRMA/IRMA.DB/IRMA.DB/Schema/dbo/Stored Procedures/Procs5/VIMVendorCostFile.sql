
CREATE PROCEDURE dbo.VIMVendorCostFile 
WITH RECOMPILE 
AS 
   -- **************************************************************************
   -- Procedure: VIMVendorCostFile
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- 6/30/2011   TTL	TFS 2196: Added ignore-case-pack check so that a retail pack size is returned instead of the vendor's pack when the ignore flag is ON.
   -- 8/18/2011   TTL	TFS 2690: Changed the precision for the two cost fields to three decimal places.
   -- **************************************************************************
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	/* cleanup of dbo.VIMVendorCostFileLoad  */
	if object_id('dbo.VIMVendorCostFileLoad') is not null DROP TABLE dbo.VIMVendorCostFileLoad
	
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo.VIMVendorCostFileLoad]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.VIMVendorCostFileLoad

CREATE TABLE dbo.VIMVendorCostFileLoad (
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

	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))


	INSERT INTO dbo.VIMVendorCostFileLoad    

	SELECT 
		SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier AS UPC,
		--(select primaryregioncode from instancedata) AS REGION, 
		(select top 1 runmode from conversion_runmode) AS REGION,
		Store.BusinessUnit_ID AS PS_BU, 
		(CASE WHEN Item_ID IS NULL 
			  THEN '000000000000' 
			  ELSE SUBSTRING('000000000000', 1, 12 - LEN(LEFT(Item_ID, 12))) +  LEFT(Item_ID, 12) END) AS VEND_ITEM_NUM,
		Vendor.Vendor_ID AS REG_VEND_NUM_CZ,
		SIV.UnitCost AS REG_COST,
		SIV.NetCost AS EFF_COST,
		NULL AS COST_ADJUSTMENTS,
		-- TFS 2196, Tom Lux, 6/30/2011: Adding ignore-case-pack check for pack size.
		CASE_SIZE = 
			CASE WHEN ISNULL(ItemVendor.IgnoreCasePack, 0) = 1
			THEN ItemVendor.RetailCasePack
			ELSE ISNULL(SIV.Package_Desc1, Item.Package_Desc1) 
			END, 
		Unit_Name AS ITEM_UOM,
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
		--AND Default_Identifier = 1	(Removed because VIM needs all identifiers sent to POS)
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
	AND (MEGA_STORE = 0 AND WFM_STORE = 1) 
	AND Store.Store_No in (select store_no from storeregionmapping where 
									region_code in (select top 1 runmode from conversion_runmode))
	AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND ItemIdentifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS
	--AND IP_Address <> 'NONE'


--Set up table variables to get the distinct records needed for the final file
	DECLARE @VIMSameVendorCost table(UPC varchar(13), REG_VEND_NUM_CZ int,  Primary Key(UPC, REG_VEND_NUM_CZ))

	DECLARE @VIMDifferentVendorCost table(UPC varchar(13), REG_VEND_NUM_CZ int,  Primary Key(UPC, REG_VEND_NUM_CZ))


	INSERT INTO @VIMSameVendorCost
	SELECT UPC, REG_VEND_NUM_CZ FROM 
		(SELECT DISTINCT UPC, EFF_COST, REG_VEND_NUM_CZ, REG_COST_DATE
		FROM dbo.VIMVendorCostFileLoad WHERE REG_COST_DATE IS NOT NULL
		) T1
	GROUP BY UPC, REG_VEND_NUM_CZ
	HAVING COUNT (REG_VEND_NUM_CZ)  = 1


	INSERT INTO @VIMDifferentVendorCost
	SELECT UPC, REG_VEND_NUM_CZ FROM 
		(SELECT DISTINCT UPC, EFF_COST, REG_VEND_NUM_CZ, REG_COST_DATE
		FROM dbo.VIMVendorCostFileLoad WHERE REG_COST_DATE IS NOT NULL
		) T1
	GROUP BY UPC, REG_VEND_NUM_CZ
	HAVING COUNT (REG_VEND_NUM_CZ)  > 1

	SELECT DISTINCT VCFL.[UPC], [REGION], 0 AS [PS_BU], [VEND_ITEM_NUM], VCFL.[REG_VEND_NUM_CZ], REG_COST AS 'REG_COST', EFF_COST AS 'EFF_COST', [COST_ADJUSTMENTS], [CASE_SIZE], [ITEM_UOM], CONVERT(varchar(10), REG_COST_DATE, 101) AS REG_COST_DATE 
	FROM @VIMSameVendorCost SVC
	LEFT JOIN dbo.VIMVendorCostFileLoad VCFL ON VCFL.UPC = SVC.UPC AND VCFL.REG_VEND_NUM_CZ = SVC.REG_VEND_NUM_CZ
	WHERE REG_COST_DATE IS NOT NULL AND EFF_COST > 0

	UNION

	SELECT VCFL.[UPC], [REGION], [PS_BU], [VEND_ITEM_NUM], VCFL.[REG_VEND_NUM_CZ], REG_COST AS 'REG_COST', EFF_COST AS 'EFF_COST', [COST_ADJUSTMENTS], [CASE_SIZE], [ITEM_UOM], CONVERT(varchar(10), REG_COST_DATE, 101) AS REG_COST_DATE 
	FROM @VIMDifferentVendorCost DVC
	LEFT JOIN dbo.VIMVendorCostFileLoad VCFL ON VCFL.UPC = DVC.UPC AND VCFL.REG_VEND_NUM_CZ = DVC.REG_VEND_NUM_CZ
	WHERE REG_COST_DATE IS NOT NULL AND EFF_COST > 0     

	SET NOCOUNT OFF

	/* drop table dbo.VIMVendorCostFileLoad
	cleanup now occurs at beginning of stored procedure, for SSIS column-mapping reasons */


END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorCostFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorCostFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorCostFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorCostFile] TO [IRMAReportsRole]
    AS [dbo];

