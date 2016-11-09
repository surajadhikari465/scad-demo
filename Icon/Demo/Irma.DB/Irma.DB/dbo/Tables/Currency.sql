CREATE TABLE [dbo].[Currency] (
    [CurrencyID]   INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CurrencyCode] CHAR (3)      NOT NULL,
    [CurrencyName] VARCHAR (255) NULL,
    [IsDeleted]    BIT           CONSTRAINT [DF_Currency_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([CurrencyID] ASC) WITH (FILLFACTOR = 80),
    UNIQUE NONCLUSTERED ([CurrencyCode] ASC) WITH (FILLFACTOR = 80)
);

