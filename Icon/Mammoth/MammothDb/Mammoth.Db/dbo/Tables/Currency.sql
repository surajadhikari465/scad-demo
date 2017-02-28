CREATE TABLE [dbo].[Currency] (
    [CurrencyID]   INT        IDENTITY (1, 1) NOT NULL,
    [CurrencyCode] NVARCHAR (5)  NOT NULL,
    [CurrencyDesc] NVARCHAR (25) NOT NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([CurrencyID] ASC) WITH (FILLFACTOR = 100)
);
GO

CREATE NONCLUSTERED INDEX IX_CurrencyCode_Currency on [dbo].[Currency] (CurrencyCode)
GO