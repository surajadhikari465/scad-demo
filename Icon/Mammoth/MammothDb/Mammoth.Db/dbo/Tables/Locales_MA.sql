CREATE TABLE [dbo].[Locales_MA] (
    [Region]         NCHAR (2)      DEFAULT ('MA') NOT NULL,
    [LocaleID]       INT            IDENTITY (1, 1) NOT NULL,
    [BusinessUnitID] INT            NOT NULL,
    [StoreName]      NVARCHAR (255) NOT NULL,
    [StoreAbbrev]    NVARCHAR (5)   NOT NULL,
	[PhoneNumber]	 NVARCHAR (255) NULL,
    [AddedDate]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]   DATETIME       NULL,
    CONSTRAINT [PK_Locales_MA] PRIMARY KEY CLUSTERED ([Region] ASC, [LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_MA],
    CONSTRAINT [CK_Locales_MA_Region] CHECK ([Region] = 'MA')
);

GO

GRANT SELECT ON [dbo].[Locales_MA] TO [TibcoRole]
GO