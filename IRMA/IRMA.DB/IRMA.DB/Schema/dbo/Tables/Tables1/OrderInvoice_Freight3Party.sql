CREATE TABLE [dbo].[OrderInvoice_Freight3Party] (
    [OrderHeader_ID]         INT           NOT NULL,
    [InvoiceNumber]          VARCHAR (16)  NOT NULL,
    [InvoiceDate]            SMALLDATETIME NOT NULL,
    [InvoiceCost]            SMALLMONEY    NOT NULL,
    [Vendor_ID]              INT           NOT NULL,
    [UploadedDate]           SMALLDATETIME NULL,
    [MatchingValidationCode] INT           NULL,
    [MatchingUser_ID]        INT           NULL,
    [MatchingDate]           DATETIME      NULL,
    CONSTRAINT [PK_OrderInvoice_Freight3Party] PRIMARY KEY CLUSTERED ([OrderHeader_ID] ASC),
    CONSTRAINT [FK_OrderInvoice_Freight3Party_MatchingUser_ID] FOREIGN KEY ([MatchingUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_OrderInvoice_Freight3Party_MatchingValidationCode] FOREIGN KEY ([MatchingValidationCode]) REFERENCES [dbo].[ValidationCode] ([ValidationCode]),
    CONSTRAINT [FK_OrderInvoice_Freight3Party_OrderHeader_ID] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID]),
    CONSTRAINT [FK_OrderInvoice_Freight3Party_Vendor_ID] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);

