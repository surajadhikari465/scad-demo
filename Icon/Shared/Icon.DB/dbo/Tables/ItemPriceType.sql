CREATE TABLE [dbo].[ItemPriceType] (
[itemPriceTypeID] INT  NOT NULL IDENTITY,  
[itemPriceTypeCode] NVARCHAR(3)  NOT NULL, 
[itemPriceTypeDesc] NVARCHAR(255)  NULL,
CONSTRAINT [AK_itemPriceTypeCode_itemPriceTypeCode] UNIQUE NONCLUSTERED ([itemPriceTypeCode] ASC) WITH (FILLFACTOR = 80)
)
GO
ALTER TABLE [dbo].[ItemPriceType] ADD CONSTRAINT [ItemPriceType_PK] PRIMARY KEY CLUSTERED (
[itemPriceTypeID]
)