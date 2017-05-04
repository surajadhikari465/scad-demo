CREATE TABLE [dbo].[ItemVendorItemId] (
    [Item_Key]      INT          NOT NULL,
    [Vendor_ID]     INT          NOT NULL,
    [Item_ID]       VARCHAR (20) NOT NULL,
    [PrimaryVendor] BIT          NOT NULL,
    [Authorized]    BIT          NOT NULL
);

