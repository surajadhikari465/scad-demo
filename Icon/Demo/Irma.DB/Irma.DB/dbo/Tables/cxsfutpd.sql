CREATE TABLE [dbo].[cxsfutpd] (
    [pzone]      SMALLINT       NULL,
    [dept]       CHAR (4)       NULL,
    [upcno]      CHAR (13)      NULL,
    [user]       CHAR (4)       NULL,
    [program]    CHAR (8)       NULL,
    [store]      SMALLINT       NULL,
    [ptype]      CHAR (3)       NULL,
    [fut_pm]     SMALLINT       NULL,
    [fut_price]  NUMERIC (6, 2) NULL,
    [start_date] DATETIME       NULL,
    [end_date]   DATETIME       NULL,
    [target_gm]  NUMERIC (5, 2) NULL
);

