CREATE TABLE [dbo].[Locales_TS](
	[Region]			NCHAR(2)		DEFAULT ('TS')		NOT NULL,
    [LocaleID]			INT				IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
	CONSTRAINT [PK_Locales_TS] PRIMARY KEY CLUSTERED ([Region] ASC,	[LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_RM],
    CONSTRAINT [CK_Locales_TS_Region] CHECK ([Region] = 'TS')
);

GO

GRANT SELECT ON [dbo].[Locales_TS] TO [TibcoRole]
GO

GRANT SELECT ON [dbo].[Locales_TS] TO [MammothRole]
GO

CREATE NONCLUSTERED INDEX [IX_Locales_TS_BusinessUnitId]
    ON [dbo].[Locales_TS]([BusinessUnitID] ASC) WITH (FILLFACTOR = 80);
    
GO