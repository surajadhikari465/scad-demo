
CREATE PROCEDURE [dbo].[VIM365RetailFuturePriceFile]
AS

BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/**************************************************************************
Find out if SKU identifiers should be excluded from the upload
***************************************************************************/
DECLARE @ExcludeSKUIdentifiers bit
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

/**************************************************************************
365 Regiono Code
***************************************************************************/
DECLARE @365RegionCode varchar(2) = 'TS';

SELECT
    SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier.Identifier)) + Identifier.Identifier AS UPC,
    @365RegionCode AS REGION,
    Store.BusinessUnit_ID AS PS_BU,
    Store.Zone_ID AS PRICEZONE,
    st.Dept_No AS POS_DEPT,
    CASE dbo.fn_OnSale(PBD.PriceChgTypeID) WHEN 1 THEN PBD.Sale_Price
                    ELSE Price.Price END AS FUT_PRICE,
    CASE dbo.fn_OnSale(PBD.PriceChgTypeID) WHEN 1 THEN PBD.Sale_Multiple
                    ELSE Price.Multiple END AS FUT_MULTIPLE,
    upper(isnull(PCT.PriceChgTypeDesc,'')) AS FUT_PRICE_TYPE,
    CONVERT(VARCHAR(10), PBD.StartDate,101) AS START_DATE,
    CASE WHEN PBD.Sale_End_Date IS NULL THEN '12/31/2099' ELSE CONVERT(VARCHAR(10),PBD.Sale_End_Date,101) END AS END_DATE,
    'NULL' AS PROMO_CODE
FROM 
PriceBatchDetail PBD (nolock)
INNER JOIN
    Store (NOLOCK)
    ON PBD.Store_No = Store.Store_No
INNER JOIN
    Item (NOLOCK)
    ON PBD.Item_Key = Item.Item_Key
INNER JOIN
    Price (nolock)
    ON Price.Item_Key = PBD.Item_Key AND Price.Store_No = PBD.Store_No
INNER JOIN
    ItemIdentifier Identifier (NOLOCK) ON Item.Item_Key = Identifier.Item_Key AND Identifier.Deleted_Identifier = 0 AND Identifier.Remove_Identifier = 0
INNER JOIN
	SubTeam st (nolock) ON Item.SubTeam_No = st.SubTeam_No
LEFT JOIN
    PriceBatchHeader PBH (nolock)
    ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
LEFT JOIN
        PriceChgType PCT
        ON PCT.PriceChgTypeID = PBD.PriceChgTypeID

WHERE (MEGA_STORE = 1 AND WFM_STORE = 0)
    AND ISNULL(PriceBatchStatusID, 0) < 6
    AND PBD.PriceChgTypeID IS NOT NULL
    AND PBD.StartDate = CONVERT(smalldatetime, CONVERT(varchar(255), DATEADD(day, 1, GETDATE()), 101))
    AND Identifier.Identifier NOT IN (SELECT Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0 GROUP BY Identifier HAVING COUNT(*) > 1)
    AND LEFT(Identifier.Identifier, 1) <> '0'
    AND Store.Store_No in (select store_no from storeregionmapping where region_code = @365RegionCode)
        AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND Identifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS

ORDER BY BusinessUnit_ID, Identifier.Identifier

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIM365RetailFuturePriceFile] TO [IRMASchedJobsRole]
    AS [dbo];

