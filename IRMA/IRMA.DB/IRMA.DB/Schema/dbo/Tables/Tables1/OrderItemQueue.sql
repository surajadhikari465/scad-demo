CREATE TABLE [dbo].[OrderItemQueue] (
    [OrderItemQueue_ID]    INT             IDENTITY (1, 1) NOT NULL,
    [Store_No]             INT             NOT NULL,
    [TransferToSubTeam_No] INT             NOT NULL,
    [Item_Key]             INT             NOT NULL,
    [Transfer]             BIT             CONSTRAINT [DF_OrderItemQueue_Transfer] DEFAULT ((0)) NOT NULL,
    [User_ID]              INT             NOT NULL,
    [Quantity]             DECIMAL (18, 4) NOT NULL,
    [Unit_ID]              INT             NOT NULL,
    [CreditReason_ID]      INT             NULL,
    [Insert_Date]          DATETIME        CONSTRAINT [DF_OrderItemQueue_Insert_Date] DEFAULT (getdate()) NOT NULL,
    [Credit]               BIT             CONSTRAINT [DF_OrderItemQueue_Credit] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OrderItemQueue] PRIMARY KEY CLUSTERED ([OrderItemQueue_ID] ASC),
    CONSTRAINT [FK_OrderItemQueue_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_OrderItemQueue_ItemUnit] FOREIGN KEY ([Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_OrderItemQueue_StoreSubTeam] FOREIGN KEY ([Store_No], [TransferToSubTeam_No]) REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No]),
    CONSTRAINT [FK_OrderItemQueue_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderItemQueueLogical]
    ON [dbo].[OrderItemQueue]([Store_No] ASC, [TransferToSubTeam_No] ASC, [Item_Key] ASC, [Transfer] ASC, [Credit] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItemQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItemQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OrderItemQueue] TO [IRMAReportsRole]
    AS [dbo];

