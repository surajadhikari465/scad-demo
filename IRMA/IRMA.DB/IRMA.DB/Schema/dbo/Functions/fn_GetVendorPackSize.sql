CREATE FUNCTION [dbo].[fn_GetVendorPackSize] 
(
	@Item_Key INT
	,@Vendor_ID INT
	,@Store_No INT
	,@Date SMALLDATETIME)
RETURNS DECIMAL
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/20/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
*/
BEGIN
    DECLARE @Package_Desc1 DECIMAL(9,4)

	-- Update target cost date that includes any lead-time for the vendor.
	SELECT @Date = @Date + dbo.fn_GetLeadTimeDays(@Vendor_ID)

    SELECT @Package_Desc1 = VCH.Package_Desc1
        FROM
            VendorCostHistory VCH (nolock)
        INNER JOIN
           (
				SELECT Store_No, ISNULL((SELECT TOP 1 VendorCostHistoryID
										 FROM VendorCostHistory (nolock)
										 INNER JOIN
											 StoreItemVendor (nolock)
											 ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
										 WHERE Promotional = 1
											 AND Store_No = @Store_No AND Item_Key = @Item_Key AND Vendor_ID = @Vendor_ID
											 AND ((@Date >= StartDate) AND (@Date <= EndDate))
											 AND @Date < ISNULL(DeleteDate, DATEADD(day, 1, @Date))
										 ORDER BY VendorCostHistoryID DESC),
										(SELECT TOP 1 VendorCostHistoryID
										 FROM VendorCostHistory (nolock)
										 INNER JOIN
											 StoreItemVendor (nolock)
											 ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
										 WHERE Promotional = 0
											 AND Store_No = @Store_No AND Item_Key = @Item_Key AND Vendor_ID = @Vendor_ID
											 AND ((@Date >= StartDate) AND (@Date <= EndDate))
											 AND @Date < ISNULL(DeleteDate, DATEADD(day, 1, @Date))
										 ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
				FROM  StoreItemVendor SIV (nolock)
				WHERE Item_Key  = @Item_Key  AND 
				      Vendor_ID = @Vendor_ID AND
				      Store_No  = @Store_No
				GROUP BY Store_No
            ) MVCH
				ON MVCH.MaxVCHID = VCH.VendorCostHistoryID
       

    RETURN @Package_Desc1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorPackSize] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorPackSize] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorPackSize] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorPackSize] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorPackSize] TO [IRMAReportsRole]
    AS [dbo];

