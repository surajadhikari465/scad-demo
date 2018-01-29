CREATE TABLE [dbo].[LocaleAttributes_PN] (
    [Region]            NCHAR (2)      DEFAULT ('PN') NOT NULL,
    [LocaleAttributeID] INT            IDENTITY (1, 1) NOT NULL,
	[LocaleID]          INT            NOT NULL,
    [AttributeID]       INT            NULL,
    [AttributeValue]    NVARCHAR (255) NULL,
    [AddedDate]         DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]      DATETIME       NULL,
    CONSTRAINT [PK_LocaleAttributes_PN] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleAttributeID] ASC) WITH (FILLFACTOR = 100) ON [FG_PN]
);
