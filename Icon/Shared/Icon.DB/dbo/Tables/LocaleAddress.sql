CREATE TABLE [dbo].[LocaleAddress] 
(
	[localeID] INT  NOT NULL,
	[addressID] INT  NOT NULL,  
	[addressUsageID] INT  NOT NULL  
)
GO
ALTER TABLE [dbo].[LocaleAddress] WITH CHECK ADD CONSTRAINT [Address_LocaleAddress_FK1] FOREIGN KEY ([addressID])
REFERENCES [dbo].[Address] ([addressID])
GO
ALTER TABLE [dbo].[LocaleAddress] WITH CHECK ADD CONSTRAINT [Locale_LocaleAddress_FK1] FOREIGN KEY ([localeID])
REFERENCES [dbo].[Locale] ([localeID])
GO
ALTER TABLE [dbo].[LocaleAddress] WITH CHECK ADD CONSTRAINT [AddressUsage_LocaleAddress_FK1] FOREIGN KEY ([addressUsageID])
REFERENCES [dbo].[AddressUsage] ([addressUsageID])
GO
ALTER TABLE [dbo].[LocaleAddress] ADD CONSTRAINT [LocaleAddress_PK] PRIMARY KEY CLUSTERED 
(
	[localeID],
	[addressUsageID],
	[addressID]
)