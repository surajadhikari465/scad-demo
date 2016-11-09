CREATE TABLE [app].[PLUMap]
(
	[itemID] INT NOT NULL,
	[flPLU] nvarchar(11) NULL,
	[maPLU] nvarchar(11) NULL,
	[mwPLU] nvarchar(11) NULL,
	[naPLU] nvarchar(11) NULL,
	[ncPLU] nvarchar(11) NULL,
	[nePLU] nvarchar(11) NULL,
	[pnPLU] nvarchar(11) NULL,
	[rmPLU] nvarchar(11) NULL,
	[soPLU] nvarchar(11) NULL,
	[spPLU] nvarchar(11) NULL,
	[swPLU] nvarchar(11) NULL,
	[ukPLU] nvarchar(11) NULL

)
GO
ALTER TABLE [app].[PLUMap] WITH CHECK ADD CONSTRAINT [Item_PLUMap_ItemID_FK] FOREIGN KEY (
[itemID]
)
REFERENCES [dbo].[Item] (
[itemID]
)
GO
ALTER TABLE [app].[PLUMap] ADD CONSTRAINT [PLUMap_PK] PRIMARY KEY CLUSTERED (
[itemID]
)
GO
