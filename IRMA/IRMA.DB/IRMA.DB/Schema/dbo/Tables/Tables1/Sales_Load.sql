CREATE TABLE [dbo].[Sales_Load] (
    [Time_Key]            DATETIME       NOT NULL,
    [Store_No]            INT            NOT NULL,
    [Transaction_No]      INT            NOT NULL,
    [Register_No]         SMALLINT       NOT NULL,
    [Row_No]              SMALLINT       NOT NULL,
    [SubTeam_No]          INT            NOT NULL,
    [Cashier_ID]          SMALLINT       NOT NULL,
    [Item_Key]            INT            NOT NULL,
    [Taxed]               BIT            CONSTRAINT [DF__Sales_Loa__Taxed__36D44B30] DEFAULT ((0)) NULL,
    [Tax_Table]           TINYINT        NULL,
    [Price_Level]         TINYINT        NULL,
    [Food_Stamp]          BIT            CONSTRAINT [DF__Sales_Loa__Food___37C86F69] DEFAULT ((0)) NULL,
    [Sales_Quantity]      SMALLINT       NULL,
    [Return_Quantity]     SMALLINT       NULL,
    [Weight]              DECIMAL (9, 2) NULL,
    [Sales_Amount]        DECIMAL (9, 2) NULL,
    [Return_Amount]       DECIMAL (9, 2) NULL,
    [Markdown_Amount]     DECIMAL (9, 2) NULL,
    [Promotion_Amount]    DECIMAL (9, 2) NULL,
    [Store_Coupon_Amount] DECIMAL (9, 2) NULL,
    [Scan_Type]           TINYINT        CONSTRAINT [DF_Sales_Load_Scan_Type] DEFAULT ((0)) NOT NULL
) ON [Warehouse];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_Load] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_Load] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_Load] TO [IRMAReportsRole]
    AS [dbo];

