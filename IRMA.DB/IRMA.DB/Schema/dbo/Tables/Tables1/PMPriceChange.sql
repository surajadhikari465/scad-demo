CREATE TABLE [dbo].[PMPriceChange] (
    [PMPriceChangeID] INT           IDENTITY (1, 1) NOT NULL,
    [Item_Key]        VARCHAR (255) NULL,
    [Price]           VARCHAR (255) NULL,
    [Org_Level]       VARCHAR (255) NULL,
    [Level_ID]        VARCHAR (255) NULL,
    [InsertDate]      DATETIME      CONSTRAINT [DF__PMPriceCh__Inser__10B5C857] DEFAULT (getdate()) NOT NULL,
    [AppliedDate]     DATETIME      NULL,
    CONSTRAINT [PK_PMPriceChange] PRIMARY KEY CLUSTERED ([PMPriceChangeID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMPriceChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMPriceChange] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMPriceChange] TO [IRMAReportsRole]
    AS [dbo];

