CREATE PROCEDURE dbo.GetStoreItemVendors
    @Item_Key int,
    @Store_No int
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
    
    SELECT Vendor.Vendor_ID, Vendor.CompanyName
    FROM Vendor (nolock)
        INNER JOIN
            StoreItemVendor SIV (nolock)
            ON SIV.Vendor_ID = Vendor.Vendor_ID 
    where SIV.Store_No = @Store_No AND SIV.Item_Key = @Item_Key
            AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemVendors] TO [IRMAReportsRole]
    AS [dbo];

