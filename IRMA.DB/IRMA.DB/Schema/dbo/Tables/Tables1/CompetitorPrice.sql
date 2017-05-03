CREATE TABLE [dbo].[CompetitorPrice] (
    [Item_Key]          INT            NOT NULL,
    [CompetitorStoreID] INT            NOT NULL,
    [FiscalYear]        SMALLINT       NOT NULL,
    [FiscalPeriod]      TINYINT        NOT NULL,
    [PeriodWeek]        TINYINT        NOT NULL,
    [Description]       VARCHAR (250)  NULL,
    [PriceMultiple]     TINYINT        NOT NULL,
    [Price]             SMALLMONEY     NOT NULL,
    [SaleMultiple]      TINYINT        NULL,
    [Sale]              SMALLMONEY     NULL,
    [Size]              DECIMAL (9, 4) NOT NULL,
    [UpdateUserID]      INT            NOT NULL,
    [UpdateDateTime]    SMALLDATETIME  NOT NULL,
    [UPCCode]           VARCHAR (50)   NOT NULL,
    [Unit_ID]           INT            NULL,
    [CheckDate]         DATETIME       CONSTRAINT [DF_CompetitorPrice_CheckDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CompetitorPrice] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [CompetitorStoreID] ASC, [FiscalYear] ASC, [FiscalPeriod] ASC, [PeriodWeek] ASC),
    CONSTRAINT [FK_CompetitorPrice_CompetitorStore] FOREIGN KEY ([CompetitorStoreID]) REFERENCES [dbo].[CompetitorStore] ([CompetitorStoreID]),
    CONSTRAINT [FK_CompetitorPrice_FiscalWeek] FOREIGN KEY ([FiscalYear], [FiscalPeriod], [PeriodWeek]) REFERENCES [dbo].[FiscalWeek] ([FiscalYear], [FiscalPeriod], [PeriodWeek]),
    CONSTRAINT [FK_CompetitorPrice_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_CompetitorPrice_ItemUnit] FOREIGN KEY ([Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_CompetitorPrice_Users] FOREIGN KEY ([UpdateUserID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorPrice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorPrice] TO [IRMAReportsRole]
    AS [dbo];

