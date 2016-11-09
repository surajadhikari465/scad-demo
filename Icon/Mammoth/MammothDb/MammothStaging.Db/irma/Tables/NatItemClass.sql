CREATE TABLE [irma].[NatItemClass] (
    [ClassID]             INT          NOT NULL,
    [ClassName]           VARCHAR (65) NULL,
    [NatCatID]            INT          NOT NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_NatItemClass] PRIMARY KEY CLUSTERED ([ClassID] ASC) WITH (FILLFACTOR = 100)
);

