CREATE TABLE [dbo].[VendorStoreFreight] (
    [Vendor_ID]     INT            NOT NULL,
    [Store_No]      INT            NOT NULL,
    [FreightMarkUp] DECIMAL (5, 4) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorStoreFreight] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorStoreFreight] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorStoreFreight] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorStoreFreight] TO [IRMAAVCIRole]
    AS [dbo];

