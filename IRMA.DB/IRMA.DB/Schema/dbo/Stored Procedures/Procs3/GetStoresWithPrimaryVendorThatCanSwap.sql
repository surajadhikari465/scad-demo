CREATE PROCEDURE dbo.GetStoresWithPrimaryVendorThatCanSwap
    @VendorID int,
    @Item_Key int
AS
BEGIN
	-- Return a list of store numbers for the stores that have another vendor that can be 
	-- swapped for the current primary vendor assigned to the item.
	SELECT DISTINCT AvailVend.Store_No 
      FROM StoreItemVendor CurVend 
          INNER JOIN
            StoreItemVendor AvailVend
            -- Check to see if another vendor is assigned to this item & store.
            ON	AvailVend.Item_Key = CurVend.Item_Key AND 
				AvailVend.Store_No = CurVend.Store_no AND 
				AvailVend.Vendor_ID <> CurVend.Vendor_ID
      WHERE CurVend.Item_key = ISNULL(@Item_key, CurVend.Item_Key) AND 
			CurVend.Vendor_ID = @VendorID AND 
			CurVend.PrimaryVendor = 1 AND 
			AvailVend.PrimaryVendor = 0 AND 
			AvailVend.DeleteDate IS NULL -- only vendors that are not marked for deletion can be made primary 
										 -- because items are automatically de-authorized for a store when they don't
										 -- have a primary vendor, and the auto de-authorizations happen immediately  
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoresWithPrimaryVendorThatCanSwap] TO [IRMAClientRole]
    AS [dbo];

