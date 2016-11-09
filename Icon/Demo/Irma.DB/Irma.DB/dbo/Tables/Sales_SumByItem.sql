CREATE TABLE [dbo].[Sales_SumByItem] (
    [Date_Key]            SMALLDATETIME   NOT NULL,
    [Store_No]            INT             NOT NULL,
    [Item_Key]            INT             NOT NULL,
    [SubTeam_No]          INT             NOT NULL,
    [Price_Level]         TINYINT         NOT NULL,
    [Sales_Quantity]      INT             NULL,
    [Return_Quantity]     INT             NULL,
    [Weight]              DECIMAL (10, 3) NULL,
    [Sales_Amount]        DECIMAL (9, 2)  NULL,
    [Return_Amount]       DECIMAL (9, 2)  NULL,
    [Markdown_Amount]     DECIMAL (9, 2)  NULL,
    [Promotion_Amount]    DECIMAL (9, 2)  NULL,
    [Store_Coupon_Amount] DECIMAL (9, 2)  NULL,
    CONSTRAINT [PK_Sales_SumByItem] PRIMARY KEY CLUSTERED ([Date_Key] ASC, [Store_No] ASC, [Item_Key] ASC, [SubTeam_No] ASC, [Price_Level] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__Sales_Sum__Store__1943E849] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Sales_SumByItem_Date] FOREIGN KEY ([Date_Key]) REFERENCES [dbo].[Date] ([Date_Key]),
    CONSTRAINT [FK_Sales_SumByItem_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_Sales_SumByItem_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);




GO



GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItem] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItem] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItem] TO [ExtractRole]
    AS [dbo];


GO
CREATE NONCLUSTERED INDEX [IX_Sales_SumByItem_Item_Key_Date_Key]
    ON [dbo].[Sales_SumByItem]([Item_Key] ASC, [Date_Key] ASC) WITH (FILLFACTOR = 80);

