CREATE TABLE [dbo].[ItemAttributes_Locale_TS_Ext](
	[Region]				[nchar](2)			DEFAULT ('TS')		NOT NULL,
	[ItemAttributeLocaleID] [int]				IDENTITY (1, 1)		NOT NULL,
	[ItemID]				[int]									NOT NULL,
	[LocaleID]				[int]									NOT NULL,
	[AttributeID]			[int]									NULL,
	[AttributeValue]		[nvarchar](max)							NULL,
	[AddedDate]				[datetime]			DEFAULT (getdate()) NOT NULL,
	[ModifiedDate]			[datetime]								NULL,
   CONSTRAINT [PK_ItemAttributes_Locale_TS_Ext] PRIMARY KEY CLUSTERED ([Region] ASC, [ItemAttributeLocaleID] ASC) 
		WITH (FILLFACTOR = 100) ON [FG_RM],
   CONSTRAINT [FK_ItemAttributes_Locale_TS_Ext_ItemID] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Items] ([ItemID])
);
GO

CREATE INDEX [IX_ItemAttributesLocaleExtended_TS_AttributeID_ItemID_LocaleID_Region_ItemAttributeLocaleID] ON [dbo].[ItemAttributes_Locale_TS_Ext]
(
	[AttributeID] ASC,
	[ItemID] ASC,
	[LocaleID] ASC,
	[Region] ASC,
	[ItemAttributeLocaleID] ASC
)
INCLUDE ([AttributeValue],[AddedDate],[ModifiedDate]) ON [PRIMARY]


