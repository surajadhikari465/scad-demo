CREATE TABLE [dbo].[ItemAttributes_Locale_FL_Ext_Audit] (
    [Region]                NCHAR (2)      DEFAULT ('FL') NOT NULL,
    [ItemAttributeLocaleID] INT            NOT NULL,
    [ItemID]                INT            NOT NULL,
    [LocaleID]              INT            NOT NULL,
    [AttributeID]           INT            NULL,
    [AttributeValue]        NVARCHAR (max) NULL,
    [AddedDate]             DATETIME       NOT NULL CONSTRAINT [DF_ItemAttributes_Locale_FL_Ext_Audit_AddedDate] DEFAULT GETDATE(),
    [ModifiedDate]          DATETIME       NULL,
    CONSTRAINT [PK_ItemAttributes_Locale_FL_Ext_Audit] PRIMARY KEY CLUSTERED ([Region] ASC, [ItemAttributeLocaleID] ASC) WITH (FILLFACTOR = 100),
	CONSTRAINT [CK_ItemAttributes_Locale_FL_Ext_Audit_Region] CHECK ([Region] = 'FL')
);
GO

CREATE INDEX [IX_ItemAttributesLocaleExtended_FL_Audit_AttributeID_ItemID_LocaleID_Region_ItemAttributeLocaleID] ON [dbo].[ItemAttributes_Locale_FL_Ext_Audit]
(
	[AttributeID] ASC,
	[ItemID] ASC,
	[LocaleID] ASC,
	[Region] ASC,
	[ItemAttributeLocaleID] ASC
)
INCLUDE ([AttributeValue],[AddedDate],[ModifiedDate])
