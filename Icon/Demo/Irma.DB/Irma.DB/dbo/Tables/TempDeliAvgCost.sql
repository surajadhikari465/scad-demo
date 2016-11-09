CREATE TABLE [dbo].[TempDeliAvgCost] (
    [Store_No] INT        NULL,
    [Item_Key] INT        NULL,
    [PHID]     INT        NULL,
    [AvgCost]  SMALLMONEY NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[TempDeliAvgCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempDeliAvgCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempDeliAvgCost] TO [IRMAReportsRole]
    AS [dbo];

