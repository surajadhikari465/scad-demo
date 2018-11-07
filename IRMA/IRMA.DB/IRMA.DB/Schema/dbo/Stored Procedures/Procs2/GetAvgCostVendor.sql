CREATE PROCEDURE dbo.GetAvgCostVendor

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor_ID, CompanyName
    FROM Store
    INNER JOIN
        Zone
        ON Zone.Zone_ID = Store.Zone_ID
    INNER JOIN
        Vendor
        ON Vendor.Store_No = Store.Store_No

    WHERE 
		(dbo.fn_GetCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) = 3)
		AND ((Distribution_Center = 1) OR (Manufacturer = 1))
		AND EXISTS (SELECT *
                FROM Price
                INNER JOIN
                    Item
                    ON Item.Item_Key = Price.Item_Key
                WHERE Price.Store_No = Store.Store_No
                AND NoDistMarkup = 1
                AND Deleted_Item = 0 AND Remove_Item = 0)
                
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostVendor] TO [IRMAReportsRole]
    AS [dbo];

