CREATE PROCEDURE dbo.UpdateItemID 
    @Item_Key int,
    @Vendor_ID int,
    @Item_ID varchar(255)
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE ItemVendor
    SET Item_ID = @Item_ID
    FROM ItemVendor (rowlock)
    WHERE Vendor_ID = @Vendor_Id AND Item_Key = @Item_Key and isnull(Item_ID,'') <> isnull(@Item_ID,'')
        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemID] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemID] TO [IRMAAVCIRole]
    AS [dbo];

