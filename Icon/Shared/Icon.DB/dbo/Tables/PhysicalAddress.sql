CREATE TABLE [dbo].[PhysicalAddress] 
(
	[addressID] INT  NOT NULL  
	, [countryID] int NULL  
	, [territoryID] INT  NULL  
	, [cityID] INT  NULL  
	, [postalCodeID] INT  NULL  
	, [latitude] decimal(9,6)  NULL  
	, [longitude] decimal(9,6)  NULL  
	, [addressLine1] NVARCHAR(255)  NULL  
	, [addressLine2] NVARCHAR(255)  NULL  
	, [addressLine3] NVARCHAR(255)  NULL  
	, [timezoneID] INT  NULL  
)
GO
ALTER TABLE [dbo].[PhysicalAddress] WITH CHECK ADD CONSTRAINT [Address_PhysicalAddress_FK1] FOREIGN KEY ([addressID])
REFERENCES [dbo].[Address] ([addressID])
GO
ALTER TABLE [dbo].[PhysicalAddress] WITH CHECK ADD CONSTRAINT [Timezone_PhysicalAddress_FK1] FOREIGN KEY ([timezoneID])
REFERENCES [dbo].[Timezone] ([timezoneID])
GO
ALTER TABLE [dbo].[PhysicalAddress] WITH CHECK ADD CONSTRAINT [PostalCode_PhysicalAddress_FK1] FOREIGN KEY ([postalCodeID])
REFERENCES [dbo].[PostalCode] ([postalCodeID])
GO
ALTER TABLE [dbo].[PhysicalAddress] WITH CHECK ADD CONSTRAINT [Country_PhysicalAddress_FK1] FOREIGN KEY ([countryID])
REFERENCES [dbo].[Country] ([countryID])
GO
ALTER TABLE [dbo].[PhysicalAddress] WITH CHECK ADD CONSTRAINT [Territory_PhysicalAddress_FK1] FOREIGN KEY ([territoryID])
REFERENCES [dbo].[Territory] ([territoryID])
GO
ALTER TABLE [dbo].[PhysicalAddress] WITH CHECK ADD CONSTRAINT [City_PhysicalAddress_FK1] FOREIGN KEY ([cityID])
REFERENCES [dbo].[City] ([cityID])
GO
ALTER TABLE [dbo].[PhysicalAddress] ADD CONSTRAINT [PhysicalAddress_PK] PRIMARY KEY CLUSTERED ([addressID]) WITH (FILLFACTOR = 80)