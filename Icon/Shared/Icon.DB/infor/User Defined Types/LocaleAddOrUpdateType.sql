CREATE TYPE [infor].[LocaleAddOrUpdateType] AS TABLE
(	[LocaleID] INT NOT NULL,
	[LocaleName] VARCHAR(25) NOT NULL,
	[LocaleOpenDate] [datetime2](7) NULL,
	[LocaleCloseDate] [datetime2](7) NULL,
	[LocaleTypeCode] NVARCHAR(3) NOT NULL,
	[ParentLocaleID] INT NULL,
	[BusinessUnitId] INT NULL,
	[EwicAgency] NVARCHAR(255) NULL
)