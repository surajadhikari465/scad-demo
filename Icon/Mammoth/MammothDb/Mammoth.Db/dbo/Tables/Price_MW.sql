CREATE TABLE [dbo].[Price_MW] (
    [Region]         NCHAR (2)    DEFAULT ('MW') NOT NULL,
    [PriceID]        INT          IDENTITY (1, 1) NOT NULL,
    [ItemID]         INT          NOT NULL,
    [BusinessUnitID] INT          NOT NULL,
    [StartDate]      DATETIME     NOT NULL,
    [EndDate]        DATETIME     NULL,
    [Price]          SMALLMONEY   NOT NULL,
    [PriceType]      NVARCHAR (3) NOT NULL,
    [PriceUOM]       NVARCHAR (3) NOT NULL,
    [CurrencyID]     INT          NOT NULL,
    [Multiple]       TINYINT      NOT NULL,
    [AddedDate]      DATETIME     DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]   DATETIME     NULL,
    CONSTRAINT [PK_Price_MW] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_MW],
	CONSTRAINT [CK_Price_MW_Region] CHECK (Region = 'MW')
) ON [FG_MW]

GO
CREATE CLUSTERED INDEX [CIX_Price_MW]
    ON [dbo].[Price_MW]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_MW];


GO

CREATE NONCLUSTERED INDEX [IX_Price_MW_ItemID] ON [dbo].[Price_MW]
(
       [ItemID] ASC,
       [BusinessUnitID] ASC,
       [StartDate] ASC,
       [PriceType] ASC,
       [Region] ASC,
       [PriceID] ASC
)
INCLUDE ([AddedDate]) ON [FG_MW]
GO
