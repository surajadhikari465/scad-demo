CREATE TABLE [dbo].[cxbdisd] (
    [vendor]    CHAR (8)       NULL,
    [warehouse] CHAR (12)      NULL,
    [upcno]     CHAR (13)      NULL,
    [discamt]   NUMERIC (6, 2) NULL,
    [discstart] SMALLDATETIME  NULL,
    [discend]   SMALLDATETIME  NULL,
    [discno]    INT            NULL,
    [store]     SMALLINT       NULL
);

