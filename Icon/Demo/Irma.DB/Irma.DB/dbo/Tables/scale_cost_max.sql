CREATE TABLE [dbo].[scale_cost_max] (
    [upcno]       CHAR (13)  NULL,
    [text]        CHAR (972) NULL,
    [casesize]    SMALLINT   NULL,
    [forc_tare]   CHAR (1)   NULL,
    [tare]        SMALLINT   NULL,
    [desc1]       CHAR (32)  NULL,
    [desc2]       CHAR (32)  NULL,
    [desc3]       CHAR (32)  NULL,
    [desc4]       CHAR (32)  NULL,
    [scale_uom]   CHAR (2)   NULL,
    [shelf_life]  SMALLINT   NULL,
    [use_by]      SMALLINT   NULL,
    [label_frmt1] SMALLINT   NULL,
    [by_count]    SMALLINT   NULL,
    [fixed_wght]  INT        NULL
);


GO
CREATE NONCLUSTERED INDEX [scm_u]
    ON [dbo].[scale_cost_max]([upcno] ASC);

