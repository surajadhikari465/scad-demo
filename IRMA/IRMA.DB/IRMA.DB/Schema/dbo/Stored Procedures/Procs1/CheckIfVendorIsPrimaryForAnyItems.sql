CREATE PROCEDURE dbo.CheckIfVendorIsPrimaryForAnyItems
    @VendorID int,
    @Item_Key int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

	--if vendor is the primary for any items return 1, else return 0
    IF EXISTS(SELECT * 
              FROM StoreItemVendor CurVend                   
              WHERE CurVend.Item_key = isnull(@Item_key, CurVend.Item_Key) 
					AND CurVend.Vendor_ID = @VendorID 
					AND CurVend.PrimaryVendor = 1 
					AND CurVend.store_no = isnull(@Store_No, CurVend.Store_No)                     
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
    ON OBJECT::[dbo].[CheckIfVendorIsPrimaryForAnyItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfVendorIsPrimaryForAnyItems] TO [IRMAClientRole]
    AS [dbo];

