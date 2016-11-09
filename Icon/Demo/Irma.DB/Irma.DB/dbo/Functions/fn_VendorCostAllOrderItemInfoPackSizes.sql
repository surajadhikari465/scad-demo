CREATE FUNCTION [dbo].[fn_VendorCostAllOrderItemInfoPackSizes]     
(    
 @Date as smalldatetime    
)    
RETURNS TABLE     
AS    
RETURN (    
    
  
  
select    
 siv.item_key,    
 siv.store_no,    
 siv.vendor_id,    
 vch.package_desc1,  
 ISNULL(vch.UnitFreight, 0) as UnitFreight,  
 vch.UnitCost as UnitCost,     
  vch.FreightUnit_ID,      
  vch.CostUnit_ID,    
 --NET DISCOUNT AMT      
 ISNULL(dbo.fn_ItemNetDiscount(siv.Store_No, siv.Item_Key, siv.Vendor_ID, ISNULL(UnitCost, 0), @Date),0) AS NetDiscount,      
 --NET COST = REG COST - NET DISCOUNT + Freight      
 ISNULL(UnitCost, 0) - ISNULL(dbo.fn_ItemNetDiscount(siv.Store_No, siv.Item_Key, siv.Vendor_ID, ISNULL(UnitCost, 0), @Date),0) + ISNULL(UnitFreight, 0) AS NetCost   ,  
 max(vendorcosthistoryid) as vendorcosthistoryid    
from VendorCostHistory vch    
 inner join storeitemvendor siv    
 on vch.storeitemvendorid = siv.storeitemvendorid
WHERE vch.startdate <= @Date
and vch.enddate >= @Date 

group by
	vch.CostUnit_ID,  
	unitfreight,  
	unitcost,
	vch.FreightUnit_ID,   
	siv.item_key,    
	siv.store_no,    
	siv.vendor_id,    
	vch.FreightUnit_ID,    
	vch.Package_Desc1   
    
    
)
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAllOrderItemInfoPackSizes] TO [IRMAClientRole]
    AS [dbo];

