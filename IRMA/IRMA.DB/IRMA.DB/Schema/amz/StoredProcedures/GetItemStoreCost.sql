CREATE PROCEDURE [amz].[GetItemStoreCost]
    @scanCode varchar(13),
    @businessUnitId int,
    @date date
AS
BEGIN

    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

    SELECT
        siv.Item_Key as itemKey,
        siv.Store_No as storeNo,
        s.BusinessUnit_ID as businessUnitId,
        ii.Identifier as identifier,
        ii.Default_Identifier as defaultIdentifier,
        ii.Deleted_Identifier as deletedIdentifier,
        vca.UnitCost as caseCost,
        vca.Package_Desc1 as casePack,
        vca.NetDiscount as netDiscount,
        vca.UnitFreight as unitFreight,
        vca.NetCost as netCaseCost,
        c.CurrencyCode as currencyCode
    FROM dbo.ItemIdentifier ii
    INNER JOIN dbo.StoreItemVendor siv on ii.Item_Key = siv.Item_Key
    INNER JOIN dbo.Store s on siv.Store_No = s.Store_No
    INNER JOIN dbo.StoreJurisdiction sj on s.StoreJurisdictionID = sj.StoreJurisdictionID
    INNER JOIN dbo.Currency c on sj.CurrencyID = c.CurrencyID
    INNER JOIN dbo.fn_VendorCostAll(@date) vca on siv.Item_Key = vca.Item_Key
        AND siv.Store_No = vca.Store_No
        AND siv.Vendor_ID = vca.Vendor_ID
    WHERE ii.Deleted_Identifier = 0
        AND ii.Remove_Identifier = 0
        AND ii.Default_Identifier = 1
        AND siv.PrimaryVendor = 1
        AND s.BusinessUnit_ID = @businessUnitId
        AND ii.Identifier = @scanCode

END
GO

GRANT EXEC on [amz].[GetItemStoreCost] to [prims-shrink-role]
GO