CREATE TABLE [app].[Settings](
	[SettingsId] [int] IDENTITY(1,1) NOT NULL,
	[SettingSectionId] [int] NOT NULL,
	[KeyName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [SettingsPK_Id] PRIMARY KEY CLUSTERED 
(
	[SettingsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [app].[Settings]  WITH CHECK ADD FOREIGN KEY([SettingSectionId])
REFERENCES [app].[SettingSection] ([SettingSectionId])
GO



CREATE INDEX [Settings_SettingSectionId_Index] ON [app].[Settings] (SettingSectionId)

GO


CREATE UNIQUE INDEX [SettingSectionId_KeyNameUnique_Index] ON [app].[Settings] (SettingSectionId, KeyName)

GO