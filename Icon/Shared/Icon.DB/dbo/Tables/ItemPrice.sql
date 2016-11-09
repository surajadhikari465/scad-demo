CREATE TABLE [dbo].[ItemPrice] (
[itemPriceID] INT  NOT NULL IDENTITY  
, [itemID] INT  NOT NULL  
, [localeID] INT  NOT NULL  
, [itemPriceTypeID] INT  NOT NULL  
, [uomID] int  NOT NULL  
, [currencyTypeID] INT  NOT NULL  
, [itemPriceAmt] MONEY  NOT NULL  
, [breakPointStartQty] INT  NULL  
, [breakPointEndQty] INT  NULL  
, [startDate] DATE  NULL  
, [endDate] DATE  NULL  
)
GO
ALTER TABLE [dbo].[ItemPrice] WITH CHECK ADD CONSTRAINT [Locale_ItemPrice_FK1] FOREIGN KEY (
[localeID]
)
REFERENCES [dbo].[Locale] (
[localeID]
)
GO
ALTER TABLE [dbo].[ItemPrice] WITH CHECK ADD CONSTRAINT [ItemPriceType_ItemPrice_FK1] FOREIGN KEY (
[itemPriceTypeID]
)
REFERENCES [dbo].[ItemPriceType] (
[itemPriceTypeID]
)
GO
ALTER TABLE [dbo].[ItemPrice] WITH CHECK ADD CONSTRAINT [Item_ItemPrice_FK1] FOREIGN KEY (
[itemID]
)
REFERENCES [dbo].[Item] (
[itemID]
)
GO
ALTER TABLE [dbo].[ItemPrice] WITH CHECK ADD CONSTRAINT [CurrencyType_ItemPrice_FK1] FOREIGN KEY (
[currencyTypeID]
)
REFERENCES [dbo].[CurrencyType] (
[currencyTypeID]
)
GO
ALTER TABLE [dbo].[ItemPrice] WITH CHECK ADD CONSTRAINT [UOM_ItemPrice_FK1] FOREIGN KEY (
[uomID]
)
REFERENCES [dbo].[UOM] (
[uomID]
)
GO
ALTER TABLE [dbo].[ItemPrice] ADD CONSTRAINT [ItemPrice_PK] PRIMARY KEY CLUSTERED (
[itemPriceID]
)
GO

CREATE NONCLUSTERED INDEX [IX_ItemPrice_itemID_localeID_itemPriceTypeID] ON [dbo].[ItemPrice] ([itemID] ASC, [localeID] ASC, [itemPriceTypeID] ASC)
GO