CREATE TABLE [dbo].[TaxDefinition] (
    [TaxJurisdictionID] INT            NOT NULL,
    [TaxFlagKey]        CHAR (1)       NOT NULL,
    [TaxPercent]        DECIMAL (9, 4) NULL,
    [POSID]             INT            NULL,
    CONSTRAINT [PK_TaxCodeDef] PRIMARY KEY CLUSTERED ([TaxFlagKey] ASC, [TaxJurisdictionID] ASC),
    CONSTRAINT [FK_TaxDefinition_TaxJurisdiction] FOREIGN KEY ([TaxJurisdictionID]) REFERENCES [dbo].[TaxJurisdiction] ([TaxJurisdictionID]) ON DELETE CASCADE
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxDefinition] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxDefinition] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxDefinition] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxDefinition] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxDefinition] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxDefinition] TO [IRMAReportsRole]
    AS [dbo];

