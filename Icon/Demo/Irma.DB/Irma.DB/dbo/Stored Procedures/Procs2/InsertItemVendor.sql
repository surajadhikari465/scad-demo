CREATE PROCEDURE dbo.InsertItemVendor 
    @Vendor_ID int, 
    @Item_Key int 
AS 
BEGIN
    SET NOCOUNT ON
    IF EXISTS(SELECT * FROM ItemVendor (NOLOCK) WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key)
      BEGIN
        UPDATE ItemVendor
        SET DeleteDate = null
        WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key
      END
    ELSE
      BEGIN
        INSERT INTO ItemVendor (Vendor_ID, Item_Key)
        VALUES (@Vendor_ID, @Item_Key)
      END
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemVendor] TO [IRMASLIMRole]
    AS [dbo];

