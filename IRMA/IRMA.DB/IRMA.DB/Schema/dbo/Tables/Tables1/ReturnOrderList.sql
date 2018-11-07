CREATE TABLE [dbo].[ReturnOrderList] (
    [OrderHeader_ID]       INT NOT NULL,
    [ReturnOrderHeader_ID] INT NOT NULL,
    CONSTRAINT [PK_ReturnOrderList_OrderHeaderId_ReturnOrderHeaderID] PRIMARY KEY CLUSTERED ([OrderHeader_ID] ASC, [ReturnOrderHeader_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__ReturnOrd__Order__063D977B] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID]),
    CONSTRAINT [FK__ReturnOrd__Retur__0731BBB4] FOREIGN KEY ([ReturnOrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxReturnOrderListOrderID]
    ON [dbo].[ReturnOrderList]([OrderHeader_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxReturnOrderListReturnID]
    ON [dbo].[ReturnOrderList]([ReturnOrderHeader_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReturnOrderList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReturnOrderList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReturnOrderList] TO [IRMAReportsRole]
    AS [dbo];

