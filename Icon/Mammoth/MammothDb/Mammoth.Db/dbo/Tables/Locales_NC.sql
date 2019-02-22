CREATE TABLE [dbo].[Locales_NC] (
    [Region]			NCHAR (2)		DEFAULT ('NC') NOT NULL,
    [LocaleID]			INT				IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
    CONSTRAINT [PK_Locales_NC] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_NC],
    CONSTRAINT [CK_Locales_NC_Region] CHECK ([Region] = 'NC')
);

GO

GRANT SELECT ON [dbo].[Locales_NC] TO [TibcoRole]
GO

CREATE NONCLUSTERED INDEX [IX_Locales_NC_BusinessUnitId]
    ON [dbo].[Locales_NC]([BusinessUnitID] ASC) WITH (FILLFACTOR = 80)
    ON [FG_NC];
GO