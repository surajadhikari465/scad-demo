CREATE TABLE [dbo].[CostImports] (
    [Item_ID]          NVARCHAR (255) NULL,
    [UPC]              NVARCHAR (255) NULL,
    [Item_Description] NVARCHAR (255) NULL,
    [MSRP]             MONEY          NULL,
    [Case_Size]        FLOAT (53)     NULL,
    [Net_Price]        FLOAT (53)     NULL,
    [Status Flag]      NVARCHAR (255) NULL,
    [Start_Date]       SMALLDATETIME  NULL,
    [End_Date]         SMALLDATETIME  NULL,
    [Deal_Quantity]    NVARCHAR (255) NULL,
    [Deal_Case_Amt]    NVARCHAR (255) NULL,
    [Deal_Start_Date]  NVARCHAR (255) NULL,
    [Deal_End_Date]    NVARCHAR (255) NULL,
    [Deal_Type]        NVARCHAR (255) NULL,
    [Item_Key]         INT            NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CostImports] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CostImports] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CostImports] TO [IRMAReportsRole]
    AS [dbo];

