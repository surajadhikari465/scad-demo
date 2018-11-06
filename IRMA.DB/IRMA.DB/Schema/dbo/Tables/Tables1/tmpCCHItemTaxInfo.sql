CREATE TABLE [dbo].[tmpCCHItemTaxInfo] (
    [Store_No]     INT            NULL,
    [Item_Key]     INT            NULL,
    [TaxFlagKey]   CHAR (1)       NULL,
    [TaxFlagValue] BIT            NULL,
    [TaxPercent]   DECIMAL (9, 4) NULL,
    [POSID]        INT            NULL
);

