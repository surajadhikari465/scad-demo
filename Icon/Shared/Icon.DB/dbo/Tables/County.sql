CREATE TABLE [dbo].[County] 
(
	[countyID] INT NOT NULL IDENTITY,
	[countyName] NVARCHAR(255)  NULL,
	[territoryID] INT  NOT NULL  
)
GO
ALTER TABLE [dbo].[County] WITH CHECK ADD CONSTRAINT [Territory_County_FK1] FOREIGN KEY ([territoryID])
REFERENCES [dbo].[Territory] ([territoryID])
GO
ALTER TABLE [dbo].[County] ADD CONSTRAINT [County_PK] PRIMARY KEY CLUSTERED ([countyID]) WITH (FILLFACTOR = 80)