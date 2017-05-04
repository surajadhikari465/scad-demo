CREATE TABLE [dbo].[ItemHistoryQueue] (
    [Store_No]                   INT             NOT NULL,
    [Item_Key]                   INT             NOT NULL,
    [DateStamp]                  DATETIME        NOT NULL,
    [Quantity]                   DECIMAL (18, 4) CONSTRAINT [DF__ItemHistoOther_Hist__Quantity] DEFAULT ((0)) NULL,
    [Weight]                     DECIMAL (18, 4) CONSTRAINT [DF__ItemHistoOther_Hist__Weight] DEFAULT ((0)) NULL,
    [Cost]                       SMALLMONEY      CONSTRAINT [DF__ItemHistorOther_Hist__Cost] DEFAULT ((0)) NULL,
    [ExtCost]                    SMALLMONEY      CONSTRAINT [DF__ItemHistoOther_Hist__ExtCost] DEFAULT ((0)) NULL,
    [Retail]                     SMALLMONEY      CONSTRAINT [DF__ItemHistoOther_Hist__Retail] DEFAULT ((0)) NULL,
    [Adjustment_ID]              INT             NOT NULL,
    [AdjustmentReason]           VARCHAR (100)   NULL,
    [CreatedBy]                  INT             NOT NULL,
    [SubTeam_No]                 INT             NOT NULL,
    [Insert_Date]                DATETIME        CONSTRAINT [DF__itemhistoOther_Hist__Insert] DEFAULT (getdate()) NULL,
    [ItemHistoryID]              INT             IDENTITY (1, 1) NOT NULL,
    [OrderItem_ID]               INT             NULL,
    [InventoryAdjustmentCode_ID] INT             NULL,
    [CorrectionRecordFlag]       BIT             NULL,
    CONSTRAINT [pk_ItemHistoryOtherHist_ItemHistoryId] PRIMARY KEY CLUSTERED ([ItemHistoryID] ASC, [DateStamp] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_InventoryAdjustmentOther_HistCode_Id] FOREIGN KEY ([InventoryAdjustmentCode_ID]) REFERENCES [dbo].[InventoryAdjustmentCode] ([InventoryAdjustmentCode_ID]),
    CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem] FOREIGN KEY ([OrderItem_ID]) REFERENCES [dbo].[OrderItem] ([OrderItem_ID]),
    CONSTRAINT [FK_ItemHistoryOther_Hist_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
ALTER TABLE [dbo].[ItemHistoryQueue] NOCHECK CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem];

