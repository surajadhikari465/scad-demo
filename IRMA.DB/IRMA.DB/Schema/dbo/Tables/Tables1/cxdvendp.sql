CREATE TABLE [dbo].[cxdvendp] (
    [vendno]        CHAR (8)       NULL,
    [diffcode]      CHAR (1)       NULL,
    [deftype]       CHAR (1)       NULL,
    [defamt]        DECIMAL (6, 2) NULL,
    [totind]        CHAR (1)       NULL,
    [retext]        CHAR (1)       NULL,
    [hiptype]       CHAR (3)       NULL,
    [vchg_amt]      DECIMAL (8, 4) NULL,
    [vchg_per]      DECIMAL (4, 2) NULL,
    [dept_adj_flag] CHAR (1)       NULL,
    [dept_adj]      CHAR (4)       NULL,
    [ap_vendno]     CHAR (8)       NULL
);

