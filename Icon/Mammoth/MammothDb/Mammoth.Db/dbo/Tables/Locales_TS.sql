CREATE TABLE [dbo].[Locales_TS](
	[Region]			[nchar](2)	DEFAULT ('TS')		NOT NULL,
	[LocaleID]			[int]		IDENTITY(1,1)		NOT NULL,
	[BusinessUnitID]	[int]							NOT NULL,
	[StoreName]			[nvarchar](255)					NOT NULL,
	[StoreAbbrev]		[nvarchar](5)					NOT NULL,
	[PhoneNumber]		[nvarchar](255)					NULL,
	[AddedDate]			[datetime]	DEFAULT (getdate()) NOT NULL,
	[ModifiedDate]		[datetime]						NULL,
	CONSTRAINT [PK_Locales_TS] PRIMARY KEY CLUSTERED ([Region] ASC,	[LocaleID] ASC) WITH (FILLFACTOR = 100) ON [FG_RM],
    CONSTRAINT [CK_Locales_TS_Region] CHECK ([Region] = 'TS')
);

GO

GRANT SELECT ON [dbo].[Locales_TS] TO [TibcoRole]
GO


