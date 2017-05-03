CREATE TABLE [dbo].[CostAdjustmentReason] (
    [CostAdjustmentReason_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]             VARCHAR (50) NOT NULL,
    [IsDefault]               BIT          CONSTRAINT [DF_CostAdjustmentReason_IsDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CostAdjustmentReason] PRIMARY KEY CLUSTERED ([CostAdjustmentReason_ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_CostAdjustmentReason_Description]
    ON [dbo].[CostAdjustmentReason]([Description] ASC);

