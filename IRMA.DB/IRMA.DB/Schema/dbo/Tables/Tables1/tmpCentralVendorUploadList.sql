CREATE TABLE [dbo].[tmpCentralVendorUploadList] (
    [Vendor Name] NVARCHAR (255) NULL,
    [Region]      NVARCHAR (255) NULL,
    [PSnum]       VARCHAR (10)   NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpCentralVendorUploadList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpCentralVendorUploadList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpCentralVendorUploadList] TO [IRMAReportsRole]
    AS [dbo];

