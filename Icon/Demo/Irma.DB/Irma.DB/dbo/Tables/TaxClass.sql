CREATE TABLE [dbo].[TaxClass] (
    [TaxClassID]           INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TaxClassDesc]         VARCHAR (50) NOT NULL,
    [ExternalTaxGroupCode] VARCHAR (7)  NULL,
    CONSTRAINT [PK_TaxClass] PRIMARY KEY CLUSTERED ([TaxClassID] ASC)
);





GO
ALTER TABLE [dbo].[TaxClass] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [_dta_IX_TaxClass_TaxClassDesc]
    ON [dbo].[TaxClass]([TaxClassDesc] ASC)
    INCLUDE([TaxClassID]);


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxClass] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxClass] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxClass] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxClass] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxClass] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxClass] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxClass] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxClass] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxClass] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxClass] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxClass] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxClass] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxClass] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxClass] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxClass] TO [iCONReportingRole]
    AS [dbo];

