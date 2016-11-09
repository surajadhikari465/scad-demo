CREATE TABLE [dbo].[R10PriceAudit] (
    [code]            NVARCHAR (60)   NULL,
    [R10_Price]       DECIMAL (19, 5) NULL,
    [R10_PM]          DECIMAL (9, 3)  NULL,
    [EffectiveDate]   DATETIME        NULL,
    [ExpirationDate]  DATETIME        NULL,
    [BusinessUnit_Id] NVARCHAR (20)   NULL,
    [UOM]             NVARCHAR (5)    NULL,
    [rn]              INT             NULL,
    [insertdate]      DATETIME        NULL
);

