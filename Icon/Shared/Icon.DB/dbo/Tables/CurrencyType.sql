CREATE TABLE [dbo].[CurrencyType] (
[currencyTypeID] INT  NOT NULL IDENTITY,
[currencyTypeCode] NVARCHAR(3)  NOT NULL, 
[currencyTypeDesc] NVARCHAR(255)  NULL,
CONSTRAINT [AK_currencyTypeCode_currencyTypeCode] UNIQUE NONCLUSTERED ([currencyTypeCode] ASC) WITH (FILLFACTOR = 80)
)
GO
ALTER TABLE [dbo].[CurrencyType] ADD CONSTRAINT [CurrencyType_PK] PRIMARY KEY CLUSTERED (
[currencyTypeID]
)