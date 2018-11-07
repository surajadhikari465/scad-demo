CREATE TABLE [dbo].[CustomerReturnItem] (
    [ReturnItemID]       INT             IDENTITY (1, 1) NOT NULL,
    [ReturnID]           INT             NOT NULL,
    [Item_Key]           INT             NOT NULL,
    [Quantity]           DECIMAL (18, 4) CONSTRAINT [DF_CustomerReturnItem_Quantity] DEFAULT ((0)) NULL,
    [Weight]             DECIMAL (18, 4) CONSTRAINT [DF_CustomerReturnItem_Weight] DEFAULT ((0)) NULL,
    [Amount]             SMALLMONEY      NULL,
    [CustReturnReasonID] INT             NOT NULL,
    CONSTRAINT [PK_CustomerReturnItem] PRIMARY KEY CLUSTERED ([ReturnItemID] ASC),
    CONSTRAINT [FK_CustomerReturnItem_CustomerReturn] FOREIGN KEY ([ReturnID]) REFERENCES [dbo].[CustomerReturn] ([ReturnID]),
    CONSTRAINT [FK_CustomerReturnItem_CustomerReturnReason] FOREIGN KEY ([CustReturnReasonID]) REFERENCES [dbo].[CustomerReturnReason] ([CustReturnReasonID])
);

