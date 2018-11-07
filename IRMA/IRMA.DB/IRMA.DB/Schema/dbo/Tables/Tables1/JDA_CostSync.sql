CREATE TABLE [dbo].[JDA_CostSync] (
    [JDA_CostSync_ID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ApplyDate]       DATETIME        NOT NULL,
    [Store_No]        INT             NOT NULL,
    [Item_Key]        INT             NOT NULL,
    [Vendor_Id]       INT             NOT NULL,
    [Promotional]     BIT             NULL,
    [NetCost]         SMALLMONEY      NOT NULL,
    [Package_Desc1]   DECIMAL (10, 2) NULL,
    [StartDate]       DATETIME        NOT NULL,
    [EndDate]         DATETIME        NULL,
    [SyncState]       TINYINT         DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JDA_CostSync] PRIMARY KEY CLUSTERED ([JDA_CostSync_ID] ASC)
);

