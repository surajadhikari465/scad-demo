CREATE TABLE [dbo].[InvoiceMatchingTolerance] (
    [Vendor_Tolerance]        DECIMAL (5, 2) NULL,
    [Vendor_Tolerance_Amount] SMALLMONEY     NULL,
    [User_ID]                 INT            CONSTRAINT [DF_InvoiceMatchingTolerance_User_ID] DEFAULT ((0)) NULL,
    [UpdateDate]              DATETIME       CONSTRAINT [DF_InvoiceMatchingTolerance_UpdateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [FK_InvoiceMatchingTolerance_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InvoiceMatchingTolerance] TO [IRMAReportsRole]
    AS [dbo];

