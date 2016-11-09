CREATE TABLE [dbo].[PriceHistory_Temp] (
    [Item_key]       INT            NOT NULL,
    [store]          SMALLINT       NULL,
    [PriceChgTypeID] TINYINT        NOT NULL,
    [On_Sale]        BIT            NOT NULL,
    [fut_pm]         SMALLINT       NULL,
    [fut_price]      NUMERIC (6, 2) NULL,
    [start_date]     DATETIME       NULL,
    [end_date]       DATETIME       NULL
);


GO
CREATE NONCLUSTERED INDEX [pht_o]
    ON [dbo].[PriceHistory_Temp]([On_Sale] ASC);


GO
CREATE NONCLUSTERED INDEX [pht_isd]
    ON [dbo].[PriceHistory_Temp]([Item_key] ASC, [store] ASC, [start_date] ASC);

