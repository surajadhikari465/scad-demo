CREATE TABLE [dbo].[Price_NE] (
    [Region]         NCHAR (2)    DEFAULT ('NE') NOT NULL,
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
    CONSTRAINT [PK_Price_NE] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_NE],
	CONSTRAINT [CK_Price_NE_Region] CHECK (Region = 'NE')
) ON [FG_NE]

GO
CREATE CLUSTERED INDEX [CIX_Price_NE]
    ON [dbo].[Price_NE]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_NE];


GO

CREATE NONCLUSTERED INDEX [IX_Price_NE_ItemID] ON [dbo].[Price_NE]
(
       [ItemID] ASC,
       [BusinessUnitID] ASC,
       [StartDate] ASC,
       [PriceType] ASC,
       [Region] ASC,
       [PriceID] ASC
)
INCLUDE ([AddedDate]) ON [FG_NE]
