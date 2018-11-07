CREATE TABLE [dbo].[Payment_Load] (
    [Time_Key]       DATETIME       NOT NULL,
    [Store_No]       INT            NOT NULL,
    [Transaction_No] INT            NOT NULL,
    [Register_No]    SMALLINT       NOT NULL,
    [Cashier_ID]     SMALLINT       NOT NULL,
    [Row_No]         SMALLINT       NOT NULL,
    [Payment_Type]   SMALLINT       NOT NULL,
    [Payment_Amount] DECIMAL (9, 2) NULL,
    [Payment_Date]   CHAR (10)      NULL,
    [Payment_ID]     CHAR (30)      NULL,
    [Payment_Misc]   CHAR (30)      NULL
) ON [Warehouse];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_Load] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_Load] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_Load] TO [IRMAReportsRole]
    AS [dbo];

