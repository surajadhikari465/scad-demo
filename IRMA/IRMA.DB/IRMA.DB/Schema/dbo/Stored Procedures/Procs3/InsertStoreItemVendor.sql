CREATE PROCEDURE dbo.InsertStoreItemVendor     
    @Vendor_ID int, 
    @Store_NO int,
    @Item_Key int
AS 
BEGIN
    SET NOCOUNT ON
    DECLARE @StoreItemCnt int
    DECLARE @PrimVend bit

    SELECT @StoreItemCnt = Count(*) 
    FROM StoreItemVendor SIV 
    WHERE Item_key = @Item_Key and Store_no = @Store_No and (DeleteDate is null or DeleteDate > getdate())
    SET @PrimVend = CASE WHEN @StoreItemCnt = 0 THEN 1 ELSE 0 END

    IF EXISTS(SELECT * FROM StoreItemVendor (NOLOCK) WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key and Store_no = @Store_No)
      BEGIN
        UPDATE StoreItemVendor
        SET DeleteDate = null,
            PrimaryVendor = CASE WHEN @PrimVend = 1 THEN 1 ELSE 0 END
        WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key and Store_No = @Store_No  
              

      END
    ELSE
      BEGIN
        INSERT INTO StoreItemVendor (Store_No, Item_Key, Vendor_ID, PrimaryVendor)
        VALUES(@Store_No, @Item_Key, @Vendor_ID, @PrimVend)
      END
    SET NOCOUNT OFF



END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertStoreItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertStoreItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertStoreItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertStoreItemVendor] TO [IRMASLIMRole]
    AS [dbo];

