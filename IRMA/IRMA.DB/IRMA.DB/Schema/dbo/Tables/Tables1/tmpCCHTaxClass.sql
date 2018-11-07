CREATE TABLE [dbo].[tmpCCHTaxClass] (
    [TaxClassID]           INT          NOT NULL,
    [TaxClassDesc]         VARCHAR (50) NOT NULL,
    [ExternalTaxGroupCode] VARCHAR (7)  NULL,
    CONSTRAINT [PK_TaxClass_TEMP] PRIMARY KEY CLUSTERED ([TaxClassID] ASC) WITH (FILLFACTOR = 80)
);

