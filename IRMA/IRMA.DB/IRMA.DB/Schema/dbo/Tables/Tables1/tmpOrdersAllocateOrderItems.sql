CREATE TABLE [dbo].[tmpOrdersAllocateOrderItems] (
    [OrderItem_ID]          INT             NOT NULL,
    [CompanyName]           VARCHAR (50)    NULL,
    [OrderHeader_ID]        INT             NULL,
    [Item_Key]              INT             NULL,
    [Package_Desc1]         DECIMAL (9, 4)  NULL,
    [QuantityOrdered]       DECIMAL (18, 4) NULL,
    [QuantityAllocated]     DECIMAL (18, 4) NULL,
    [OrigQuantityAllocated] DECIMAL (18, 4) NULL,
    [OrigPackage_Desc1]     DECIMAL (18, 4) NULL,
    [Transfer_To_SubTeam]   INT             NOT NULL,
    CONSTRAINT [PK_tmpOrdersAllocateOrderItems] PRIMARY KEY CLUSTERED ([OrderItem_ID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO PUBLIC
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO PUBLIC
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO PUBLIC
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO PUBLIC
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateOrderItems] TO [IRMAReportsRole]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The OrderHeader.Transfer_To_Subteam value associated with the PO this item appears on.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tmpOrdersAllocateOrderItems', @level2type = N'COLUMN', @level2name = N'Transfer_To_SubTeam';

