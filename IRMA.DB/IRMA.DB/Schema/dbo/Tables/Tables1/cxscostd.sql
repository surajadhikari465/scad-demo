﻿CREATE TABLE [dbo].[cxscostd] (
    [vendor]      CHAR (8)       NULL,
    [warehouse]   CHAR (12)      NULL,
    [upcno]       CHAR (13)      NULL,
    [casecost]    NUMERIC (8, 4) NULL,
    [chgdate]     DATETIME       NULL,
    [targetgm]    DECIMAL (5, 2) NULL,
    [caseno]      CHAR (13)      NULL,
    [avgcost]     NUMERIC (8, 4) NULL,
    [casesize]    SMALLINT       NULL,
    [vendseq]     CHAR (5)       NULL,
    [qtybrk1]     SMALLINT       NULL,
    [costbrk1]    NUMERIC (7, 4) NULL,
    [qtybrk2]     SMALLINT       NULL,
    [costbrk2]    NUMERIC (7, 4) NULL,
    [qtybrk3]     SMALLINT       NULL,
    [costbrk3]    NUMERIC (7, 4) NULL,
    [deposit]     NUMERIC (5, 2) NULL,
    [splitcharge] NUMERIC (7, 4) NULL,
    [futcost]     NUMERIC (8, 4) NULL,
    [futdate]     DATETIME       NULL,
    [lastcost]    NUMERIC (8, 4) NULL,
    [store]       SMALLINT       NULL,
    [shellchg]    DECIMAL (6, 2) NULL,
    [handchg]     DECIMAL (5, 4) NULL
);

