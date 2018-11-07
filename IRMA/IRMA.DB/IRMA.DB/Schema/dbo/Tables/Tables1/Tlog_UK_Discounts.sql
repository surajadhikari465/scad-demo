CREATE TABLE [dbo].[Tlog_UK_Discounts] (
    [DiscountId]        INT           IDENTITY (1, 1) NOT NULL,
    [TimeKey]           SMALLDATETIME NULL,
    [Transaction_No]    INT           NULL,
    [Store_No]          INT           NULL,
    [Register_No]       INT           NULL,
    [DiscountAmt]       MONEY         NULL,
    [DiscountReference] VARCHAR (20)  NULL,
    [DiscountBarcode]   VARCHAR (20)  NULL,
    [DiscountReason]    VARCHAR (20)  NULL,
    [DiscountType]      INT           NULL,
    CONSTRAINT [PK_TLog_UK_Discounts] PRIMARY KEY CLUSTERED ([DiscountId] ASC),
    CONSTRAINT [FK_TLog_UK_Discounts_TLog_UK_Transaction] FOREIGN KEY ([TimeKey], [Transaction_No], [Store_No], [Register_No]) REFERENCES [dbo].[TLog_UK_Transaction] ([TimeKey], [Transaction_No], [Store_No], [Register_No])
);

