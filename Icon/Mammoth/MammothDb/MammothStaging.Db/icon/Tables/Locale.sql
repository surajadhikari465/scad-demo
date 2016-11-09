CREATE TABLE [icon].[Locale] (
    [localeID]        INT            NOT NULL,
    [ownerOrgPartyID] INT            NOT NULL,
    [localeName]      NVARCHAR (255) NOT NULL,
    [localeOpenDate]  DATE           NULL,
    [localeCloseDate] DATE           NULL,
    [localeTypeID]    INT            NOT NULL,
    [parentLocaleID]  INT            NULL,
    CONSTRAINT [PK_Locale] PRIMARY KEY CLUSTERED ([localeID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [AK_OrgRegionLocale] UNIQUE NONCLUSTERED ([ownerOrgPartyID] ASC, [parentLocaleID] ASC, [localeName] ASC) WITH (FILLFACTOR = 100)
);

