CREATE TABLE [irma].[Store] (
    [Region]              NCHAR (2)    NOT NULL,
    [Store_No]            INT          NOT NULL,
    [Store_Name]          VARCHAR (50) NOT NULL,
    [BusinessUnit_ID]     INT          NULL,
    [StoreJurisdictionID] INT          NULL,
    [WFM_Store]           BIT          NULL,
    [Mega_Store]          BIT          NULL,
    [Internal]            BIT          NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([Region] ASC, [Store_No] ASC) WITH (FILLFACTOR = 100)
);



