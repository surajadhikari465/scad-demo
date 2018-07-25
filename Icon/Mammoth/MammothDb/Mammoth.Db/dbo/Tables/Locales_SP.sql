CREATE TABLE [dbo].[Locales_SP] (
    [Region]			NCHAR (2)		DEFAULT ('SP') NOT NULL,
    [LocaleID]			INT				IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
    CONSTRAINT [PK_Locales_SP] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_SP],
    CONSTRAINT [CK_Locales_SP_Region] CHECK ([Region] = 'SP')
);

GO

GRANT SELECT ON [dbo].[Locales_SP] TO [TibcoRole]
GO
