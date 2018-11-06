CREATE TABLE [dbo].[tmpCCHTaxFlag] (
    [TaxClassID]        INT      NOT NULL,
    [TaxJurisdictionID] INT      NOT NULL,
    [TaxFlagKey]        CHAR (1) NOT NULL,
    [TaxFlagValue]      BIT      NOT NULL,
    CONSTRAINT [PK_TaxClassCode_TEMP] PRIMARY KEY CLUSTERED ([TaxClassID] ASC, [TaxFlagKey] ASC, [TaxJurisdictionID] ASC) WITH (FILLFACTOR = 80)
);

