CREATE TABLE [dbo].[Territory] 
(
	[territoryID] int NOT NULL IDENTITY,
	[territoryCode] nvarchar(3)  NOT NULL,
	[territoryName] nvarchar(255) NULL,  
	[countryID] int  NOT NULL

	CONSTRAINT [Territory_PK] PRIMARY KEY CLUSTERED ([territoryID]) WITH (FILLFACTOR = 80),
	CONSTRAINT [Country_Territory_FK1] FOREIGN KEY ([countryID]) REFERENCES [dbo].[Country] ([countryID]),
	CONSTRAINT [Territory_territoryCode_UNQ] UNIQUE ([territoryCode])
)
GO
