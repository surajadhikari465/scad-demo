CREATE TABLE [irma].[NatItemFamily] (
    [NatFamilyID]         INT          NOT NULL,
    [NatFamilyName]       VARCHAR (65) NULL,
    [NatSubTeam_No]       INT          NULL,
    [SubTeam_No]          INT          NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_NatItemFamily] PRIMARY KEY CLUSTERED ([NatFamilyID] ASC) WITH (FILLFACTOR = 100)
);

