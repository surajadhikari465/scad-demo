﻿CREATE TABLE [dbo].[JDA_TRFDTL] (
    [TRFBCH] CHAR (8)       NOT NULL,
    [TRFFLC] DECIMAL (5)    NULL,
    [TRFTLC] DECIMAL (5)    NULL,
    [INUMBR] DECIMAL (9)    NOT NULL,
    [ASNUM]  NUMERIC (6)    NULL,
    [IDEPT]  NUMERIC (3)    NULL,
    [ISDEPT] NUMERIC (3)    NULL,
    [ICLAS]  NUMERIC (3)    NULL,
    [ISCLAS] NUMERIC (3)    NULL,
    [TRFREQ] DECIMAL (7)    NULL,
    [TRFSHP] DECIMAL (7)    NULL,
    [TRFREC] DECIMAL (7)    NULL,
    [TRFALC] DECIMAL (7)    NULL,
    [TRFSUB] DECIMAL (9)    NULL,
    [TRFSCD] CHAR (1)       NULL,
    [TRFRLC] CHAR (8)       NULL,
    [TRFCST] DECIMAL (9, 3) NULL,
    [TRFRIN] DECIMAL (9, 2) NULL,
    [TRFROU] DECIMAL (9, 2) NULL,
    [ICUBE]  DECIMAL (9, 3) NULL,
    [IVNDP#] CHAR (15)      NULL,
    [TRFSTS] CHAR (1)       NULL,
    [TRFTYP] CHAR (1)       NULL,
    [TRFPTY] CHAR (1)       NULL,
    [TDCYCL] DECIMAL (5)    NULL,
    [TDSCWV] NUMERIC (2)    NULL,
    [TDSCSQ] NUMERIC (3)    NULL,
    [TDSCNM] CHAR (8)       NULL,
    [TRSVND] NUMERIC (6)    NULL,
    [TRSTYL] CHAR (15)      NULL,
    [TRSCOL] NUMERIC (4)    NULL,
    [TRSSIZ] CHAR (4)       NULL,
    [TRSDIM] CHAR (4)       NULL,
    [TRFDSP] CHAR (1)       NULL,
    CONSTRAINT [PK_JDA_TRFDTL] PRIMARY KEY NONCLUSTERED ([TRFBCH] ASC, [INUMBR] ASC)
);

