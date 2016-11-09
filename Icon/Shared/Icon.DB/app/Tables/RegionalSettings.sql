CREATE TABLE [app].[RegionalSettings](
	[RegionalSettingsId] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[SettingsId] [int] NOT NULL,
	[Value] [bit] NOT NULL,
CONSTRAINT [RegionalSettingsPK_Id] PRIMARY KEY CLUSTERED 
(
	[RegionalSettingsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [app].RegionalSettings  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [app].[Regions] ([RegionId])
GO

ALTER TABLE [app].RegionalSettings  WITH CHECK ADD FOREIGN KEY([SettingsId])
REFERENCES [app].[Settings] ([SettingsId])
GO

CREATE INDEX [regionalsettings_SettingsId_Index] ON [app].[RegionalSettings](SettingsId)
GO

CREATE INDEX [regionalsettings_SettingsRegionaID_Index] ON [app].[RegionalSettings](SettingsId, RegionId)
GO

CREATE UNIQUE INDEX [SettingsId_RegionIdUnique_Index]  ON [app].[RegionalSettings](SettingsId, RegionId)
GO