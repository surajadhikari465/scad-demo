CREATE TABLE [dbo].[tmpIndexDefragList] (
    [ObjectName]     CHAR (255)   NULL,
    [ObjectId]       INT          NULL,
    [IndexName]      CHAR (255)   NULL,
    [IndexId]        INT          NULL,
    [Lvl]            INT          NULL,
    [CountPages]     INT          NULL,
    [CountRows]      INT          NULL,
    [MinRecSize]     INT          NULL,
    [MaxRecSize]     INT          NULL,
    [AvgRecSize]     INT          NULL,
    [ForRecCount]    INT          NULL,
    [Extents]        INT          NULL,
    [ExtentSwitches] INT          NULL,
    [AvgFreeBytes]   INT          NULL,
    [AvgPageDensity] INT          NULL,
    [ScanDensity]    DECIMAL (18) NULL,
    [BestCount]      INT          NULL,
    [ActualCount]    INT          NULL,
    [LogicalFrag]    DECIMAL (18) NULL,
    [ExtentFrag]     DECIMAL (18) NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpIndexDefragList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpIndexDefragList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpIndexDefragList] TO [IRMAReportsRole]
    AS [dbo];

