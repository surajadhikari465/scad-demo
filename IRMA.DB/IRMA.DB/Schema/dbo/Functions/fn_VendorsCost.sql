﻿CREATE FUNCTION dbo.fn_VendorsCost 
	(@Item_Key int, @Store_No int, @Date smalldatetime)
RETURNS TABLE
AS
RETURN (SELECT Vendor_ID, UnitCost, ISNULL(UnitFreight, 0) As UnitFreight, Package_Desc1, MSRP, PrimaryVendor
        FROM
            VendorCostHistory VCH (nolock)
        INNER JOIN
           (SELECT Vendor_ID, PrimaryVendor, ISNULL((SELECT TOP 1 VendorCostHistoryID
                                                     FROM VendorCostHistory (nolock)
                                                     INNER JOIN
                                                         StoreItemVendor (nolock)
                                                         ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
                                                     WHERE Promotional = 1
                                                         AND Store_No = @Store_No AND Item_Key = @Item_Key AND Vendor_ID = SIV.Vendor_ID
                                                         AND ((@Date >= StartDate) AND (@Date <= EndDate))
                                                         AND @Date < ISNULL(DeleteDate, DATEADD(day, 1, @Date))
                                                     ORDER BY VendorCostHistoryID DESC),
                                                    (SELECT TOP 1 VendorCostHistoryID
                                                     FROM VendorCostHistory (nolock)
                                                     INNER JOIN
                                                         StoreItemVendor (nolock)
                                                         ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
                                                     WHERE Promotional = 0
                                                         AND Store_No = @Store_No AND Item_Key = @Item_Key AND Vendor_ID = SIV.Vendor_ID
                                                         AND ((@Date >= StartDate) AND (@Date <= EndDate))
                                                         AND @Date < ISNULL(DeleteDate, DATEADD(day, 1, @Date))
                                                     ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
            FROM StoreItemVendor SIV (nolock)
            WHERE Item_Key = @Item_Key AND Store_No = @Store_No
            GROUP BY Vendor_ID, PrimaryVendor) MVCH
           ON MVCH.MaxVCHID = VCH.VendorCostHistoryID
       )
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorsCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorsCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorsCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorsCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorsCost] TO [IRMAReportsRole]
    AS [dbo];

