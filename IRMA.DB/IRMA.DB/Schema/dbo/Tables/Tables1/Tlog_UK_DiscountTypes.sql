CREATE TABLE [dbo].[Tlog_UK_DiscountTypes] (
    [DiscountTypeId]           INT          IDENTITY (1, 1) NOT NULL,
    [DiscountRecordIdentifier] VARCHAR (1)  NULL,
    [DiscountType]             VARCHAR (50) NULL,
    CONSTRAINT [PK_Tlog_UK_DiscountTypes] PRIMARY KEY CLUSTERED ([DiscountTypeId] ASC)
);

