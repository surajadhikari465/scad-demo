CREATE TABLE [dbo].[VIMRetailPriceLoad] (
    [UPC]           VARCHAR (13) NULL,
    [REGION]        VARCHAR (2)  NOT NULL,
    [PS_BU]         INT          NULL,
    [PRICEZONE]     INT          NULL,
    [POS_DEPT]      VARCHAR (3)  NULL,
    [REG_PRICE]     SMALLMONEY   NOT NULL,
    [REG_MULTIPLE]  TINYINT      NOT NULL,
    [EFF_PRICE]     SMALLMONEY   NOT NULL,
    [EFF_MULTIPLE]  TINYINT      NOT NULL,
    [EFF_PRICETYPE] VARCHAR (4)  NULL,
    [START_DATE]    VARCHAR (10) NULL,
    [END_DATE]      VARCHAR (10) NULL,
    [EFF_DATE]      VARCHAR (10) NULL,
    [PROMO_CODE]    VARCHAR (4)  NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [idxEffPrice]
    ON [dbo].[VIMRetailPriceLoad]([EFF_PRICE] ASC, [UPC] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxUPC]
    ON [dbo].[VIMRetailPriceLoad]([UPC] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUPCStore]
    ON [dbo].[VIMRetailPriceLoad]([UPC] ASC, [PS_BU] ASC) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [Statistic_REGION]
    ON [dbo].[VIMRetailPriceLoad]([REGION]);


GO
CREATE STATISTICS [Statistic_PRICEZONE]
    ON [dbo].[VIMRetailPriceLoad]([PRICEZONE]);


GO
CREATE STATISTICS [Statistic_POS_DEPT]
    ON [dbo].[VIMRetailPriceLoad]([POS_DEPT]);


GO
CREATE STATISTICS [Statistic_REG_MULTIPLE]
    ON [dbo].[VIMRetailPriceLoad]([REG_MULTIPLE]);


GO
CREATE STATISTICS [Statistic_EFF_MULTIPLE]
    ON [dbo].[VIMRetailPriceLoad]([EFF_MULTIPLE]);


GO
CREATE STATISTICS [Statistic_EFF_PRICETYPE]
    ON [dbo].[VIMRetailPriceLoad]([EFF_PRICETYPE]);


GO
CREATE STATISTICS [Statistic_START_DATE]
    ON [dbo].[VIMRetailPriceLoad]([START_DATE]);


GO
CREATE STATISTICS [Statistic_END_DATE]
    ON [dbo].[VIMRetailPriceLoad]([END_DATE]);


GO
CREATE STATISTICS [Statistic_EFF_DATE]
    ON [dbo].[VIMRetailPriceLoad]([EFF_DATE]);


GO
CREATE STATISTICS [Statistic_PROMO_CODE]
    ON [dbo].[VIMRetailPriceLoad]([PROMO_CODE]);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VIMRetailPriceLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VIMRetailPriceLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VIMRetailPriceLoad] TO [IRMAReportsRole]
    AS [dbo];

