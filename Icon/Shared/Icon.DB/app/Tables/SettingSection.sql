CREATE TABLE [app].[SettingSection](
	[SettingSectionId] [int] IDENTITY(1,1) NOT NULL,
	[SectionName] [nvarchar](255) NOT NULL unique,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [SettingSectionPK_Id] PRIMARY KEY CLUSTERED 
(
	[SettingSectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]


GO



CREATE INDEX [SectionName_Index]  ON [app].[SettingSection] ([SectionName])
