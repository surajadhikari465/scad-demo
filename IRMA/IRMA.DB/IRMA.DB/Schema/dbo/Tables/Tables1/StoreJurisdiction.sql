CREATE TABLE [dbo].[StoreJurisdiction] (
    [StoreJurisdictionID]   INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [StoreJurisdictionDesc] VARCHAR (30) NOT NULL,
    [CurrencyID]            INT          NULL,
    CONSTRAINT [PK_StoreJurisdiction] PRIMARY KEY CLUSTERED ([StoreJurisdictionID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreJurisdiction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreJurisdiction] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreJurisdiction] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreJurisdiction] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreJurisdiction] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreJurisdiction] TO [IRMASLIMRole]
    AS [dbo];

