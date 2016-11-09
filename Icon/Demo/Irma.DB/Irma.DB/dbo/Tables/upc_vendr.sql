CREATE TABLE [dbo].[upc_vendr] (
    [upcno]      CHAR (13)   NULL,
    [vendor]     CHAR (30)   NULL,
    [store]      SMALLINT    NULL,
    [prim_vend]  CHAR (1)    NULL,
    [authrzd]    CHAR (1)    NULL,
    [associated] INT         NOT NULL,
    [warehouse]  CHAR (13)   NULL,
    [Item_Key]   INT         NOT NULL,
    [Vendor_Id]  INT         NOT NULL,
    [subteam_no] CHAR (4)    NULL,
    [state]      VARCHAR (2) NULL
);




GO



GO



GO



GO


