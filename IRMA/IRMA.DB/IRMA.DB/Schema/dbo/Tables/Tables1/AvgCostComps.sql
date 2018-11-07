CREATE TABLE [dbo].[AvgCostComps] (
    [Store_No]         INT           NULL,
    [SubTeam_No]       INT           NULL,
    [Category_ID]      INT           NULL,
    [CategoryName]     VARCHAR (60)  NULL,
    [Identifier]       VARCHAR (13)  NULL,
    [Item_Description] VARCHAR (60)  NULL,
    [On_Sale]          BIT           NULL,
    [Price]            SMALLMONEY    NULL,
    [Margin]           DECIMAL (18)  NULL,
    [AvgCost]          SMALLMONEY    NULL,
    [AvgCostUpdated]   SMALLDATETIME NULL,
    [LastCost]         SMALLMONEY    NULL,
    [DateReceived]     SMALLDATETIME NULL,
    [CostDiff]         SMALLMONEY    NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostComps] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostComps] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AvgCostComps] TO [IRMAReportsRole]
    AS [dbo];

