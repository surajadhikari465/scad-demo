CREATE PROCEDURE dbo.DeleteItemVendor 
@Vendor_ID int, 
@Item_Key int,
@DeleteDate Smalldatetime
AS  

DECLARE @ItemVendCnt int

Update ItemVendor 
SET DeleteDate = @DeleteDate
WHERE Vendor_ID = @Vendor_ID AND 
      Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemVendor] TO [IRMAReportsRole]
    AS [dbo];

