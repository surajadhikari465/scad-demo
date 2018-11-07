CREATE PROCEDURE dbo.GetPrimVendItmCnt
@VendorID int
AS
BEGIN
    SET NOCOUNT ON


    SELECT COUNT(DISTINCT source.item_key)  ItmCnt
    FROM StoreItemVendor source
        inner join
            StoreItemVendor Target
            on Source.Item_key = Target.Item_Key and Source.Store_no = Target.Store_No
    WHERE Source.vendor_id = @VendorID and Source.primaryvendor = 1 and Source.Vendor_ID <> Target.Vendor_ID

   
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPrimVendItmCnt] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPrimVendItmCnt] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPrimVendItmCnt] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPrimVendItmCnt] TO [IRMAReportsRole]
    AS [dbo];

