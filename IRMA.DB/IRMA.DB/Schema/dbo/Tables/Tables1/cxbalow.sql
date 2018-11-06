CREATE TABLE [dbo].[cxbalow] (
    [vendor]    CHAR (8)       NULL,
    [warehouse] CHAR (12)      NULL,
    [upcno]     CHAR (13)      NULL,
    [allamnt]   NUMERIC (7, 3) NULL,
    [allstart]  DATETIME       NULL,
    [allend]    DATETIME       NULL,
    [allcode]   CHAR (1)       NULL,
    [allregstr] INT            NULL,
    [allaction] CHAR (1)       NULL,
    [store]     SMALLINT       NULL
);

