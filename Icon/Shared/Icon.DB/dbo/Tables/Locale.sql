CREATE TABLE [dbo].[Locale] (
[localeID] INT  NOT NULL IDENTITY  
, [ownerOrgPartyID] INT  NOT NULL  
, [localeName] NVARCHAR(255)  NOT NULL  
, [localeOpenDate] DATE  NULL  
, [localeCloseDate] DATE  NULL  
, [localeTypeID] INT NOT NULL  
, [parentLocaleID] INT  NULL  
)
GO
ALTER TABLE [dbo].[Locale] WITH CHECK ADD CONSTRAINT [Organization_Locale_FK1] FOREIGN KEY (
[ownerOrgPartyID]
)
REFERENCES [dbo].[Organization] (
[orgPartyID]
)
GO
ALTER TABLE [dbo].[Locale] WITH CHECK ADD CONSTRAINT [Locale_Locale_FK1] FOREIGN KEY (
[parentLocaleID]
)
REFERENCES [dbo].[Locale] (
[localeID]
)
GO
ALTER TABLE [dbo].[Locale] WITH CHECK ADD CONSTRAINT [LocaleType_Locale_FK1] FOREIGN KEY (
[localeTypeID]
)
REFERENCES [dbo].[LocaleType] (
[localeTypeID]
)
GO
ALTER TABLE [dbo].[Locale] ADD CONSTRAINT [AK_OrgRegionLocale] UNIQUE NONCLUSTERED (
[ownerOrgPartyID] ASC
,[parentLocaleID] ASC
, [localeName] ASC
)
GO
ALTER TABLE [dbo].[Locale] ADD CONSTRAINT [Locale_PK] PRIMARY KEY CLUSTERED (
[localeID]
)