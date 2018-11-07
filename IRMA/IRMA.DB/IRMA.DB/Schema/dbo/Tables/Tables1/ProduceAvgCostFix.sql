CREATE TABLE [dbo].[ProduceAvgCostFix] (
    [Identifier]       VARCHAR (13)  NULL,
    [Item Description] VARCHAR (65)  NULL,
    [Effective Date]   SMALLDATETIME NULL,
    [UnitAvgCost]      MONEY         NULL,
    [CaseAvgCost]      MONEY         NULL,
    [Item_Key]         INT           NULL,
    [Deleted]          BIT           CONSTRAINT [DF_ProduceAvgCostFix_Deleted] DEFAULT ((0)) NOT NULL,
    [Effective_Date]   SMALLDATETIME NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProduceAvgCostFix] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProduceAvgCostFix] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ProduceAvgCostFix] TO [IRMAReportsRole]
    AS [dbo];

