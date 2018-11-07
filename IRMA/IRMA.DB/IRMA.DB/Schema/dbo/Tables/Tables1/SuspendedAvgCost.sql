CREATE TABLE [dbo].[SuspendedAvgCost] (
    [Item_Key]       INT            NOT NULL,
    [Store_No]       INT            NOT NULL,
    [SubTeam_No]     INT            NOT NULL,
    [OrderHeader_ID] INT            NOT NULL,
    [OrderItem_ID]   INT            NOT NULL,
    [PriorAvgCost]   SMALLMONEY     NOT NULL,
    [NewAvgCost]     SMALLMONEY     NOT NULL,
    [Variance_Pct]   DECIMAL (7, 4) NOT NULL,
    [Effective_Date] SMALLDATETIME  NULL,
    [LastUpdateDt]   SMALLDATETIME  CONSTRAINT [DF_SuspendedAvgCost_LastUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SuspendedAvgCost] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [Store_No] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_SuspendedAvgCost_Item_Key] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_SuspendedAvgCost_OrderHeader_ID] FOREIGN KEY ([OrderHeader_ID]) REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID]),
    CONSTRAINT [FK_SuspendedAvgCost_OrderItem_ID] FOREIGN KEY ([OrderItem_ID]) REFERENCES [dbo].[OrderItem] ([OrderItem_ID]),
    CONSTRAINT [FK_SuspendedAvgCost_Store_No] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_SuspendedAvgCost_SubTeam_No] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SuspendedAvgCost] TO [IRMAReportsRole]
    AS [dbo];

