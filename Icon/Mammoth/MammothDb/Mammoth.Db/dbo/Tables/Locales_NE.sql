CREATE TABLE [dbo].[Locales_NE] (
    [Region]			NCHAR (2)		DEFAULT ('NE') NOT NULL,
    [LocaleID]			INT				IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
    CONSTRAINT [PK_Locales_NE] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_NE],
    CONSTRAINT [CK_Locales_NE_Region] CHECK ([Region] = 'NE')
);

GO

GRANT SELECT ON [dbo].[Locales_NE] TO [TibcoRole]
GO

GRANT SELECT ON [dbo].[Locales_NE] TO [MammothRole]
GO
    
CREATE NONCLUSTERED INDEX [IX_Locales_NE_BusinessUnitId]
    ON [dbo].[Locales_NE]([BusinessUnitID] ASC) WITH (FILLFACTOR = 80)
    ON [FG_NE];
GO