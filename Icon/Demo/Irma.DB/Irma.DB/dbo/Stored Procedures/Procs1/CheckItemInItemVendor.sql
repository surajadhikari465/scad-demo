CREATE PROCEDURE dbo.CheckItemInItemVendor
    @Item_Key int,
    @Vendor_ID int
AS 
    SELECT ItemVendor.Vendor_ID
    FROM Item INNER JOIN ItemVendor ON (Item.Item_Key = ItemVendor.Item_Key) 
    WHERE ItemVendor.Item_Key = @Item_Key and ItemVendor.Vendor_ID = @Vendor_ID and (ItemVendor.DeleteDate is null or ItemVendor.DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckItemInItemVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckItemInItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckItemInItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckItemInItemVendor] TO [IRMAReportsRole]
    AS [dbo];

