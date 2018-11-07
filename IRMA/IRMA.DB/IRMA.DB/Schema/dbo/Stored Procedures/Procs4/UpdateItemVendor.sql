CREATE PROCEDURE dbo.UpdateItemVendor 
    @Item_Key int,
    @Vendor_ID int,
    @Item_ID varchar(255) 
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE ItemVendor
    SET Item_ID = @Item_ID 
    FROM ItemVendor (rowlock)
    WHERE Vendor_ID = @Vendor_Id AND Item_Key = @Item_Key 
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemVendor] TO [IRMASLIMRole]
    AS [dbo];

