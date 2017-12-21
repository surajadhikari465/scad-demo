CREATE TABLE [dbo].[Locales_MW] (
    [Region]         NCHAR (2)      DEFAULT ('MW') NOT NULL,
    [LocaleID]       INT            IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID] INT            NOT NULL,
    [StoreName]      NVARCHAR (255) NOT NULL,
    [StoreAbbrev]    NVARCHAR (5)   NOT NULL,
	[PhoneNumber]	 NVARCHAR (255) NULL,
    [AddedDate]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]   DATETIME       NULL,
    CONSTRAINT [PK_Locales_MW] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_MW],
    CONSTRAINT [CK_Locales_MW_Region] CHECK ([Region] = 'MW')
);

GO

GRANT SELECT ON [dbo].[Locales_MW] TO [TibcoRole]
GO