CREATE TABLE [dbo].[TaxJurisdiction] (
    [TaxJurisdictionID]      INT          IDENTITY (1, 1) NOT NULL,
    [TaxJurisdictionDesc]    VARCHAR (30) NOT NULL,
    [LastUpdateUserID]       INT          NULL,
    [RegionalJurisdictionID] VARCHAR (15) NULL,
    [LastUpdate]             DATETIME     DEFAULT ('01/01/2010') NOT NULL,
    CONSTRAINT [PK_TaxJurisdiction] PRIMARY KEY CLUSTERED ([TaxJurisdictionID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxJurisdiction] TO [IRMAReportsRole]
    AS [dbo];

