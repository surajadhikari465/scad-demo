﻿CREATE TABLE [dbo].[ItemAttributes_Locale_SO_Ext] (
    [Region]                NCHAR (2)      DEFAULT ('SO') NOT NULL,
    [ItemAttributeLocaleID] INT            IDENTITY (1, 1) NOT NULL,
    [ItemID]                INT            NOT NULL,
    [LocaleID]              INT            NOT NULL,
    [AttributeID]           INT            NULL,
    [AttributeValue]        NVARCHAR (max) NULL,
    [AddedDate]             DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]          DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Locale_SO_Ext] PRIMARY KEY CLUSTERED ([Region] ASC, [ItemAttributeLocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_SO],
    CONSTRAINT [FK_ItemAttributes_Locale_SO_Ext_ItemID] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Items] ([ItemID])
);
GO

CREATE INDEX [IX_ItemAttributesLocaleExtended_SO_AttributeID_ItemID_LocaleID_Region_ItemAttributeLocaleID] ON [dbo].[ItemAttributes_Locale_SO_Ext]
(
	[AttributeID] ASC,
	[ItemID] ASC,
	[LocaleID] ASC,
	[Region] ASC,
	[ItemAttributeLocaleID] ASC
)
INCLUDE ([AttributeValue],[AddedDate],[ModifiedDate]) ON [PRIMARY]
