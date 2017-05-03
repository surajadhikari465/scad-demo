CREATE TABLE [dbo].[Sales_Fact] (
    [Time_Key]            DATETIME       NOT NULL,
    [Store_No]            INT            NOT NULL,
    [Transaction_No]      INT            NOT NULL,
    [Register_No]         SMALLINT       NOT NULL,
    [Row_No]              SMALLINT       NOT NULL,
    [SubTeam_No]          INT            NOT NULL,
    [Cashier_ID]          SMALLINT       NOT NULL,
    [Item_Key]            INT            NOT NULL,
    [Taxed]               BIT            CONSTRAINT [DF__Sales_Fac__Taxed__0FBA7E0F] DEFAULT ((0)) NULL,
    [Tax_Table]           TINYINT        NULL,
    [Price_Level]         TINYINT        NULL,
    [Food_Stamp]          BIT            CONSTRAINT [DF__Sales_Fac__Food___10AEA248] DEFAULT ((0)) NULL,
    [Sales_Quantity]      SMALLINT       NULL,
    [Return_Quantity]     SMALLINT       NULL,
    [Weight]              DECIMAL (9, 2) NULL,
    [Sales_Amount]        DECIMAL (9, 2) NULL,
    [Return_Amount]       DECIMAL (9, 2) NULL,
    [Markdown_Amount]     DECIMAL (9, 2) NULL,
    [Promotion_Amount]    DECIMAL (9, 2) NULL,
    [Store_Coupon_Amount] DECIMAL (9, 2) NULL,
    [Scan_Type]           TINYINT        CONSTRAINT [DF_Sales_Fact_Scan_Type] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Sales_Fact__0AF5C8F2] PRIMARY KEY CLUSTERED ([Time_Key] ASC, [Store_No] ASC, [Transaction_No] ASC, [Register_No] ASC, [Row_No] ASC) WITH (FILLFACTOR = 80) ON [Warehouse],
    CONSTRAINT [FK_Sales_Fact_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
CREATE NONCLUSTERED INDEX [idxTime_Key]
    ON [dbo].[Sales_Fact]([Time_Key] ASC) WITH (FILLFACTOR = 80)
    ON [Warehouse];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_Fact] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_Fact] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_Fact] TO [IRMAReportsRole]
    AS [dbo];

