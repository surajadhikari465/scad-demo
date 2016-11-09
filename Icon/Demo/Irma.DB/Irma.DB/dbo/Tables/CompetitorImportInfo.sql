CREATE TABLE [dbo].[CompetitorImportInfo] (
    [CompetitorImportInfoID]    INT            IDENTITY (1, 1) NOT NULL,
    [CompetitorImportSessionID] INT            NOT NULL,
    [Item_Key]                  INT            NULL,
    [CompetitorID]              INT            NULL,
    [CompetitorLocationID]      INT            NULL,
    [CompetitorStoreID]         INT            NULL,
    [FiscalYear]                SMALLINT       NOT NULL,
    [FiscalPeriod]              TINYINT        NOT NULL,
    [PeriodWeek]                TINYINT        NOT NULL,
    [Competitor]                VARCHAR (50)   NOT NULL,
    [Location]                  VARCHAR (50)   NOT NULL,
    [CompetitorStore]           VARCHAR (50)   NOT NULL,
    [UPCCode]                   VARCHAR (50)   NOT NULL,
    [Description]               VARCHAR (250)  NULL,
    [Size]                      DECIMAL (9, 4) NULL,
    [PriceMultiple]             TINYINT        NOT NULL,
    [Price]                     SMALLMONEY     NOT NULL,
    [SaleMultiple]              TINYINT        NULL,
    [Sale]                      SMALLMONEY     NULL,
    [DateChecked]               SMALLDATETIME  NOT NULL,
    [Unit_ID]                   INT            NULL,
    [CheckDate]                 DATETIME       CONSTRAINT [DF_CompetitorImportInfo_CheckDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CompetitorImportInfo] PRIMARY KEY CLUSTERED ([CompetitorImportInfoID] ASC),
    CONSTRAINT [FK_CompetitorImportInfo_Competitor] FOREIGN KEY ([CompetitorID]) REFERENCES [dbo].[Competitor] ([CompetitorID]),
    CONSTRAINT [FK_CompetitorImportInfo_CompetitorImportSession] FOREIGN KEY ([CompetitorImportSessionID]) REFERENCES [dbo].[CompetitorImportSession] ([CompetitorImportSessionID]),
    CONSTRAINT [FK_CompetitorImportInfo_CompetitorLocation] FOREIGN KEY ([CompetitorLocationID]) REFERENCES [dbo].[CompetitorLocation] ([CompetitorLocationID]),
    CONSTRAINT [FK_CompetitorImportInfo_CompetitorStore] FOREIGN KEY ([CompetitorStoreID]) REFERENCES [dbo].[CompetitorStore] ([CompetitorStoreID]),
    CONSTRAINT [FK_CompetitorImportInfo_FiscalWeek] FOREIGN KEY ([FiscalYear], [FiscalPeriod], [PeriodWeek]) REFERENCES [dbo].[FiscalWeek] ([FiscalYear], [FiscalPeriod], [PeriodWeek]),
    CONSTRAINT [FK_CompetitorImportInfo_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_CompetitorImportInfo_ItemUnit] FOREIGN KEY ([Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorImportInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorImportInfo] TO [IRMAReportsRole]
    AS [dbo];

