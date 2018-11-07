CREATE TABLE [dbo].[upc_vendr] (
    [upcno]     CHAR (13) NULL,
    [vendor]    CHAR (30) NULL,
    [store]     SMALLINT  NULL,
    [prim_vend] CHAR (1)  NULL,
    [authrzd]   CHAR (1)  NULL
);


GO
CREATE NONCLUSTERED INDEX [uv_s]
    ON [dbo].[upc_vendr]([store] ASC);


GO
CREATE NONCLUSTERED INDEX [uv_v]
    ON [dbo].[upc_vendr]([vendor] ASC);


GO
CREATE NONCLUSTERED INDEX [uv_vs]
    ON [dbo].[upc_vendr]([vendor] ASC, [store] ASC);


GO
CREATE NONCLUSTERED INDEX [uv_us]
    ON [dbo].[upc_vendr]([upcno] ASC, [store] ASC);

