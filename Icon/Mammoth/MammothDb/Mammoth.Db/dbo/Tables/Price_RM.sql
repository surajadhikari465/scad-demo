CREATE TABLE [dbo].[Price_RM] (
    [Region]         NCHAR (2)    DEFAULT ('RM') NOT NULL,
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
    CONSTRAINT [PK_Price_RM] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_RM],
	CONSTRAINT [CK_Price_RM_Region] CHECK (Region = 'RM')
) ON [FG_RM]

GO
CREATE CLUSTERED INDEX [CIX_Price_RM]
    ON [dbo].[Price_RM]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_RM];


GO

CREATE NONCLUSTERED INDEX [IX_Price_RM_ItemID] ON [dbo].[Price_RM]
(
       [ItemID] ASC,
       [BusinessUnitID] ASC,
       [StartDate] ASC,
       [PriceType] ASC,
       [Region] ASC,
       [PriceID] ASC
)
INCLUDE ([AddedDate]) ON [FG_RM]
GO
