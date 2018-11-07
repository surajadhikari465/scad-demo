CREATE TABLE [dbo].[JDA_PriceSync] (
    [JDA_PriceSync_ID]     BIGINT     IDENTITY (1, 1) NOT NULL,
    [ApplyDate]            DATETIME   NOT NULL,
    [Item_Key]             INT        NOT NULL,
    [Store_No]             INT        NOT NULL,
    [Multiple]             TINYINT    NULL,
    [Price]                SMALLMONEY NULL,
    [Sale_Multiple]        SMALLMONEY NULL,
    [Sale_Price]           SMALLMONEY NULL,
    [Sale_Start_Date]      DATETIME   NULL,
    [Sale_End_Date]        DATETIME   NULL,
    [SyncState]            TINYINT    DEFAULT ((0)) NOT NULL,
    [JDA_PricePriority]    SMALLINT   NULL,
    [IRMA_PriceChgType_ID] INT        NULL,
    CONSTRAINT [PK_JDA_PriceSync] PRIMARY KEY CLUSTERED ([JDA_PriceSync_ID] ASC)
);

