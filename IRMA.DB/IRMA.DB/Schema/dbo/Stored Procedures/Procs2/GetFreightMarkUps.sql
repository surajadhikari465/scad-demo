CREATE PROCEDURE dbo.GetFreightMarkUps
@Vendor_ID int,
@Store_No int
AS

SELECT Vendor_ID, Store_No, FreightMarkUp 
FROM VendorStoreFreight
where Store_no = isnull(@Store_no,Store_No) and Vendor_ID = isnull(@Vendor_ID, Vendor_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFreightMarkUps] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFreightMarkUps] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFreightMarkUps] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFreightMarkUps] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFreightMarkUps] TO [IRMAAVCIRole]
    AS [dbo];

