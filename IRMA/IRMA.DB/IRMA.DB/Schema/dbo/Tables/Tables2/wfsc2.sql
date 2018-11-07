CREATE TABLE [dbo].[wfsc2] (
    [scale_upc]  CHAR (5)       NULL,
    [upcno]      CHAR (13)      NULL,
    [store]      SMALLINT       NULL,
    [scale_uom]  CHAR (2)       NULL,
    [by_count]   SMALLINT       NULL,
    [fs_type]    CHAR (1)       NULL,
    [fs_value]   NUMERIC (6, 2) NULL,
    [exc_price]  NUMERIC (5, 2) NULL,
    [fixed_wght] INT            NULL,
    [shelf_life] SMALLINT       NULL,
    [use_by]     SMALLINT       NULL,
    [tare]       SMALLINT       NULL,
    [ingr_no]    CHAR (6)       NULL,
    [status]     CHAR (1)       NULL
);

