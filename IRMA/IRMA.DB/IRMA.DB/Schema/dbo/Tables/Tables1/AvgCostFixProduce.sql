CREATE TABLE [dbo].[AvgCostFixProduce] (
    [Identifier]  VARCHAR (13) NULL,
    [Description] VARCHAR (65) NULL,
    [Pack]        INT          NULL,
    [Unit]        VARCHAR (10) NULL,
    [Cost]        MONEY        NULL,
    [AvgCost]     MONEY        NULL,
    [Dist Margin] NUMERIC (18) NULL,
    [LandedCost]  MONEY        NULL,
    [UnitCost]    MONEY        NULL,
    [Item_key]    INT          NULL,
    [Fixed]       BIT          CONSTRAINT [DF_AvgCostFixProduce_Fixed] DEFAULT ((0)) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostFixProduce] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostFixProduce] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostFixProduce] TO [IRMAReportsRole]
    AS [dbo];

