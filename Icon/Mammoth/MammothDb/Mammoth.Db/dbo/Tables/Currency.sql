CREATE TABLE [dbo].[Currency] (
    [CurrencyID]	INT IDENTITY (1, 1) NOT NULL,
    [CurrencyCode]	NVARCHAR (5) NOT NULL,
    [CurrencyDesc]	NVARCHAR (25) NOT NULL,
    [IssuingEntity] NVARCHAR(255) NULL,
    [NumericCode]	INT NULL,
    [MinorUnit]		INT NULL,
    [Symbol]		NVARCHAR(3) NULL, 
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([CurrencyID] ASC) WITH (FILLFACTOR = 100)
);
GO

CREATE NONCLUSTERED INDEX IX_CurrencyCode_Currency on [dbo].[Currency] (CurrencyCode)
GO