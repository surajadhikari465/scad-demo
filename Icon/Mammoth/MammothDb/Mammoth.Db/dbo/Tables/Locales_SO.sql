CREATE TABLE [dbo].[Locales_SO] (
    [Region]			NCHAR (2)		DEFAULT ('SO') NOT NULL,
    [LocaleID]			INT				IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
    CONSTRAINT [PK_Locales_SO] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_SO],
    CONSTRAINT [CK_Locales_SO_Region] CHECK ([Region] = 'SO')
);

GO

GRANT SELECT ON [dbo].[Locales_SO] TO [TibcoRole]
GO

GRANT SELECT ON [dbo].[Locales_SO] TO [MammothRole]
GO

CREATE NONCLUSTERED INDEX [IX_Locales_SO_BusinessUnitId]
    ON [dbo].[Locales_SO]([BusinessUnitID] ASC) WITH (FILLFACTOR = 80)
    ON [FG_SO];
GO