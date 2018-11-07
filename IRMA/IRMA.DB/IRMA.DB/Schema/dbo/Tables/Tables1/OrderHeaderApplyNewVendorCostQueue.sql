CREATE TABLE [dbo].[OrderHeaderApplyNewVendorCostQueue] (
    [OrderHeader_ID] INT      NOT NULL,
    [InsertedDate]   DATETIME CONSTRAINT [DF_OrderHeaderApplyNewVendorCostQueue_InsertedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OrderHeaderApplyNewVendorCostQueue] PRIMARY KEY CLUSTERED ([OrderHeader_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_OrderHeaderApplyNewVendorCostQueue_OrderHeader] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
);

