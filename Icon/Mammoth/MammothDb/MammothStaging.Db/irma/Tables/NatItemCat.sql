CREATE TABLE [irma].[NatItemCat] (
    [Region]              NCHAR (2)    NOT NULL,
    [NatCatID]            INT          NOT NULL,
    [NatCatName]          VARCHAR (65) NULL,
    [NatFamilyID]         INT          NOT NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_NatItemCat] PRIMARY KEY CLUSTERED ([Region] ASC, [NatCatID] ASC) WITH (FILLFACTOR = 100)
);



