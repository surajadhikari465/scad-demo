CREATE TABLE [dbo].[City] 
(
	[cityID] int NOT NULL IDENTITY,  
	[cityName] NVARCHAR(255) NULL,  
	[territoryID] INT NOT NULL,
	[countyID] INT NOT NULL  
)
GO
ALTER TABLE [dbo].[City] WITH CHECK ADD CONSTRAINT [County_City_FK1] FOREIGN KEY ([countyID])
REFERENCES [dbo].[County] ([countyID])
GO
ALTER TABLE [dbo].[City] WITH CHECK ADD CONSTRAINT [Territory_City_FK1] FOREIGN KEY ([territoryID])
REFERENCES [dbo].[Territory] ([territoryiD])
GO
ALTER TABLE [dbo].[City] ADD CONSTRAINT [City_PK] PRIMARY KEY CLUSTERED ([cityID]) WITH (FILLFACTOR = 80)
GO