CREATE TABLE [dbo].[InvoiceMatchingTolerance_StoreOverride] (
    [Store_No]                INT            NOT NULL,
    [Vendor_Tolerance]        DECIMAL (5, 2) NULL,
    [Vendor_Tolerance_Amount] SMALLMONEY     NULL,
    [User_ID]                 INT            CONSTRAINT [DF_InvoicematchingTolerance_StoreOverride_User_ID] DEFAULT ((0)) NULL,
    [UpdateDate]              DATETIME       CONSTRAINT [DF_InvoicematchingTolerance_StoreOverride_UpdateDate] DEFAULT (getdate()) NULL,
    [Deleted]                 BIT            CONSTRAINT [DF_InvoiceMatchingTolerance_StoreOverride_Deleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_InvoiceMatchingTolerance_StoreOverride] PRIMARY KEY CLUSTERED ([Store_No] ASC),
    CONSTRAINT [FK_InvoiceMatchingTolerance_StoreOverride_Store_No] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_InvoicematchingTolerance_StoreOverride_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);

