CREATE TABLE [dbo].[Locales_FL] (
    [Region]			NCHAR (2)		DEFAULT ('FL') NOT NULL,
    [LocaleID]			INT				IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
    CONSTRAINT [PK_Locales_FL] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_FL],
    CONSTRAINT [CK_Locales_FL_Region] CHECK ([Region] = 'FL')
);

GO

GRANT SELECT ON [dbo].[Locales_FL] TO [TibcoRole]
GO