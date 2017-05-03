CREATE FUNCTION dbo.fn_VendorCostAll     
 (@Date as smalldatetime)    
RETURNS TABLE     
AS    
RETURN (    
 SELECT    
  Result.Store_No,    
  Result.Item_Key,    
  Result.Vendor_ID,    
  UnitCost,    
  ISNULL(VendorCostHistory.UnitFreight, 0) AS UnitFreight,
  VendorCostHistory.Package_Desc1,    
  VendorCostHistory.StartDate,    
  VendorCostHistory.EndDate,    
  VendorCostHistory.InsertDate,    
  VendorCostHistory.CostUnit_ID,    
  VendorCostHistory.FreightUnit_ID,    
  VendorCostHistory.Promotional,    
  VendorCostHistory.FromVendor,    
  VendorCostHistory.VendorCostHistoryId,    
  --NET DISCOUNT AMT    
  ISNULL(dbo.fn_ItemNetDiscount(Result.Store_No, Result.Item_Key, Result.Vendor_ID, ISNULL(UnitCost, 0), @Date),0) AS NetDiscount,    
  --NET COST = REG COST - NET DISCOUNT + Freight    
  ISNULL(UnitCost, 0) - ISNULL(dbo.fn_ItemNetDiscount(Result.Store_No, Result.Item_Key, Result.Vendor_ID, ISNULL(UnitCost, 0), @Date),0) + ISNULL(UnitFreight, 0) AS NetCost    
 FROM     
  VendorCostHistory (nolock)      
  INNER JOIN (    
  SELECT    
   SIV.Store_No,    
   SIV.Item_Key,     
   SIV.Vendor_ID,    
   (SELECT     
    TOP 1 VendorCostHistory.VendorCostHistoryID    
   FROM VendorCostHistory (nolock)    
    INNER JOIN StoreItemVendor (nolock)    
     ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID    
   WHERE     
    StoreItemVendor.Store_No = SIV.Store_No     
    AND StoreItemVendor.Item_Key = SIV.Item_Key     
    AND StoreItemVendor.Vendor_ID = SIV.Vendor_ID    
    AND((@Date >= StartDate) AND (@Date <= EndDate))    
    AND @Date < ISNULL(DeleteDate, DATEADD(day, 1, @Date))    
   ORDER BY VendorCostHistoryID DESC    
   ) AS VendorCostHistoryID    
   FROM StoreItemVendor SIV (nolock)    
   GROUP BY Store_No, Item_Key, Vendor_ID    
  ) AS Result    
   ON  Result.VendorCostHistoryID = VendorCostHistory.VendorCostHistoryID    
       )
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostAll] TO [ExtractRole]
    AS [dbo];

