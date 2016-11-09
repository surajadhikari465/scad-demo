CREATE TABLE [irma].[Currency] (
    [CurrencyID]   INT           IDENTITY (1, 1) NOT NULL,
    [CurrencyCode] CHAR (3)      NOT NULL,
    [CurrencyName] VARCHAR (255) NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([CurrencyID] ASC) WITH (FILLFACTOR = 100)
);

