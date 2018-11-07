CREATE TABLE [dbo].[InvoiceMatchingTolerance_VendorOverride] (
    [Vendor_ID]               INT            NOT NULL,
    [Vendor_Tolerance]        DECIMAL (5, 2) NULL,
    [Vendor_Tolerance_Amount] SMALLMONEY     NULL,
    [User_ID]                 INT            CONSTRAINT [DF_InvoicematchingTolerance_VendorOverride_User_ID] DEFAULT ((0)) NULL,
    [UpdateDate]              DATETIME       CONSTRAINT [DF_InvoicematchingTolerance_VendorOverride_UpdateDate] DEFAULT (getdate()) NULL,
    [Deleted]                 BIT            CONSTRAINT [DF_InvoiceMatchingTolerance_VendorOverride_Deleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_InvoiceMatchingTolerance_VendorOverride] PRIMARY KEY CLUSTERED ([Vendor_ID] ASC),
    CONSTRAINT [FK_InvoicematchingTolerance_VendorOverride_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);

