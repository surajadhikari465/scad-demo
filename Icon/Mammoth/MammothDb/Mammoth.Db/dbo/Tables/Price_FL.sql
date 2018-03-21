﻿CREATE TABLE [dbo].[Price_FL] (
    [Region]         NCHAR (2)    DEFAULT ('FL') NOT NULL,
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
    CONSTRAINT [PK_Price_FL] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_FL],
	CONSTRAINT [CK_Price_FL_Region] CHECK (Region = 'FL')
) ON [FG_FL]

GO
CREATE CLUSTERED INDEX [CIX_Price_FL]
    ON [dbo].[Price_FL]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_FL];


GO

CREATE NONCLUSTERED INDEX [IX_Price_FL_ItemID] ON [dbo].[Price_FL]
(
       [ItemID] ASC,
       [BusinessUnitID] ASC,
       [StartDate] ASC,
       [PriceType] ASC,
       [Region] ASC,
       [PriceID] ASC
)
INCLUDE ([AddedDate]) ON [FG_FL]
