
CREATE PROCEDURE dbo.VIMVendorStoreItemFile
AS 
BEGIN
    SET NOCOUNT ON
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
   /**************************************************************************
    Find out if SKU identifiers should be excluded from the upload
   ***************************************************************************/
    DECLARE @ExcludeSKUIdentifiers bit
    SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)
    DECLARE @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT 
    	SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier AS UPC,
    	--(select primaryregioncode from instancedata) AS REGION,
    	(select top 1 runmode from conversion_runmode) AS REGION,
    	BusinessUnit_ID AS PS_BU,
    	(CASE WHEN Item_ID IS NULL 
              THEN '000000000000' 
    		  ELSE SUBSTRING('000000000000', 1, 12 - LEN(LEFT(Item_ID, 12))) +  LEFT(Item_ID, 12) END) AS VEND_ITEM_NUM,
    	ItemVendor.Vendor_ID AS REG_VEND_NUM_CZ,
    	(CASE WHEN PrimaryVendor = 1  THEN 'P' ELSE 'A' END) AS V_AUTH_STATUS,
    	Store.Zone_ID AS PRICEZONE
    FROM StoreItemVendor SIV (nolock)
    INNER JOIN
        StoreItem (nolock)
        ON StoreItem.Store_No = SIV.Store_No AND StoreItem.Item_Key = SIV.Item_Key AND StoreItem.Authorized = 1    
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = SIV.Item_Key AND Deleted_Item = 0
    INNER JOIN
        ItemIdentifier (NOLOCK)
        ON ItemIdentifier.Item_Key = Item.Item_Key 
        --AND Default_Identifier = 1  (Removed because the VIM needs all identifiers sent to POS)
    INNER JOIN 
        ItemVendor (NOLOCK) 
        On ItemVendor.Item_Key = SIV.Item_Key AND ItemVendor.Vendor_ID = SIV.Vendor_ID
    INNER JOIN 
        Store (NOLOCK) 
        ON Store.Store_No = SIV.Store_No
    WHERE @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
    AND @CurrDate < ISNULL(ItemVendor.DeleteDate, DATEADD(day, 1, @CurrDate)) 
    AND (MEGA_STORE = 0 AND WFM_STORE = 1)
    --AND IP_Address <> 'NONE'  
    AND (Identifier NOT IN (SELECT Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0 GROUP BY Identifier HAVING COUNT(*) > 1))
	AND (Store.Store_No in (select store_no from storeregionmapping where 
									region_code in (select top 1 runmode from conversion_runmode)))
	AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND ItemIdentifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS									
	ORDER BY SIV.Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorStoreItemFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorStoreItemFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorStoreItemFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMVendorStoreItemFile] TO [IRMAReportsRole]
    AS [dbo];

