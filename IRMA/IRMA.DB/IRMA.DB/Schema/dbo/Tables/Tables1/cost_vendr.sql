CREATE TABLE [dbo].[cost_vendr] (
    [upcno]     CHAR (13)      NULL,
    [vendor]    CHAR (8)       NULL,
    [warehouse] CHAR (12)      NULL,
    [store]     SMALLINT       NULL,
    [avgcost]   NUMERIC (8, 4) NULL,
    [casecost]  NUMERIC (8, 4) NULL,
    [casesize]  SMALLINT       NULL
);


GO
CREATE NONCLUSTERED INDEX [cst_u]
    ON [dbo].[cost_vendr]([upcno] ASC);


GO
CREATE NONCLUSTERED INDEX [cst_v]
    ON [dbo].[cost_vendr]([vendor] ASC);


GO
CREATE NONCLUSTERED INDEX [cst_s]
    ON [dbo].[cost_vendr]([store] ASC);

