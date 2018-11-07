--------------------------------------------------
-- The Fuax VendorCostHistory Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_VendorCostHistoryView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS item_key ,
            ItemRequest.ItemRequest_ID AS StoreItemVendorID ,
            vendornumber AS vendor_id ,
            promotional AS promotional ,
            casecost AS unitcost ,
            costend AS enddate ,
            coststart AS startdate ,
            CAST(casesize AS DECIMAL(9, 4)) AS package_desc1 ,
            costunit AS costunit_id ,
            vendorfreightunit AS freightunit_id ,
            unitfreight AS unitfreight ,
            CAST(NULL AS SMALLMONEY) AS netcost ,
            CAST(NULL AS SMALLMONEY) AS netdiscount
    FROM    ItemRequest (NOLOCK)
    WHERE   ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_VendorCostHistoryView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_VendorCostHistoryView] TO [IRMAReportsRole]
    AS [dbo];

