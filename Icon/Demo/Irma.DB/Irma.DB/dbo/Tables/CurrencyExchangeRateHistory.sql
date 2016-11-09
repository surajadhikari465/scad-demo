CREATE TABLE [dbo].[CurrencyExchangeRateHistory] (
    [FromCurrency] VARCHAR (3) NOT NULL,
    [ToCurrency]   VARCHAR (3) NOT NULL,
    [Multiplier]   FLOAT (53)  NOT NULL,
    [Divider]      FLOAT (53)  NOT NULL,
    [Date]         DATETIME    NOT NULL,
    [UploadDate]   DATETIME    NOT NULL
);

