CREATE TABLE [dbo].[PostalCode] 
(
	[postalCodeID] int NOT NULL IDENTITY,
	[postalCode] nvarchar(15) NOT NULL,
	[countryID] int NOT NULL,
	[countyID] int NULL
)
GO
ALTER TABLE [dbo].[PostalCode] WITH CHECK ADD CONSTRAINT [County_PostalCode_FK1] FOREIGN KEY ([countyID])
REFERENCES [dbo].[County] ([countyID])
GO
ALTER TABLE [dbo].[PostalCode] WITH CHECK ADD CONSTRAINT [Country_PostalCode_FK1] FOREIGN KEY ([countryID])
REFERENCES [dbo].[Country] ([countryID])
GO
ALTER TABLE [dbo].[PostalCode] WITH CHECK ADD CONSTRAINT [Country_PostalCode_UNQ] UNIQUE ([postalCode], [countryID])
GO
ALTER TABLE [dbo].[PostalCode] ADD CONSTRAINT [PostalCode_PK] PRIMARY KEY CLUSTERED ([postalCodeID])