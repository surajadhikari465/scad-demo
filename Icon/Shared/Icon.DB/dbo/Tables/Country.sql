CREATE TABLE [dbo].[Country] 
(
	[countryID] int NOT NULL IDENTITY,
	[countryCode] nvarchar(3)  NOT NULL,  
	[countryName] NVARCHAR(255)  NULL  
	CONSTRAINT [Country_PK] PRIMARY KEY CLUSTERED ([countryID])
)
GO
