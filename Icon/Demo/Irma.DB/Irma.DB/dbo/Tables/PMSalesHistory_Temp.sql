CREATE TABLE [dbo].[PMSalesHistory_Temp] (
    [Store_No]      INT             NULL,
    [Item_Key]      INT             NULL,
    [FiscalYear]    INT             NULL,
    [FiscalPeriod]  INT             NULL,
    [FiscalWeek]    INT             NULL,
    [Unit_Quantity] DECIMAL (18, 2) NULL,
    [RetailSales]   MONEY           NULL,
    [Cost]          MONEY           NULL,
    [Pending_Cost]  MONEY           NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMSalesHistory_Temp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMSalesHistory_Temp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMSalesHistory_Temp] TO [IRMAReportsRole]
    AS [dbo];

