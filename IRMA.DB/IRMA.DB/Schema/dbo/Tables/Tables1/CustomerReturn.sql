CREATE TABLE [dbo].[CustomerReturn] (
    [ReturnID]    INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]  INT           NOT NULL,
    [Store_No]    INT           NOT NULL,
    [User_ID]     INT           NOT NULL,
    [ReturnDate]  SMALLDATETIME NOT NULL,
    [Approver_ID] INT           NOT NULL,
    CONSTRAINT [PK_CustomerReturn] PRIMARY KEY CLUSTERED ([ReturnID] ASC),
    CONSTRAINT [FK_CustomerReturn_Approver] FOREIGN KEY ([Approver_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_CustomerReturn_Customer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]),
    CONSTRAINT [FK_CustomerReturn_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_CustomerReturn_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);

