CREATE PROCEDURE dbo.CheckIfPrimVendCanSwap
    @VendorID int,
    @Item_Key int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

--if record exists where primary vendor can be moved to then return 1 else return 0
    IF EXISTS(SELECT * 
              FROM StoreItemVendor CurVend 
                  INNER JOIN
                    StoreItemVendor AvailVend
                    -- Check to see if another vendor is assigned to this item & store.
                    on	AvailVend.Item_Key = CurVend.Item_Key and 
						AvailVend.Store_No = CurVend.Store_no and 
						AvailVend.Vendor_ID <> CurVend.Vendor_ID
              WHERE CurVend.Item_key = isnull(@Item_key, CurVend.Item_Key) and 
					CurVend.Vendor_ID = @VendorID and 
					CurVend.PrimaryVendor = 1 and 
					CurVend.store_no = isnull(@Store_No, CurVend.Store_No)and 
					AvailVend.PrimaryVendor = 0 and 
					AvailVend.DeleteDate IS null -- only vendors that are not marked for deletion can be made primary 
												 -- because items are automatically de-authorized for a store when they don't
												 -- have a primary vendor, and the auto de-authorizations happen immediately 
                )
        BEGIN
            SELECT 1 as IsPrimVend
        END
    ELSE
        BEGIN
            SELECT 0 as IsPrimVend
        END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfPrimVendCanSwap] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfPrimVendCanSwap] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfPrimVendCanSwap] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfPrimVendCanSwap] TO [IRMAReportsRole]
    AS [dbo];

