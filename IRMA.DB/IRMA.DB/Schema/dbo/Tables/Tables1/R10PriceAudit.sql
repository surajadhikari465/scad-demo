CREATE TABLE [dbo].[R10PriceAudit] (
    [Code]            NVARCHAR (60)   NULL,
    [R10_Price]       DECIMAL (19, 5) NULL,
    [R10_PM]          DECIMAL (9, 3)  NULL,
    [EffectiveDate]   DATETIME        NULL,
    [ExpirationDate]  DATETIME        NULL,
    [BusinessUnit_Id] NVARCHAR (20)   NULL,
    [UOM]             NVARCHAR (5)    NULL,
    [rn]              INT             NULL,
    [InsertDate]      DATETIME        NULL
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[R10PriceAudit] TO [WFM.R10.Operations.IRMAPriceAudit]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[R10PriceAudit] TO [WFM.R10.Operations.IRMAPriceAudit]
    AS [dbo];

