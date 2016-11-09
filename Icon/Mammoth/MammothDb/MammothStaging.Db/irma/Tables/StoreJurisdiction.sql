CREATE TABLE [irma].[StoreJurisdiction] (
    [Region]                NCHAR (2)    NOT NULL,
    [StoreJurisdictionID]   INT          NOT NULL,
    [StoreJurisdictionDesc] VARCHAR (30) NOT NULL,
    [CurrencyID]            INT          NULL,
    CONSTRAINT [PK_StoreJurisdiction] PRIMARY KEY CLUSTERED ([Region] ASC, [StoreJurisdictionID] ASC) WITH (FILLFACTOR = 100)
);



