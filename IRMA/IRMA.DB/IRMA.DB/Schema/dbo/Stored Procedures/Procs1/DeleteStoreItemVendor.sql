CREATE PROCEDURE dbo.DeleteStoreItemVendor 
@Vendor_ID int, 
@Store_No int,
@Item_Key int,
@DeleteDate Smalldatetime
AS 
DECLARE @StoreItemCnt int

Update StoreItemVendor 
SET DeleteDate = @DeleteDate, PrimaryVendor = 0
WHERE Vendor_ID = @Vendor_ID AND 
      Item_Key = @Item_Key AND
      Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteStoreItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteStoreItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteStoreItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteStoreItemVendor] TO [IRMASLIMRole]
    AS [dbo];

