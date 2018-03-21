﻿CREATE TABLE [dbo].[ItemAttributes_Locale_NC_Ext] (
    [Region]                NCHAR (2)      DEFAULT ('NC') NOT NULL,
    [ItemAttributeLocaleID] INT            IDENTITY (1, 1) NOT NULL,
    [ItemID]                INT            NOT NULL,
    [LocaleID]              INT            NOT NULL,
    [AttributeID]           INT            NULL,
    [AttributeValue]        NVARCHAR (max) NULL,
    [AddedDate]             DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]          DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Locale_NC_Ext] PRIMARY KEY CLUSTERED ([Region] ASC, [ItemAttributeLocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_NC],
    CONSTRAINT [FK_ItemAttributes_Locale_NC_Ext_ItemID] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Items] ([ItemID]),
	CONSTRAINT [CK_ItemAttributes_Locale_NC_Ext_Region] CHECK ([Region] = 'NC')
) ON [FG_NC];
GO

CREATE INDEX [IX_ItemAttributesLocaleExtended_NC_AttributeID_ItemID_LocaleID_Region_ItemAttributeLocaleID] ON [dbo].[ItemAttributes_Locale_NC_Ext]
(
	[AttributeID] ASC,
	[ItemID] ASC,
	[LocaleID] ASC,
	[Region] ASC,
	[ItemAttributeLocaleID] ASC
)
INCLUDE ([AttributeValue],[AddedDate],[ModifiedDate]) ON [FG_NC]
