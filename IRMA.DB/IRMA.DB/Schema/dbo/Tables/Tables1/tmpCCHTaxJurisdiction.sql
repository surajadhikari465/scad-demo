CREATE TABLE [dbo].[tmpCCHTaxJurisdiction] (
    [TaxJurisdictionID]      INT          NOT NULL,
    [TaxJurisdictionDesc]    VARCHAR (30) NOT NULL,
    [LastUpdateUserID]       INT          NULL,
    [RegionalJurisdictionID] VARCHAR (15) NULL,
    [LastUpdate]             DATETIME     NOT NULL,
    CONSTRAINT [PK_TaxJurisdiction_TEMP] PRIMARY KEY CLUSTERED ([TaxJurisdictionID] ASC) WITH (FILLFACTOR = 80)
);

