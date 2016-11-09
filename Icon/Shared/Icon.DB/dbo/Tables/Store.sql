CREATE TABLE [dbo].[Store] (
[storeID] INT  NOT NULL  
)
GO
ALTER TABLE [dbo].[Store] WITH CHECK ADD CONSTRAINT [Locale_Store_FK1] FOREIGN KEY (
[storeID]
)
REFERENCES [dbo].[Locale] (
[localeID]
)
GO
ALTER TABLE [dbo].[Store] ADD CONSTRAINT [Store_PK] PRIMARY KEY CLUSTERED (
[storeID]
)