CREATE PROCEDURE dbo.CheckVendorCostHistoryOverlap
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @Item_Key int,
    @Vendor_ID int,
    @StartDate smalldatetime,
    @EndDate smalldatetime
AS

BEGIN
    SET NOCOUNT ON

    SELECT COUNT(*)
    FROM StoreItemVendor SIV (nolock)
    INNER JOIN VendorCostHistory VCH (nolock)
        ON VCH.StoreItemVendorID = SIV.StoreItemVendorID
    INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store 
        ON Store.Key_Value = SIV.Store_No AND SIV.Item_Key = @Item_Key AND SIV.Vendor_ID = @Vendor_ID
    WHERE (ISNULL(@StartDate, CONVERT(smalldatetime,CONVERT(varchar(255),GETDATE(),101))) >= VCH.StartDate
          OR
          ISNULL(@EndDate, '2079-06-06') <= VCH.EndDate)
    AND ((DeleteDate IS NULL) OR (ISNULL(@EndDate, DeleteDate) < DeleteDate))

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorCostHistoryOverlap] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorCostHistoryOverlap] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorCostHistoryOverlap] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVendorCostHistoryOverlap] TO [IRMAReportsRole]
    AS [dbo];

