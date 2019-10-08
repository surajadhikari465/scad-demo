CREATE TABLE [dbo].[Locales_FL_Audit] (
    [Region]			NCHAR (2)		DEFAULT ('FL') NOT NULL,
    [LocaleID]			INT				NOT NULL,
    [BusinessUnitID]	INT				NOT NULL,
    [StoreName]			NVARCHAR (255)	NOT NULL,
    [StoreAbbrev]		NVARCHAR (5)	NOT NULL,
	[PhoneNumber]		NVARCHAR (255)	NULL,
	[LocaleOpenDate]	DATETIME		NULL,
	[LocaleCloseDate]	DATETIME		NULL,
    [AddedDate]			DATETIME		DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]		DATETIME		NULL,
    CONSTRAINT [PK_Locales_FL_Audit] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [CK_Locales_FL_Audit_Region] CHECK ([Region] = 'FL')
);
GO