CREATE TABLE [dbo].[Price_SO] (
    [Region]         NCHAR (2)    DEFAULT ('SO') NOT NULL,
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
    CONSTRAINT [PK_Price_SO] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_SO],
	CONSTRAINT [CK_Price_SO_Region] CHECK (Region = 'SO')
) ON [FG_SO]

GO
CREATE CLUSTERED INDEX [CIX_Price_SO]
    ON [dbo].[Price_SO]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_SO];


GO

CREATE NONCLUSTERED INDEX [IX_Price_SO_ItemID] ON [dbo].[Price_SO]
(
       [ItemID] ASC,
       [BusinessUnitID] ASC,
       [StartDate] ASC,
       [PriceType] ASC,
       [Region] ASC,
       [PriceID] ASC
)
INCLUDE ([AddedDate]) ON [FG_SO]
