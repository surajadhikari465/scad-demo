CREATE TABLE [dbo].[item_cost_temp] (
    [identifier] VARCHAR (13)  NULL,
    [vendor_key] VARCHAR (10)  NULL,
    [unitcost]   SMALLMONEY    NULL,
    [startdate]  SMALLDATETIME NULL,
    [enddate]    SMALLDATETIME NULL
);


GO
CREATE NONCLUSTERED INDEX [IDX_UPC_VDR]
    ON [dbo].[item_cost_temp]([identifier] ASC, [vendor_key] ASC);

