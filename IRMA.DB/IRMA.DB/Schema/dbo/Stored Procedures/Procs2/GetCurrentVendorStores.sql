CREATE PROCEDURE dbo.GetCurrentVendorStores     
    @Vendor_ID int,
    @Item_Key int

AS
/*DECLARE @Item_Key int,
        @Vendor_ID int

SELECT @vendor_id =5645,
       @item_key = 114831*/


BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime
    DECLARE @Exclude TABLE (Store_No int)

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT distinct Store_No
    FROM StoreItemVendor SIV (nolock)
    WHERE (@CurrDate < isnull(SIV.DeleteDate,dateadd(d,1,@CurrDate))) 
          AND (SIV.Vendor_ID = @Vendor_ID) and SIV.Item_Key = @Item_Key

    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentVendorStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentVendorStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentVendorStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentVendorStores] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentVendorStores] TO [IRMAAVCIRole]
    AS [dbo];

