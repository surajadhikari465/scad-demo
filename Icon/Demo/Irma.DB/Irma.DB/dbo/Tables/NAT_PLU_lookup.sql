CREATE TABLE [dbo].[NAT_PLU_lookup] (
    [upcno]        VARCHAR (30) NOT NULL,
    [nat_plu]      VARCHAR (30) NOT NULL,
    [longdesc]     CHAR (30)    NULL,
    [last_updated] DATETIME     NOT NULL,
    [natpluID]     INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    CONSTRAINT [PK_NAT_PLU_lookup] PRIMARY KEY CLUSTERED ([natpluID] ASC) WITH (FILLFACTOR = 80)
);

