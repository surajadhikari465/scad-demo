CREATE FUNCTION [dbo].[fn_VendorCostAllPackSizes]     
(    
 @Date as smalldatetime    
)    
RETURNS TABLE     
AS    
RETURN (    
    

 SELECT ISNULL(vc.UnitCost, 0) - (ISNULL(dbo.fn_ItemNetDiscount(pack.Store_No, pack.Item_Key, pack.Vendor_ID, ISNULL(vc.UnitCost, 0), @Date),0)) AS NetCost,
        pack.item_key,
        pack.store_no,
        pack.package_desc1,
        pack.vendor_id,
		pack.vendorcosthistoryID
FROM VendorCostHistory vc
inner join
(SELECT
 siv.item_key,
 siv.store_no,
 siv.vendor_id,
 vch.package_desc1,
  vch.FreightUnit_ID,
  vch.CostUnit_ID,
 MAX(vendorcosthistoryid) AS vendorcosthistoryid
FROM VendorCostHistory vch
 inner join storeitemvendor siv
 ON vch.storeitemvendorid = siv.storeitemvendorid
WHERE vch.startdate <= @Date
AND vch.enddate >= @Date
GROUP BY
        vch.CostUnit_ID,
        vch.FreightUnit_ID,
        siv.item_key,
        siv.store_no,
        siv.vendor_id,
        vch.FreightUnit_ID,
        vch.Package_Desc1) pack
ON vc.vendorcosthistoryID = pack.vendorcosthistoryID

    
)
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAllPackSizes] TO [IRMAClientRole]
    AS [dbo];

