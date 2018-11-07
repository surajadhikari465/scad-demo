CREATE PROCEDURE dbo.CheckForDuplicateItemVendors 
    @Vendor_ID int, 
    @Item_Key int 
AS 
    SELECT COUNT(*) AS ItemVendorCount 
    FROM ItemVendor 
    WHERE Vendor_ID = @Vendor_ID AND Item_Key = @Item_Key and (DeleteDate is null or DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemVendors] TO [IRMAReportsRole]
    AS [dbo];

