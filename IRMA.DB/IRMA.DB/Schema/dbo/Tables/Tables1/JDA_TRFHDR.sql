﻿CREATE TABLE [dbo].[JDA_TRFHDR] (
    [TRFBCH] CHAR (8)        NOT NULL,
    [TRFSTS] CHAR (1)        NULL,
    [TRFTYP] CHAR (1)        NULL,
    [TRFPTY] CHAR (1)        NULL,
    [TRFFLC] DECIMAL (5)     NULL,
    [TRFTLC] DECIMAL (5)     NULL,
    [TRFICN] NUMERIC (1)     NULL,
    [TRFIDT] NUMERIC (6)     NULL,
    [TRFBCN] NUMERIC (1)     NULL,
    [TRFBDT] NUMERIC (6)     NULL,
    [TRFSCN] NUMERIC (1)     NULL,
    [TRFSDT] NUMERIC (6)     NULL,
    [TRFRCN] NUMERIC (1)     NULL,
    [TRFRDT] NUMERIC (6)     NULL,
    [TRFINS] CHAR (30)       NULL,
    [TRFDLC] CHAR (8)        NULL,
    [TRFREF] CHAR (8)        NULL,
    [TRFPYN] CHAR (1)        NULL,
    [TRINIT] CHAR (1)        NULL,
    [TRFPO#] DECIMAL (6)     NULL,
    [TRFBO#] NUMERIC (2)     NULL,
    [THTUNT] DECIMAL (9)     NULL,
    [THTPCK] DECIMAL (9)     NULL,
    [THTPLT] DECIMAL (9)     NULL,
    [THTCTN] DECIMAL (9)     NULL,
    [THTIPK] DECIMAL (9)     NULL,
    [THTRTA] DECIMAL (11, 2) NULL,
    [THTCST] DECIMAL (11, 3) NULL,
    [THTCUB] DECIMAL (11, 3) NULL,
    [THTUNP] DECIMAL (9)     NULL,
    [THTPLP] DECIMAL (9)     NULL,
    [THTCTP] DECIMAL (9)     NULL,
    [THTIPP] DECIMAL (9)     NULL,
    [THTRTP] DECIMAL (11, 2) NULL,
    [THTCSP] DECIMAL (11, 3) NULL,
    [THTPCU] DECIMAL (11, 3) NULL,
    [TRPKLN] DECIMAL (5)     NULL,
    [THCYCL] DECIMAL (5)     NULL,
    [THSCWV] NUMERIC (2)     NULL,
    [THSCSQ] NUMERIC (3)     NULL,
    [THSCNM] CHAR (8)        NULL,
    CONSTRAINT [PK_JDA_TRFHDR] PRIMARY KEY NONCLUSTERED ([TRFBCH] ASC)
);

